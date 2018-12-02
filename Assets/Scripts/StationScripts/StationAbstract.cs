/*
Origonal Author: ECHibiki

Sets mandatory station requirements

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationAbstract : MonoBehaviour {
    public PlayerScript stationOperator;

    protected bool in_use;
	public bool nearby;
	public float time_nearby;
	protected StationController station_controller;

	abstract public bool actionPressed ();

	abstract public void fire1Pressed ();

	abstract public void fire2Pressed ();

	abstract public void fire3Pressed ();

	abstract public bool checkDistance (Vector2 caller_location, StationAbstract station);

	abstract public void directionPressed (Vector3 dir_vec);

	abstract protected void enterStation ();

	abstract protected void exitStation ();

    abstract public bool IsOccupied(Vector2 caller_location, StationAbstract station);

	public bool get_in_use(){
		return in_use;
	}

	public StationController get_station_controller(){
		return station_controller;
	}

	public void set_in_use(bool iu){
		in_use = iu;
	}

	public void set_station_controller(StationController sc){
		station_controller = sc;
	}

}
