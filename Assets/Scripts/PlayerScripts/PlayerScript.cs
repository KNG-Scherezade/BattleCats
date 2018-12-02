//ECHibiki. For testing station interactions 23, 26 and movement 30
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

//	public float drive_x_warp;
//	public float drive_y_warp;
//	public float idle_x_warp;
//	public float idle_y_warp;

	public int controller_number;
	public char color;

	MovementToStationConnector movement_station_abstraction;

	public PlayerStationBehaviour player_station_behaviour;
	CatMovement cat_movement;


	// Use this for initialization
	void Start () {
		player_station_behaviour = new PlayerStationBehaviour();
		player_station_behaviour.init (this);
		player_station_behaviour.controller_number = controller_number;
		cat_movement = gameObject.GetComponent (typeof(CatMovement)) as CatMovement;
		cat_movement.controller_number = controller_number;

		movement_station_abstraction = new MovementToStationConnector ();
		movement_station_abstraction.init (player_station_behaviour, cat_movement);
	}
	
	// Update is called once per frame
	void Update () {
		movement_station_abstraction.update ();
	}
}
