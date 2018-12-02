using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour {

    //public static int[] player_settings = new int[4];

    public GameObject cat_red;
	public GameObject cat_green;
	public GameObject cat_blue;
	public GameObject cat_yellow;

	public GameObject[] start_locations = new GameObject[4];
	public GameObject[] player_chevrons = new GameObject[4];

    CharacterSelection characterSelection;

    void Start(){

        if (!PlayerSettings.CharactersSelectedByPlayers())
        {
            exampleInitialization();
        }

        createPlayers();
    }

	public void exampleInitialization(){
        PlayerSettings.player_settings[0] = 'r';
        PlayerSettings.player_settings[1] = 'y';
        PlayerSettings.player_settings[2] = 'b';
        PlayerSettings.player_settings[3] = 'g';
	}

	public void createPlayers(){
		for (int player_no = 0; player_no < 4; player_no++) {
			char player_type = (char)PlayerSettings.player_settings[player_no] ;
			if (PlayerSettings.player_settings[player_no] == '\0') {
				Debug.Log ("No PLayer " + player_no);
			} else {
				GameObject new_cat = null;
				Color chevron_color = Color.white;
				switch (player_type) {
				case 'r':
					new_cat = Instantiate (cat_red, Vector3.zero, Quaternion.identity);
					chevron_color = Color.red;
					break;
				case 'g':
					new_cat = Instantiate (cat_green, Vector3.zero, Quaternion.identity);
					chevron_color = Color.green;
					break;
				case 'b':
					new_cat = Instantiate (cat_blue, Vector3.zero, Quaternion.identity);
					chevron_color = Color.blue;
					break;
				case 'y':
					new_cat = Instantiate (cat_yellow, Vector3.zero, Quaternion.identity);
					chevron_color = Color.yellow;
					break;
				default:
					Debug.LogError ("No Cat Color For Initialization");
					break;
				}
				if (new_cat != null) {
                    GameObject player_chevron = Instantiate (player_chevrons [player_no], Vector3.zero, Quaternion.identity);
					player_chevron.transform.parent = new_cat.transform;
					player_chevron.transform.localPosition = player_chevrons [player_no].transform.position;
					player_chevron.GetComponent<SpriteRenderer> ().color = chevron_color;

					new_cat.transform.parent = GameObject.FindGameObjectWithTag ("Yarn_inside_container").transform;
					((PlayerScript)new_cat.GetComponent<PlayerScript> ()).controller_number = player_no + 1;
					((PlayerScript)new_cat.GetComponent<PlayerScript> ()).color = player_type;
					new_cat.transform.localPosition = start_locations [player_no].transform.localPosition;
				}
			}
		}
	}

}
