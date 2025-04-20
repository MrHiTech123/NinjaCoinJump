using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	
	protected float speed = 2.5f;
	protected float safeDistance = 15f;
	protected GameObject player;
	protected Vector3 startPos;
	protected Vector3 endPos;
	protected Vector3 targetPos;
	protected bool chasingPlayer = false;
	
	[SerializeField] private float patrolSize;
	
	private Animator animator;
	private Rigidbody2D body;
	private bool dying;
	
	private const float animationMovementThreshhold = 0.0001f;
	private const float speedThreshholdDeath = 100;
	private float horizontalMove = 0;
	
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		startPos = transform.position;
		targetPos = startPos;
		endPos = startPos + new Vector3(patrolSize, 0, 0);
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();
    }
	
	// Protected = visible to things that inherit this
	// Virtual = can be overwritten by child class
	protected virtual void MoveToPlayer() {		
		targetPos = player.transform.position;
	}
	
	public bool isDying() {
		return dying;
	}
	protected virtual void Patrol() {
		if (transform.position.x <= startPos.x) {
			targetPos = endPos;
		}
		else if (transform.position.x >= endPos.x) {
			targetPos = startPos;
		}
		else if (transform.position == targetPos) {
			targetPos = startPos;
		}
		
	}
	
	protected virtual void DoMovement() {
		float currentDistance = Vector2.Distance(transform.position, player.transform.position);
		if (currentDistance > safeDistance) {
			if (chasingPlayer) {
				targetPos = startPos;
				chasingPlayer = false;
			}
			Patrol();
		}
		else {
			chasingPlayer = true;
			MoveToPlayer();
		}
		Debug.Log("Start");
		Debug.Log(speed);
		Debug.Log(Time.deltaTime);
		Debug.Log(targetPos);
		Debug.Log("end");
		Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
		transform.position = new Vector3(newPos.x, transform.position.y, transform.position.z);
	}
	
	void GetData() {
		horizontalMove = targetPos.x - transform.position.x;
	}
	void SetData() {
		
	}
	
	void Animate() {
		if (horizontalMove <= - animationMovementThreshhold || horizontalMove >= animationMovementThreshhold) {
			animator.Play("walk");
		}
		else {
			animator.Play("idle");
		}
		
		if (horizontalMove < - animationMovementThreshhold) {
			GetComponent<SpriteRenderer>().flipX = true;
		}
		
		if (horizontalMove > animationMovementThreshhold) {
			GetComponent<SpriteRenderer>().flipX = false;
		}
	}
	
	IEnumerator DestroySelfAfterAnimation() {
		yield return new WaitForSeconds(1.5f);
		Destroy(this.gameObject);
	}
	
	void HaltVelocity() {
		body.velocity = new Vector2();
	}
	void Die() {
		dying = true;
		body.isKinematic = false;
		StartCoroutine(DestroySelfAfterAnimation());
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("CoinProjectile")) {
			if (collision.relativeVelocity.magnitude > speedThreshholdDeath) {
				Die();
			}
		}
	}
	
	void LivingBehavior() {
		GetData();
    	DoMovement();
		Animate();
		SetData();
	}
	
	void AnimateDeath() {
		animator.Play("die");
	}
	
	void DeathBehavior() {
		AnimateDeath();
	}
	// Update is called once per frame
	void Update()
    {
		HaltVelocity();
		if (!dying) {
			LivingBehavior();
		}
		else {
			DeathBehavior();
		}
    }
}
