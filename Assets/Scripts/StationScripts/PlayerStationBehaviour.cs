using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStationBehaviour {

	private PlayerScript player;

	private StationController station_controller;
	private StationAbstract in_use_station;
	public bool using_station;

    private PlayerSetup playerSetup;
	public int controller_number;

	private Vector3 playerJumpLocation;

	// Use this for initialization
	public void init (PlayerScript plyr) {
		station_controller = (StationController) GameObject.FindGameObjectWithTag("StationController").GetComponent(typeof(StationController));
        playerSetup = (PlayerSetup)GameObject.FindGameObjectWithTag("GameInitializer").GetComponent(typeof(PlayerSetup));
        player = plyr;

	}
	
	public void updateRoutine(){
		playerJumpLocation = Vector3.zero;
		if (controller_number > 0) {
			if (Input.GetButtonDown ("Drive_P" + controller_number)) {
				if (using_station) {// Leave station
					using_station = in_use_station.actionPressed ();
					playerJumpLocation = in_use_station.gameObject.transform.Find ("WarpPoint").transform.position;
//					player.transform.position = playerJumpLocation;
					player.GetComponent<Rigidbody2D> ().simulated = true;
                    in_use_station.stationOperator = null;
                    in_use_station = null;
				} else {// Enter station
					int station_index = station_controller.checkStationWithinDistance (new Vector2 (player.transform.position.x, player.transform.position.y));
                    if (station_index != -1) {// Station is available
                        in_use_station = station_controller.getStationByIndex (station_index);
						using_station = in_use_station.actionPressed ();
                        in_use_station.stationOperator = player;
                        BoxCollider2D collider = in_use_station.GetComponent<BoxCollider2D> ();
						if ((player.color == 'y' || player.color == 'g') && getTypeOfStation() == "StationShield") {
//							Debug.Log (getTypeOfStation ());
							playerJumpLocation = in_use_station.gameObject.transform.Find ("WarpPointFat").transform.position;
//							player.transform.position = playerJumpLocation;
						} else {
							playerJumpLocation = in_use_station.gameObject.transform.Find ("WarpPoint").transform.position;
//							player.transform.position = playerJumpLocation;
						}
						player.GetComponent<Rigidbody2D> ().simulated = false;
                    } else { // Station is being used by another player, kick whoever out
                        /*
                         * First, find the station in use
                         */
                        in_use_station = station_controller.getInUseStation(new Vector2(player.transform.position.x, player.transform.position.y));

                        if (in_use_station != null)
                        {
                            /*
                             * Second, kick the pilot out
                             */
                            // Find the pilot
                            var stationOperator = in_use_station.stationOperator;
                            // Set StationGeneric.in_use to false, this value indicates whether the station is in use.
                            stationOperator.player_station_behaviour.in_use_station.actionPressed();
                            // Set using_station to false, because animation depends on this value.
                            stationOperator.player_station_behaviour.using_station = false;
                            // Move the pilot out (it did not actually move out, I just follow the code written before, see line 32)
                            stationOperator.player_station_behaviour.playerJumpLocation = in_use_station.gameObject.transform.Find("WarpPoint").transform.position;
                            stationOperator.GetComponent<Rigidbody2D>().simulated = true;
                            // Reset the pilot of the current in-use station
                            stationOperator.player_station_behaviour.in_use_station.stationOperator = null;

                            /*
                             * Third, enter station
                             */
                            // Set the station in use
                            using_station = in_use_station.actionPressed();
                            // Update the pilot
                            in_use_station.stationOperator = player;
                            if ((player.color == 'y' || player.color == 'g') && getTypeOfStation() == "StationShield")
                            {
                                //							Debug.Log (getTypeOfStation ());
                                playerJumpLocation = in_use_station.gameObject.transform.Find("WarpPointFat").transform.position;
                                //							player.transform.position = playerJumpLocation;
                            }
                            else
                            {
                                playerJumpLocation = in_use_station.gameObject.transform.Find("WarpPoint").transform.position;
                                //							player.transform.position = playerJumpLocation;
                            }
                            player.GetComponent<Rigidbody2D>().simulated = false;
                        }

                    }
				}
			}
			if (using_station) {
				float hforce = Input.GetAxis ("Horizontal_P" + controller_number);
				float vforce = Input.GetAxis ("Vertical_P" + controller_number);
				if (player.GetComponent<CatMovement> ().facingDirection.x > 0)
					vforce *= -1;

//				Debug.Log (player.GetComponent<CatMovement> ().facingDirection);
				in_use_station.directionPressed (new Vector3 (hforce, vforce, 0));
				if (Input.GetButton ("Fire1_P" + controller_number)) {
					in_use_station.fire1Pressed ();
				} else if (Input.GetButton ("Fire2_P" + controller_number)) {
					in_use_station.fire2Pressed ();
				} else if (Input.GetButton ("Fire3_P" + controller_number)) {
					in_use_station.fire3Pressed ();
				}
			} else {
				int station_index = station_controller.checkStationWithinDistance (new Vector2 (player.transform.position.x, player.transform.position.y));
				if (station_index != -1) {
					StationAbstract checked_station = station_controller.getStationByIndex (station_index);
					if (!checked_station.get_in_use ()) {
						checked_station.nearby = true;
						float hforce = Input.GetAxis ("Horizontal_P" + controller_number);
						float vforce = Input.GetAxis ("Vertical_P" + controller_number);
						if (hforce != 0) {
							checked_station.time_nearby = Time.time;
						}
					}
				}
			}
		}
	}

	public bool getUsingStation(){
		return using_station;
	}

	public string getTypeOfStation(){
		if(in_use_station != null) return in_use_station.GetType ().ToString();
		else return "";
	}

	public string getStationName(){
		if(in_use_station != null)	return in_use_station.gameObject.name;
		else return "";
	}

	public Vector3 getNewPlayerLocation(){
		return playerJumpLocation;
	}

}
