using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnRollScript : MonoBehaviour {

	private GameObject yarn_object;
	private Rigidbody2D yarn_rigidbody;
	private bool moving;
	private bool stopped;
	public float moving_threshold;
	private AudioSource[] sounds;
	private AudioSource yarn_start;
	private AudioSource yarn_loop;


	// Use this for initialization
	void Start () {
		yarn_object = GameObject.FindGameObjectWithTag("YarnPhysics") as GameObject;
		yarn_rigidbody = yarn_object.GetComponent<Rigidbody2D> ();
		stopped = true;
		moving = false;

		sounds = GetComponents<AudioSource> ();
		yarn_start = sounds [0];
		yarn_loop = sounds [1];
	}
	
	// Update is called once per frame
	void Update () {
		if (moving) {
			if (!yarn_start.isPlaying && !yarn_loop.isPlaying) {
				yarn_loop.Play ();
			}
			yarn_loop.pitch = Mathf.Abs(yarn_rigidbody.angularVelocity) / 150;
			if (Mathf.Abs(yarn_rigidbody.angularVelocity) <= moving_threshold) {
				stopped = true;
				moving = false;
				StopYarnSound ();
				//yarn_rolling.Stop ();
			}
		}

		if (stopped) {
			if (Mathf.Abs(yarn_rigidbody.angularVelocity) > moving_threshold) {
				stopped = false;
				moving = true;
				StartYarnSound ();
				//yarn_rolling.Play ();
			}
		}

	}

	void StartYarnSound(){
		yarn_start.Play ();
	}

	void StopYarnSound(){
		yarn_start.Stop ();
		yarn_loop.Stop ();
	}
}
