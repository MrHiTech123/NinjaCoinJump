using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
	
	[SerializeField] private TMP_Text scoreboard;
	
	private Animator playerAnimator;
	Rigidbody2D body;
	// Start is called before the first frame update

	void Awake()
	{
		GameManager.coinText = scoreboard;
		playerAnimator = GetComponent<Animator>();
	}
	void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
	
	private float speed = 3f;
	private float jumpHeight = 10f;
	private bool isGrounded = true;
	
	void Jump() {
		if (!isGrounded) {
			return;
		}
		isGrounded = false;
		body.velocity = new Vector2(body.velocity.x, jumpHeight);
		AudioManager.PlayAudio("jump");
	}
	
    // Update is called once per frame
    void Update()
    {
		float horizontalMove = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) {
			body.velocity = new Vector2(-speed, body.velocity.y);
			horizontalMove = -0.1f;
		}
		else if (Input.GetKey(KeyCode.RightArrow)) {
			body.velocity = new Vector2(speed, body.velocity.y);
			horizontalMove = 0.1f;
		}
		else {
			body.velocity = new Vector2(0, body.velocity.y);
		}
		
		
		playerAnimator.SetFloat("HorizontalMove", horizontalMove);
		playerAnimator.SetBool("Jump", !isGrounded);
		
		if (horizontalMove < 0) {
			GetComponent<SpriteRenderer>().flipX = true;
		}
		
		if (horizontalMove > 0) {
			GetComponent<SpriteRenderer>().flipX = false;
		}
		
		Debug.Log(playerAnimator.GetFloat("HorizontalMove"));
		if (Input.GetKey(KeyCode.Space)) {
			Jump();
		}
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
