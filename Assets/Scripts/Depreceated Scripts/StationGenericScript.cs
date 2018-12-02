/*
Origonal Author: ECHibiki

Dead

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationGenericScript {

	//Alphabetic ordering. Capitals up top, lower on bottom
	protected bool in_use;
	protected StationController station_controller;

	public bool actionPressed(){
		if (in_use == false){
			enterStation ();
			return in_use;
		}
		else {
			exitStation ();
			return in_use;
		}   
	}

	public bool checkDistance (Vector2 caller_location, StationAbstract station){
		
		string log_string = "Generic Station";
		if (in_use) {
			log_string += " - In Use";
			//Debug.Log(log_string);
			return false;
		}
		else {		
			log_string += " - Testing Distance: " + ((BoxCollider2D)station.GetComponent(typeof(BoxCollider2D))).OverlapPoint(caller_location);
			//Debug.Log(log_string);
			return ((BoxCollider2D)station.GetComponent(typeof(BoxCollider2D))).OverlapPoint(caller_location);
		}
	}
		
	public void directionPressed(Vector3 dir_vec){	Debug.Log ("generic move heard");}

	protected void enterStation(){
		string log_string = "Generic Station - Enter";
		//Debug.Log(log_string);

		in_use = true;
	}

	protected void exitStation(){
		string log_string = "Generic Station - Exit";
		//Debug.Log(log_string);

		in_use = false;
	}

	public void startRoutine(StationAbstract station){
		in_use = false;
		station_controller = (StationController) GameObject.FindGameObjectWithTag("StationController").GetComponent(typeof(StationController));
		station_controller.addStation (station);
	}
}
