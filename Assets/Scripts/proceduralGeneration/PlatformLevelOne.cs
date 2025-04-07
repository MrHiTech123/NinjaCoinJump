using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLevelOne : PlatformGenerator
{
	
	private string[] platformString = new string[] {
		"eeee----------------------ewwwwwwwwwwwwwwggggg",
		"eeee----------------------ewweeeeeeeeeewwggggg",
		"eeee----------------------ewweeeeeeeeeewwggggg",
		"eeeeeeee----gw-----eeeeeeeewweeeeeeeeeewwggggg",
		"eeeeeeee----gw-----eeeeeeeewwwwwwwwwwwwwwggggg",
	};

	protected override GameObject GenerateTile(int x, int y)
	{
		char c = platformString[y].ToCharArray()[x];
		switch (c) {
			case 'e':
				return tilePrefabs[0];
			case 'g':
				return tilePrefabs[1];
			case 'w':
				return tilePrefabs[2];
			default:
				return null;
		}
		return base.GenerateTile(x, y);
	}
	
    // Start is called before the first frame update
    void Start()
    {
        platformWidth = platformString[0].Length;
		platformHeight = platformString.Length;
		GeneratePlatform();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
