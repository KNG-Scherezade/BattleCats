using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletMover : MonoBehaviour {

	public float speed;

	void Start () {
		Rigidbody2D bullet;
		bullet = GetComponent<Rigidbody2D>();
		bullet.velocity = transform.right * speed;
	}

}
