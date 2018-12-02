/*
Origonal Author: ECHibiki

Maintains an objects rotation with the yarn

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilizerInside : MonoBehaviour {

	public GameObject yarn_outside;

	private Vector2 start_pos;
	void Start(){
		start_pos = new Vector2 (this.transform.localPosition.x, this.transform.localPosition.y);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localPosition =new Vector2( 
			yarn_outside.transform.localPosition.x + start_pos.x,
			yarn_outside.transform.localPosition.y + start_pos.y);
	}
}
