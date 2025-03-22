using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	public GameObject player;
	
	[SerializeField] private float cameraWidth;
	[SerializeField] private float spriteWidth;
	[SerializeField] private float cameraHeight;
	
	[SerializeField] private float minX = -10;
	[SerializeField] private float maxX = 10;
	[SerializeField] private float minY = -5;
	[SerializeField] private float maxY = 5;
	
	
    // Start is called before the first frame update
    void Start()
    {
        cameraHeight = Camera.main.orthographicSize * 2f;
		cameraWidth = cameraHeight * Camera.main.aspect;
		spriteWidth = player.GetComponent<SpriteRenderer>().bounds.size.x;
    }
	
	float AvgTwo(float a, float b) {
		return (a + b) / 2;
	}
    // Update is called once per frame
    void Update()
    {
		float x = player.transform.position.x;
		float y = player.transform.position.y;
		float clampedX = Mathf.Clamp(x, minX, maxX);
		float clampedY = Mathf.Clamp(y, minY, maxY);
        transform.position = new Vector3(
			clampedX,
			clampedY,
			transform.position.z
		);
    }
}
