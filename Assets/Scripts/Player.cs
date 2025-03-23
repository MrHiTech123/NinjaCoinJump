using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
	
	[SerializeField] private TMP_Text scoreboard;
	
	[SerializeField] private GameObject coinProjectile;
	private Animator playerAnimator;
	
	private LinkedList<GameObject> thrownCoins = new LinkedList<GameObject>();
	private LinkedListNode<GameObject> currentThrownCoin;
	Rigidbody2D body;
	// Start is called before the first frame update

	void Awake()
	{
		GameManager.coinText = scoreboard;
		playerAnimator = GetComponent<Animator>();
		
		
		// TODO: Move to LevelManager
		Application.targetFrameRate = Consts.frameRate;
	}
	void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
	
	private float speed = 10f;
	private float jumpHeight = 4f;
	private float throwPower = 2f;
	
	private float horizontalMove;
	
	private bool isGrounded = true;
	
	void Jump() {
		if (!isGrounded) {
			return;
		}
		isGrounded = false;
		body.velocity = new Vector2(body.velocity.x, jumpHeight);
		AudioManager.PlayAudio("jump");
	}
	
	
	private Vector3 PositionOfThrownCoin() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		return new Vector3(
			Mathf.Clamp(mousePos.x, transform.position.x - (GetComponent<BoxCollider2D>().size.x / 2), transform.position.x + (GetComponent<BoxCollider2D>().size.x / 2)),
			Mathf.Clamp(mousePos.y, transform.position.y - (GetComponent<BoxCollider2D>().size.y / 2), transform.position.y + (GetComponent<BoxCollider2D>().size.y / 2)),
			transform.position.z
		);
		
	}
	
	
	private Vector2 VelocityOfThrownCoin() {
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return (body.velocity) + (new Vector2(mousePos.x, mousePos.y).normalized * throwPower);
	}
	
	void ThrowCoin() {
		thrownCoins.AddLast(Instantiate(coinProjectile, PositionOfThrownCoin(), transform.rotation));
		currentThrownCoin = thrownCoins.Last;
		
		currentThrownCoin.Value.GetComponent<Rigidbody2D>().velocity = VelocityOfThrownCoin();
		
	}
	
	void MoveAwayFromCoin() {
		if (currentThrownCoin == null || currentThrownCoin.Value == null) {
			return;
		}
		Movement.BothMaybeMoveAwayDistScaled(this.gameObject, currentThrownCoin.Value);
		
		Coin coin = currentThrownCoin.Value.GetComponent<Coin>();
		
		
		body.velocity = new Vector2(
			body.velocity.x + (coin.movementMissedAmount.x * (coin.GetComponent<Rigidbody2D>().mass / GetComponent<Rigidbody2D>().mass)),
			body.velocity.y + (coin.movementMissedAmount.y * (coin.GetComponent<Rigidbody2D>().mass / GetComponent<Rigidbody2D>().mass))
		);
		
		
	}
	
	void Move() {
		if (Input.GetKey(KeyCode.LeftArrow) && isGrounded) {
			horizontalMove = -speed;
		}
		else if (Input.GetKey(KeyCode.RightArrow) && isGrounded) {
			horizontalMove = speed;
		}
		else if (isGrounded) {
			horizontalMove = 0;
		}
		
		body.velocity = new Vector2(horizontalMove, body.velocity.y);
		
		if (Input.GetKey(KeyCode.UpArrow)) {
			Jump();
		}
		if (Input.GetMouseButtonDown((int)UnityEngine.UIElements.MouseButton.LeftMouse)) {
			ThrowCoin();
		}
		if (Input.GetMouseButton((int)UnityEngine.UIElements.MouseButton.LeftMouse) || Input.GetKey(KeyCode.Space)) {
			MoveAwayFromCoin();
		}
		
	}
	void Animate() {
		
		horizontalMove = GetComponent<Rigidbody2D>().velocity.x;
		
		if (!isGrounded) {
			playerAnimator.Play("jump");
		}
		else if (horizontalMove <= -speed || horizontalMove >= speed) {
			playerAnimator.Play("running");
		}
		else {
			playerAnimator.Play("idle");
		}
		
		if (horizontalMove < 0) {
			GetComponent<SpriteRenderer>().flipX = true;
		}
		
		if (horizontalMove > 0) {
			GetComponent<SpriteRenderer>().flipX = false;
		}
	}
	
    // Update is called once per frame
    void Update()
    {
        Move();
		Animate();
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground")) {
			isGrounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground")) {
			isGrounded = false;
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Collectible")) {
			GameManager.CollectCoin();
			Debug.Log(GameManager.GetCollectedCoins());
			AudioManager.PlayAudio("collectCoin");
			Destroy(collision.gameObject);
		}
	}
	
}
