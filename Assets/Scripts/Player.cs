using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	
	[SerializeField] private TMP_Text scoreboard;
	
	[SerializeField] private GameObject coinProjectile;
	private Animator playerAnimator;
	private BoxCollider2D boxCollider;
	
	private LinkedList<GameObject> thrownCoins = new LinkedList<GameObject>();
	private LinkedListNode<GameObject> currentThrownCoin;
	private const float pickUpThreshhold = 1;
	
	[SerializeField] private float respawnHeight = -20;
	
	Rigidbody2D body;
	// Start is called before the first frame update

	void Awake()
	{
		GameManager.coinText = scoreboard;
		playerAnimator = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
		// TODO: Move to LevelManager
		Application.targetFrameRate = Consts.frameRate;
	}
	void Start()
    {
        body = GetComponent<Rigidbody2D>();
		GameManager.SetSpawnPoint(transform.position, false);
    }
	
	private float maxSpeed = 15f;
	private float acceleration = 0.2f;
	private float jumpHeight = 4f;	
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
	
	
	private Vector3 PositionOfThrownCoinMouse() {
		// Algorithm adapted from the one in the top answer found here
		// https://stackoverflow.com/questions/3180000/calculate-a-vector-from-a-point-in-a-rectangle-to-edge-based-on-angle
		// I didn't read any of the other answers, and the algorithm definitely took up residence in my head
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		Vector2 mousePosRelativeToPlayer = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
		
		float sinAngle = mousePosRelativeToPlayer.y / mousePosRelativeToPlayer.magnitude;
		float cosAngle = mousePosRelativeToPlayer.x / mousePosRelativeToPlayer.magnitude;
		
		float tHorizontal = boxCollider.size.x / (2 * cosAngle);
		float tVertical = boxCollider.size.y / (2 * sinAngle);
		float t = Mathf.Min(Mathf.Abs(tHorizontal), Mathf.Abs(tVertical));
		
		Vector2 placedPosRelativeToPlayer = new Vector2(
			t * cosAngle,
			t * sinAngle
		);
		
		Vector2 pos = new Vector2(placedPosRelativeToPlayer.x + transform.position.x, placedPosRelativeToPlayer.y + transform.position.y);
		
		return new Vector3(pos.x, pos.y, 0);
		
	}
	
	void HaltMovement() {
		body.velocity = new Vector2(0, 0);
		horizontalMove = 0;
	}
	void Respawn() {
		Vector3 spawnPoint = GameManager.GetSpawnPoint();
		if (GameManager.CheckPointsReached) {
			transform.position = spawnPoint;
			HaltMovement();
		}
		else {
			ResetScene();
		}
		
	}

	void Die()
	{
		// AudioManager.PlayAudio("playerDies");
		Respawn();
	}
	
	
	void DieIfYCoordTooLow() {
		if (transform.position.y < respawnHeight) {
			Die();
		}
	}
	
	private Vector2 VelocityOfThrownCoin() {
		return body.velocity;
	}
	
	void ReassignCurrentThrownCoin(LinkedListNode<GameObject> toReassign) {
		if (currentThrownCoin != null && currentThrownCoin.Value != null) {
			currentThrownCoin.Value.GetComponent<Coin>().Glowing = false;
		}
		currentThrownCoin = toReassign;
		currentThrownCoin.Value.GetComponent<Coin>().Glowing = true;
	}
	
	void IncrementCurrentThrownCoin() {
		if (currentThrownCoin.Next != null) {
			ReassignCurrentThrownCoin(currentThrownCoin.Next);
		}
	}
	
	void DecrementCurrentThrownCoin() {
		if (currentThrownCoin.Previous != null) {
			ReassignCurrentThrownCoin(currentThrownCoin.Previous);
		}
	}
	
	LinkedListNode<GameObject> ClosestCoinNode() {
		LinkedListNode<GameObject> closest = thrownCoins.First;
		if (closest == null) {
			return null;
		}
		float biggestDist = Vector2.Distance(closest.Value.transform.position, transform.position);
		for (LinkedListNode<GameObject> c = thrownCoins.First; c != null; c = c.Next) {
			float maybeDist = Vector2.Distance(c.Value.transform.position, transform.position);
			
			if (maybeDist <= biggestDist) {
				closest = c;
				biggestDist = maybeDist;
			}
		}
		return closest;
	}
	void MakeClosestCoinCurrentThrownCoin() {
		if (thrownCoins.Count > 0) {
			ReassignCurrentThrownCoin(ClosestCoinNode());
		}
	}
	
	void ThrowCoin(Vector3 pos) {
		if (GameManager.GetCollectedCoins() <= 0) {
			return;
		}
		
		GameManager.DeCollectCoin();
		
		thrownCoins.AddLast(Instantiate(coinProjectile, pos, transform.rotation));
		ReassignCurrentThrownCoin(thrownCoins.Last);
		
		currentThrownCoin.Value.GetComponent<Rigidbody2D>().velocity = VelocityOfThrownCoin();
	}
	
	void ThrowCoin() {
		ThrowCoin(PositionOfThrownCoinMouse());
	}
	
	
	void CoinJump() {
		Jump();
		ThrowCoin(new Vector3(transform.position.x, transform.position.y - (boxCollider.size.y / 2), transform.position.z));
	}
	
	void MoveAwayFromCoin() {
		if (currentThrownCoin == null || currentThrownCoin.Value == null) {
			return;
		}
		Movement.BothMaybeMoveAwayDistScaled(this.gameObject, currentThrownCoin.Value);
	}
	
	void GetData() {
		horizontalMove = body.velocity.x;
	}
	void SetData() {
		body.velocity = new Vector2(horizontalMove, body.velocity.y);
	}
	
	void Move() {
		if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && isGrounded) {
			horizontalMove = Mathf.Clamp(horizontalMove - acceleration, -maxSpeed, 0);
		}
		else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && isGrounded) {
			horizontalMove = Mathf.Clamp(horizontalMove + acceleration, 0, maxSpeed);
		}
		else if (isGrounded) {
			horizontalMove = 0;
		}
		
		body.velocity = new Vector2(horizontalMove, body.velocity.y);
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			CoinJump();
		}
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			Jump();
		}
		if (Input.GetMouseButtonDown((int)UnityEngine.UIElements.MouseButton.LeftMouse)) {
			ThrowCoin();
		}
		if (Input.GetMouseButton((int)UnityEngine.UIElements.MouseButton.LeftMouse) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.C)) {
			MoveAwayFromCoin();
		}
		if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.V)) {
			MakeClosestCoinCurrentThrownCoin();
		}
		
	}
	void Animate() {		
		if (!isGrounded) {
			playerAnimator.Play("jump");
		}
		else if (horizontalMove <= -acceleration || horizontalMove >= acceleration) {
			playerAnimator.Play("running");
		}
		else {
			playerAnimator.Play("idle");
		}
		
		if (horizontalMove < -acceleration) {
			GetComponent<SpriteRenderer>().flipX = true;
		}
		
		if (horizontalMove > acceleration) {
			GetComponent<SpriteRenderer>().flipX = false;
		}
	}
	
	void ResetScene() {
		GameManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
	void ResetSceneIfPlayerSaidSo() {
		if (Input.GetKeyDown(KeyCode.R)) {
			ResetScene();
		}
	}
	
	void PickUpCoin(GameObject toBePickedUp) {
		thrownCoins.Remove(toBePickedUp);
		Destroy(toBePickedUp);
		GameManager.CollectCoin();
		AudioManager.PlayAudio("collectCoin");
	}
	
	void PickUpClosestCoin() {
		LinkedListNode<GameObject> node = ClosestCoinNode();
		if (node == null) {
			return;
		}
		GameObject closestCoin = node.Value;
		if (Vector2.Distance(transform.position, closestCoin.transform.position) < pickUpThreshhold) {
			PickUpCoin(closestCoin);
		}
	}
	
	void MaybePickUpClosestCoin() {
		if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.X)) {
			PickUpClosestCoin();
		}
	}
	
    // Update is called once per frame
    void Update()
    {
		GetData();
		Move();
		Animate();
		MaybePickUpClosestCoin();
		ResetSceneIfPlayerSaidSo();
		DieIfYCoordTooLow();
		SetData();
    }
	
	private HashSet<GameObject> touchingGroundedObjects = new HashSet<GameObject>();
	
	
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (GameManager.IsGround(collision.gameObject)) {
			touchingGroundedObjects.Add(collision.gameObject);
			isGrounded = true;
		}
		if (collision.gameObject.CompareTag("Enemy")) {
			Enemy enemy = collision.gameObject.GetComponent<Enemy>();
			if (enemy == null) {
				return;
			}
			if (!enemy.isDying()) {
				Die();
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (GameManager.IsGround(collision.gameObject)) {
			touchingGroundedObjects.Remove(collision.gameObject);
			if (touchingGroundedObjects.Count == 0) {
				isGrounded = false;
			}
		}
	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Collectible")) {
			GameManager.CollectCoin();
			AudioManager.PlayAudio("collectCoin");
			Destroy(collision.gameObject);
			return;
		}
		if (collision.gameObject.CompareTag("Respawn")) {
			Vector3 spawnPoint = collision.gameObject.transform.position;
			spawnPoint.z = transform.position.z;
			GameManager.SetSpawnPoint(spawnPoint, true);
			collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
			return;
		}
	}
	
}
