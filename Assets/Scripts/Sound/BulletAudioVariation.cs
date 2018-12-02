using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAudioVariation : MonoBehaviour {

	AudioSource bulletSound;

	void Awake(){
		bulletSound = GetComponent<AudioSource> ();
		bulletSound.volume = Random.Range (0.3f, 1.0f);
		bulletSound.Play ();
	}

}
