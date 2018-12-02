/*
Origonal Author: ECHibiki

Use to run functions on combined set of stations

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour {

	public float end_ui_alpha, start_ui_alpha;

	private List<StationAbstract> station_list = new List<StationAbstract>();
	private GameObject player_station_UI;
	private Transform[] ui_components;

	void Start(){
		player_station_UI = GameObject.FindGameObjectWithTag ("PlayerUI");
		ui_components = player_station_UI.GetComponentsInChildren<Transform> (true);
	}

	void Update(){
		//nearby property set in playerStationBehaviour.cs
		foreach (StationAbstract station in station_list) { 
			if (station.nearby) {
				string station_type = station.GetType ().ToString ();
				switch (station_type) {
				case "StationNav":  // [House] - Yarn control - Hiding animation
					foreach (Transform component in ui_components) {
						if (component != null) {
							if (component.gameObject.name == "drive") {
								component.gameObject.active = true;
								component.gameObject.GetComponent<SpriteRenderer>().color 
								= new Color(1.0f,1.0f,1.0f,Mathf.SmoothStep(start_ui_alpha, end_ui_alpha, Time.time - station.time_nearby));   
								break;
							}
						}
					}
					break;
				case "StationShield": // [Box] - Shield control - Driving animation
					foreach (Transform component in ui_components) {
						if (component != null) {
							if (component.gameObject.name == "shield") {
								component.gameObject.active = true;
								component.gameObject.GetComponent<SpriteRenderer>().color 
								= new Color(1.0f,1.0f,1.0f,Mathf.SmoothStep(start_ui_alpha, end_ui_alpha, Time.time - station.time_nearby));   
								break;
							}
						}
					}
					break;
				case "TurretStation": // [Whackamole/scratch thing/fish] - Turret control - Pushing animation
					if (GameManager.ammoTypeUnlocked [0]) {
						foreach (GameObject xl1 in GameObject.FindGameObjectsWithTag("XL1")) {
							xl1.SetActive (false);
						}
					}
					if (GameManager.ammoTypeUnlocked [1]) {
						foreach (GameObject xl2 in GameObject.FindGameObjectsWithTag("XL2")) {
							xl2.SetActive (false);
						}
					}

					switch (station.name) {
					case "TurretStation_Right":
						foreach (Transform component in ui_components) {
							if (component != null) {
								if (component.gameObject.name == "shoot R") {
									component.gameObject.active = true;
									component.gameObject.GetComponent<SpriteRenderer>().color 
									= new Color(1.0f,1.0f,1.0f,Mathf.SmoothStep(start_ui_alpha, end_ui_alpha, Time.time - station.time_nearby));   
									break;
								}
							}
						}
						break;
					case  "TurretStation_Left":
						foreach (Transform component in ui_components) {
							if (component != null) {
								if (component.gameObject.name == "shoot L") {
									component.gameObject.active = true;
									component.gameObject.GetComponent<SpriteRenderer>().color 
									= new Color(1.0f,1.0f,1.0f,Mathf.SmoothStep(start_ui_alpha, end_ui_alpha, Time.time - station.time_nearby));   
									break;
								}
							}
						}
						break;
					case "TurretStation_Top":
						foreach (Transform component in ui_components) {
							if (component != null) {
								if (component.gameObject.name == "shoot T") {
									component.gameObject.active = true;
									component.gameObject.GetComponent<SpriteRenderer>().color 
									= new Color(1.0f,1.0f,1.0f,Mathf.SmoothStep(start_ui_alpha, end_ui_alpha, Time.time - station.time_nearby));   
									break;
								}
							}
						}
						break;
					}
					break;
				default:
					break;
				}
			} else {
				string station_type = station.GetType ().ToString ();
				switch (station_type) {
				case "StationNav":  // [House] - Yarn control - Hiding animation
					foreach (Transform component in ui_components) {
						if (component != null) {
							if (component.gameObject.name == "drive") {
								component.gameObject.active = false;
								break;
							}
						}
					}
					break;
				case "StationShield": // [Box] - Shield control - Driving animation
					foreach (Transform component in ui_components) {
						if (component != null) {
							if (component.gameObject.name == "shield") {
								component.gameObject.active = false;
								break;
							}
						}
					}
					break;
				case "TurretStation": // [Whackamole/scratch thing/fish] - Turret control - Pushing animation
					switch (station.name) {
					case "TurretStation_Right":
						foreach (Transform component in ui_components) {
							if (component != null) {
								if (component.gameObject.name == "shoot R") {
									component.gameObject.active = false;
									break;
								}
							}
						}
						break;
					case  "TurretStation_Left":
						foreach (Transform component in ui_components) {
							if (component != null) {
								if (component.gameObject.name == "shoot L") {
									component.gameObject.active = false;
									break;
								}
							}
						}
						break;
					case "TurretStation_Top":
						foreach (Transform component in ui_components) {
							if (component != null) {
								if (component.gameObject.name == "shoot T") {
									component.gameObject.active = false;
									break;
								}
							}
						}
						break;
					}
					break;
				default:
					break;
				}
			}
		}

		foreach (StationAbstract station in station_list) {
			station.nearby = false;
		}

	}

	public void addStation(StationAbstract station){
		station_list.Add (station);
	}

	public int checkStationWithinDistance(Vector2 caller_location){
		int station_index = 0;
		foreach (StationAbstract station in station_list) {
			if (station.checkDistance (caller_location, station)) {
				return station_index;
			}
			station_index++;
		}
		station_index = -1;
		return station_index;
	}

    public StationAbstract getInUseStation(Vector2 caller_location)
    {
        foreach (StationAbstract station in station_list)
        {
            if(station.IsOccupied(caller_location, station))
            {
                return station;
            }
        }
        return null;
    }

    //	public int[] checkPlayersWithinStation(Vector2 caller_location){
    //		List<int> inside_indices;
    //		int station_index = 0;
    //		foreach (StationAbstract station in station_list) {
    //			if (station.checkDistance (caller_location, station)) {
    //				inside_indices.Add(station_index);
    //			}
    //			station_index++;
    //		}
    //		return inside_indices.ToArray();
    //	}

    public StationAbstract getStationByIndex(int station_index){
		return station_list [station_index];
	}
	public List<StationAbstract> getStationList(){
		return station_list;
	}


}
