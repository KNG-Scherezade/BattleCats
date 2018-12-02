//ECHibiki. For testing station interactions 23, 26 and movement 30
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer23 : MonoBehaviour {

	public float drive_x_warp;
	public float drive_y_warp;
	public float idle_x_warp;
	public float idle_y_warp;

	MovementToStationConnector movement_station_abstraction;

	PlayerStationBehaviour player_station_behaviour;
	CatMovement cat_movement;


	// Use this for initialization
	void Start () {
		player_station_behaviour = new PlayerStationBehaviour();
//		player_station_behaviour.init (this);
		cat_movement = gameObject.GetComponent (typeof(CatMovement)) as CatMovement;

		movement_station_abstraction = new MovementToStationConnector ();
		movement_station_abstraction.init (player_station_behaviour, cat_movement);
	}
	
	// Update is called once per frame
	void Update () {

		movement_station_abstraction.update ();

	}
}
