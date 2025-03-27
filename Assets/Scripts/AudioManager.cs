using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager INSTANCE;
	
	private AudioSource audioSource;
	public AudioClip[] audioClipList;

	void Awake()
	{
		if (INSTANCE == null) {
			INSTANCE = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	void Start()
	{
		audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
	}
	
	public static void PlayAudio(string audioName) {
		INSTANCE.PlayAudioNonStatic(audioName);
	}
	
	private void PlayAudioNonStatic(string audioName) {
		AudioClip clip = Array.Find(audioClipList, x => x.name == audioName);
		if (clip == null) {
			throw new KeyNotFoundException("Audioclip \"" + audioName + "\" not found in audioClipList");
		}
				
		audioSource.PlayOneShot(clip);
		
	}
}
