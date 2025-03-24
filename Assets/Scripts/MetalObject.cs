using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetalObject : MonoBehaviour
{
	private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }
	
	Vector3 DirectionOfPlayer() {
		return transform.position - player.transform.position;
	}
    // Update is called once per frame
    void Update()
    {
		Debug.Log("Drawing");
        Debug.DrawRay(transform.position, DirectionOfPlayer(), Color.cyan, 1);
    }
}
