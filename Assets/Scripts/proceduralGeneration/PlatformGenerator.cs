using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
	protected int platformWidth;
	protected int platformHeight;
	public float xSpacing;
	public float ySpacing;
	public GameObject[] tilePrefabs;
	
	public void GeneratePlatform() {
		float offsetX = platformWidth * -xSpacing / 2f;
		float offsetY = platformHeight * ySpacing / 2f;
		
		for (int i = 0; i < platformWidth; ++i) {
			for (int j = 0; j < platformHeight; ++j) {
				Debug.Log("Placing object " + i + " " + j);
				GameObject tile = GenerateTile(i, j);
				if (tile != null) {
					Debug.Log("\tWas indeed placed");
					tile = Instantiate(tile, transform);
					tile.transform.localScale = new Vector3(xSpacing, xSpacing, xSpacing);
					tile.transform.position = new Vector3(offsetX + xSpacing * i, offsetY + ySpacing * j, transform.position.z);
				}
			}
		}
		
	}
	
	protected virtual GameObject GenerateTile(int x, int y) {
		return null;
		// return tilePrefabs[Random.Range(0, tilePrefabs.Length - 1)];
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
