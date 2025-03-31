using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	protected override void DoMovement() {
		MoveToPlayer();
	}
	
    // Update is called once per frame
    void Update()
    {
        
    }
}
