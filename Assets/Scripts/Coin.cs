using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Coin : MonoBehaviour
{
	[SerializeField] private Sprite glowingSprite;
	[SerializeField] private Sprite nonGlowingSprite;
	private Rigidbody2D body;
    
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
				renderer.sprite = glowingSprite;
			}
			else {
				renderer.sprite = nonGlowingSprite;
			}
		}
	}
	
	public Vector3 movementMissedAmount {get; private set;}
	
    void Start()
    {
		body = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
		Glowing = true;
    }
	
    // Update is called once per frame
    void Update()
    {
		// if (glowing) Debug.Log(body.velocity.magnitude);
    }
}
