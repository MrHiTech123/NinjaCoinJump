using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	[SerializeField] private string destinationScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player")) {
			// SceneManager.LoadScene(destinationScene, LoadSceneMode.Single);
			int currentIndex = SceneManager.GetActiveScene().buildIndex;
			int nextIndex = currentIndex + 1;
			int lastIndex = SceneManager.sceneCountInBuildSettings - 1;
			Debug.Log(currentIndex + " " + nextIndex + " " + lastIndex);
			
			if (nextIndex <= lastIndex) {
				SceneManager.LoadScene(nextIndex);
			}
			else {
				Debug.Log("Last scene");
			}
		}
	}
}
