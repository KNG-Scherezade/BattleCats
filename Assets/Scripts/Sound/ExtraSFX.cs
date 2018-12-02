using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraSFX : MonoBehaviour {

	[SerializeField]
	private GameObject coin;
	AudioSource [] audioSources;
	AudioSource splash;
	AudioSource endLevel;

	// Use this for initialization
	void Start () {
		audioSources = GetComponents<AudioSource> ();
		splash = audioSources [0];
		endLevel = audioSources [1];
	}
		
	public void PlayCoinSound()
	{
		Instantiate (coin, transform.position, transform.rotation);
	} 

	public void PlaySplashSound()
	{
		splash.Play ();
	}

	public void PlayEndLevelSound()
	{
		endLevel.Play ();
	}
}
