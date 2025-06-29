using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
	public int speedThreshhold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void Break() {
		AudioManager.PlayAudio("blockBreak");
		Destroy(this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("CoinProjectile")) {
			if (collision.relativeVelocity.magnitude > speedThreshhold) {
				Break();
			}
		}
	}
}
