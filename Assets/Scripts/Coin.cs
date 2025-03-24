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
    
	private SpriteRenderer renderer;
	public bool glowing = false;
	
	public Vector3 movementMissedAmount {get; private set;}
	
	void SaveAttributes() {
		prevPos = body.position;
		prevVel = body.velocity;
		renderer = GetComponent<SpriteRenderer>();
	}
	
    void Start()
    {
		body = GetComponent<Rigidbody2D>();
		SaveAttributes();
    }
	
	private Color orange = new Color(255, 128, 0);
	
    // Update is called once per frame
    void Update()
    {
		if (glowing) {
			renderer.color = orange;
		}
		else {
			renderer.color = Color.yellow;
		}
    }
}
