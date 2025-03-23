using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Coin : MonoBehaviour
{
	private Rigidbody2D body;
	private Vector3 prevPos;
	private Vector3 prevVel;
    
	
	public Vector3 movementMissedAmount {get; private set;}
	
	void SaveAttributes() {
		prevPos = body.position;
		prevVel = body.velocity;
	}
	
    void Start()
    {
		body = GetComponent<Rigidbody2D>();
		SaveAttributes();
    }
	
	private Vector3 PredictedPosition() {
		return prevPos + prevVel;
	}
	
	private float MissedXMovement() {
		return (transform.position.x / Consts.frameRate) - (prevVel.x + prevPos.x);
	}
	
	private float MissedYMovement() {
		return (prevVel.y / Consts.frameRate) + prevPos.y - transform.position.y;
	}
	
	private void PosPredictedCorrectly() {
		movementMissedAmount = new Vector3(MissedXMovement(), MissedYMovement(), 0);
	}
	
    // Update is called once per frame
    void Update()
    {
        PosPredictedCorrectly();
		
    }
}
