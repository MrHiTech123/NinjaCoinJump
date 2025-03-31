using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	
	[SerializeField] protected float speed = 5f;
	[SerializeField] protected float safeDistance = 5f;
	protected GameObject player;
	protected Vector3 startPos;
	protected Vector3 endPos;
	protected Vector3 targetPos;
	protected bool chasingPlayer = false;
	
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		startPos = transform.position;
		targetPos = startPos;
		endPos = startPos + new Vector3(20, 0, 0);
    }
	
	// Protected = visible to things that inherit this
	// Virtual = can be overwritten by child class
	protected virtual void MoveToPlayer() {
		targetPos = player.transform.position;
	}
	
	protected virtual void Patrol() {
		if (transform.position == startPos) {
			targetPos = endPos;
		}
		else if (transform.position == endPos) {
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
		transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
	}
	
    // Update is called once per frame
    void Update()
    {
        DoMovement();
		
		Debug.Log("Start");
		Debug.Log(startPos);
		Debug.Log(endPos);
		Debug.Log(targetPos);
		Debug.Log("End");
		
    }
}
