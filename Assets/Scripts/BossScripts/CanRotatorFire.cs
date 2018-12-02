using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanRotatorFire : MonoBehaviour {

	public GameObject bullet;
	public GameObject grenade;

	public Transform shotSpawn;

	public float rotationSpeed;
	private float nextFire;
	public float nextFireDelay;
	private float nextGrenade;
	public float nextGrenadeDelay;
	public float grenadeLaunchPower;

	public bool noAimSequence;
	public bool grenadeSequence;
	private bool rotateUp;
	public bool isRightCan;


	// Use this for initialization
	void Start () {
		noAimSequence = false;
		grenadeSequence = false;
		rotateUp = true;
		isRightCan = false;
		rotationSpeed = 40f;
		nextFire = 0f;
		nextFireDelay = 1.5f;
		nextGrenade = 0f;
		nextGrenadeDelay = 5f;
		grenadeLaunchPower = 4f;
	}
	
	// Update is called once per frame
	void Update () {

		if (noAimSequence == true) {
			NoAimSequence ();
		}

		if (grenadeSequence == true) {
			GrenadeSequence ();
		}
		
	}

	void Fire() {
		
		Instantiate (bullet, shotSpawn.position, shotSpawn.rotation);
	}

	void DropGrenade(){
		GameObject clone;
		clone = Instantiate (grenade, shotSpawn.position, shotSpawn.rotation); 
		if (isRightCan) {
			clone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection (new Vector2(1,1) * grenadeLaunchPower);
		} else {
			clone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection (new Vector2(-1,1) * grenadeLaunchPower);
		}
		clone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection (new Vector2(-1,1) * grenadeLaunchPower);
	}

	void NoAimSequence(){
		if (Time.time > nextFire) {
			Fire ();
			nextFire = Time.time + nextFireDelay;
		}
		if (transform.rotation.z <= -0.3230917f) {
			rotateUp = false;
		}
		if (transform.rotation.z >= 0.2873605f) {
			rotateUp = true;
		} 
		if (rotateUp) {
			transform.Rotate (0, 0, -Time.deltaTime * rotationSpeed);
		} else {
			transform.Rotate (0, 0, Time.deltaTime * rotationSpeed);
		}
	}

	void GrenadeSequence(){
		if (Time.time > nextGrenade) {
			DropGrenade ();
			nextGrenade = Time.time + nextGrenadeDelay;
		}
	}
		
}
