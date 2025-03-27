using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static int collectedCoins = 0;
	
	public static TMP_Text coinText;
	
	private const string COLLECTED_COIN_TEXT = "Collected Coins: ";

	
	static void UpdateCoinText() {
		coinText.text = COLLECTED_COIN_TEXT + collectedCoins;
	}
	static void Start()
	{
		Debug.Log("Starting GameManager");
		UpdateCoinText();
	}

	public static int GetCollectedCoins() {
		return collectedCoins;
	}
	
	public static void CollectCoin() {
		++collectedCoins;
		UpdateCoinText();
	}
}
