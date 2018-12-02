using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindMenuManager : MonoBehaviour {

	private GameObject menuManager;
	private MenuManager menuManagerScript;


	void Awake(){
		
	}
	// Use this for initialization
	void Start () {
		menuManager = GameObject.FindGameObjectWithTag ("MenuManager");
		menuManagerScript = menuManager.GetComponent<MenuManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoToMainMenu()
	{
		menuManagerScript.GoToMainMenu ();
	}

	public void GoToLevelSelection()
	{
		menuManagerScript.GoToLevelSelection ();
	}

	public void GoToCharacterSelection ()
	{
		menuManagerScript.GoToCharacterSelection ();
	}

	public void GoToSettings()
	{
		menuManagerScript.GoToSettings ();
	}

	public void GoToCredits()
	{
		menuManagerScript.GoToCredits ();
	}

	public void GoToInstructions ()
	{
		menuManagerScript.GoToInstructions ();	
	}

	public void GoToExit()
	{
		menuManagerScript.StopMenuMusic ();
		menuManagerScript.ExitGame ();
	}


	public void SetMasterVolume(Slider slider){
		menuManagerScript.UpdateMasterValue (slider);
	}

	public void SetAmbientVolume(Slider slider)
	{
		menuManagerScript.UpdateAmbientValue (slider);
	}

	public void SetSFXVolume(Slider slider)
	{
		menuManagerScript.UpdateSFXValue (slider);
	}
}
