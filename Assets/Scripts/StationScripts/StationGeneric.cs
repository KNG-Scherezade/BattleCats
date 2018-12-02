/*
Origonal Author: ECHibiki

A box that players can enter. Implements basic behaviour.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationGeneric : StationAbstract{

	//Alphabetic ordering. Capitals up top, lower on bottom

	void Start(){
		//startRoutine (this);
	}
		
	public override bool actionPressed(){
		if (in_use == false){
			enterStation ();
			return in_use;
		}
		else {
			exitStation ();
			return in_use;
		}   
	}

	public override void fire1Pressed()
	{      
	}

	public override void fire2Pressed()
	{      
	}

	public override void fire3Pressed()
	{      
	}

	public override bool checkDistance (Vector2 caller_location, StationAbstract station){

		string log_string = "Generic Station";
		if (in_use) {
			log_string += " - In Use";
			return false;
		}
		else {		
			log_string += " - Testing Distance: " + ((BoxCollider2D)station.GetComponent(typeof(BoxCollider2D))).OverlapPoint(caller_location);
			return ((BoxCollider2D)station.GetComponent(typeof(BoxCollider2D))).OverlapPoint(caller_location);
		}
	}

    public override bool IsOccupied(Vector2 caller_location, StationAbstract station)
    {
        return ((BoxCollider2D)station.GetComponent(typeof(BoxCollider2D))).OverlapPoint(caller_location) && in_use;
    }

    public override void directionPressed(Vector3 dir_vec){	Debug.Log ("generic move heard");}

	protected override void enterStation(){
		string log_string = "Generic Station - Enter";

		in_use = true;
	}

	protected override void exitStation(){
		string log_string = "Generic Station - Exit";

		in_use = false;
	}

	public void startRoutine(StationAbstract station){
		in_use = false;
		station_controller = (StationController) GameObject.FindGameObjectWithTag("StationController").GetComponent(typeof(StationController));
		station_controller.addStation (station);
	}
}