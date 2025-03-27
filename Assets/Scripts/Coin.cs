using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Coin : MonoBehaviour
{
	private static Color orange = new Color(255, 128, 0);
	private static Color lightBlue = new Color(0f, 225f, 255f);
	
	private Rigidbody2D body;
	private Vector3 prevPos;
	private Vector3 prevVel;
    
	private SpriteRenderer renderer;
	
	private bool glowing;
	public bool Glowing {
		get {return glowing;}
		set {
			glowing = value;
			
			if (renderer == null) {
				return;
			}
			
			if (glowing) {
				renderer.color = lightBlue;
			}
			else {
				renderer.color = Color.yellow;
			}
		}
	}
	
	public Vector3 movementMissedAmount {get; private set;}
	
	void SaveAttributes() {
		prevPos = body.position;
		prevVel = body.velocity;
	}
	
    void Start()
    {
		body = GetComponent<Rigidbody2D>();
		SaveAttributes();
		renderer = GetComponent<SpriteRenderer>();
		// Glowing = false;
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }
}
