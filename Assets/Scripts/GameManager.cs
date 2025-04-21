using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static int collectedCoins = 0;
	
	public static TMP_Text coinText;
	private static Vector3 spawnPoint;
	public static bool CheckPointsReached {get; private set;}
	
	private const string COLLECTED_COIN_TEXT = "Collected Coins: ";
	
	static void UpdateCoinText() {
		coinText.text = COLLECTED_COIN_TEXT + collectedCoins;
	}
	void Start()
	{
		UpdateCoinText();
	}

	public static void SetSpawnPoint(Vector3 pos, bool checkpoint) {
		spawnPoint = pos;
		if (!CheckPointsReached) {
			CheckPointsReached = checkpoint;
		}
	}
	
	public static Vector3 GetSpawnPoint() {
		return spawnPoint;
	}
	
	public static int GetCollectedCoins() {
		return collectedCoins;
	}
	
	public static void DeCollectCoin() {
		--collectedCoins;
		UpdateCoinText();
	}
	
	public static void ResetCoins() {
		collectedCoins = 0;
		UpdateCoinText();
	}
	
	public static void CollectCoin() {
		++collectedCoins;
		UpdateCoinText();
	}
	public static void ResetSpawnPoint() {
		CheckPointsReached = false;
	}
	public static void ResetLevel() {
		ResetCoins();
		ResetSpawnPoint();
	}
	
	public static void LoadScene(string sceneName) {
		ResetLevel();
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
	public static bool IsGround(GameObject gameObject) {
		return gameObject.CompareTag("Ground") || gameObject.CompareTag("BreakableBlock");
	}
}
