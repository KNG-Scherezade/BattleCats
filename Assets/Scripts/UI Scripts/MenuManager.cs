using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MenuManager : MonoBehaviour {


	public AudioMixer masterMixer;

	private AudioSource[] menuSound;
	private AudioSource menuMusic;
	private AudioSource menuSel;
	private Scene m_Scene;
	private GameObject[] menuManagers;

	private float timestamp;
	private float musicFadeTimer;
	private bool fadeMusic;
	private bool isMaster;

	void Start()
	{
		menuSound = GetComponents<AudioSource> ();
		menuMusic = menuSound [0];
		menuSel = menuSound [1];
		m_Scene = SceneManager.GetActiveScene ();
		DontDestroyOnLoad (menuSel);
		fadeMusic = false;
		isMaster = false;
		menuManagers = GameObject.FindGameObjectsWithTag ("MenuManager");
		//creates a singleton style MenuManager
		if (m_Scene.name == "MainMenu" && menuManagers.Length == 1) {
			DontDestroyOnLoad (this.gameObject);
			DontDestroyOnLoad (menuMusic);
			menuMusic.Play ();
			isMaster = true;

		} else {
			Destroy (gameObject);
		}
	}

	void Update()
	{
		if (fadeMusic) 
		{
			menuMusic.volume = menuMusic.volume - 0.025f;
			if (Time.time - timestamp > musicFadeTimer) {
				fadeMusic = false;
				menuMusic.Stop ();
			}
		}
		
	}

    public void GoToMainMenu()
    {
		menuSel.Play ();
		SceneManager.LoadScene ("MainMenu");
    }

    public void GoToCharacterSelection()
    {
		menuSel.Play ();
		SceneManager.LoadScene ("CharacterSelection");
    }

    public void GoToLevelSelection()
    {
        menuSel.Play();
        SceneManager.LoadScene("LevelSelection");
    }

    public void GoToLevel0()
    {
		menuSel.Play ();
        SceneManager.LoadScene("Level0");
    }

    public void GoToLevel1()
    {
		menuSel.Play ();
        SceneManager.LoadScene("Level1");
    }

    public void GoToLevel2()
    {
        menuSel.Play();
        SceneManager.LoadScene("Level2");
    }

    public void GoToSettings()
    {
		menuSel.Play ();
        SceneManager.LoadScene("Settings");
    }

    public void GoToInstructions()
    {
		menuSel.Play ();
        SceneManager.LoadScene("Instructions");
    }

    public void GoToCredits()
    {
		menuSel.Play ();
        SceneManager.LoadScene("Credits");
    }
    

    public void ExitGame()
    {
        Application.Quit();
    }

    public void UpdateMasterValue(Slider slider)
    {
		masterMixer.SetFloat("MasterLevel", slider.value);
        //Set master value
    }

    public void UpdateAmbientValue(Slider slider)
    {
		masterMixer.SetFloat("AmbientLevel", slider.value);
        //Set Ambient value
    }

    public void UpdateSFXValue(Slider slider)
    {
		masterMixer.SetFloat("SFXLevel", slider.value);
        //Set SFX Volume
    }

	public void StartMenuMusic()
	{
		menuMusic.volume = 1f;
		menuMusic.Play ();		
	}

	public void PlayMenuSel(){
		menuSel.Play ();
	}

	public bool IsMaster()
	{
		return isMaster;
	}

	public void StopMenuMusic()
	{
		fadeMusic = true;
		timestamp = Time.time;
		musicFadeTimer = 1.5f;
	}
		
	public bool IsMenuMusicPlaying()
	{
		return menuMusic.isPlaying;
	}
}
