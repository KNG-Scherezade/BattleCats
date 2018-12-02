using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementToStationConnector {

	private CatMovement cat_movement;
	private PlayerStationBehaviour player_station_behaviour;

	public void init(PlayerStationBehaviour psb, CatMovement cm){
		cat_movement = cm;
		player_station_behaviour = psb;
	}

	public void update(){
		player_station_behaviour.updateRoutine ();

		cat_movement.setStationAnimation (
			player_station_behaviour.getUsingStation(), 
			player_station_behaviour.getTypeOfStation(),
			player_station_behaviour.getStationName(),
			player_station_behaviour.getNewPlayerLocation()
		);
	}
}
