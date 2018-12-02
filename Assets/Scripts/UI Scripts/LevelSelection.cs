using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelSelection : MonoBehaviour {

    public int selectedLevel;
    [SerializeField] GameObject m_ConfirmationPanel;
    [SerializeField] GameObject m_CancelButton;
    [SerializeField] GameObject SelectionPanel;
    [SerializeField] GameObject Level0Button;
	private GameObject[] menuManagers;
	private MenuManager menuManagerScript;


    private void Start()
    {
        m_ConfirmationPanel.SetActive(false);
		menuManagers = GameObject.FindGameObjectsWithTag ("MenuManager");
    }

    public void SelectLevel0()
    {
        selectedLevel = 0;
        SelectionPanel.SetActive(false);
        m_ConfirmationPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_CancelButton,null);
    }

    public void SelectLevel1()
    {
        selectedLevel = 1;
        SelectionPanel.SetActive(false);
        m_ConfirmationPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_CancelButton,null);

    }

    public void SelectLevel2()
    {
        selectedLevel = 2;
        SelectionPanel.SetActive(false);
        m_ConfirmationPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_CancelButton, null);

    }



    public void SelectLevelBoss()
    {
        selectedLevel = 3;
        SelectionPanel.SetActive(false);
        m_ConfirmationPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_CancelButton,null);

    }

    public void GoToSelectedLevel()
    {
        switch (selectedLevel)
        {
			case 0:
				DestroyMenuMusic ();
                SceneManager.LoadScene("Level0");
                break;
            case 1:
				DestroyMenuMusic ();
                SceneManager.LoadScene("Level1");
                break;
            case 2:
				DestroyMenuMusic ();
                SceneManager.LoadScene("Level2a");
                break;
            case 3:
				DestroyMenuMusic ();
                SceneManager.LoadScene("Level3");
                break;
        }
    }

    public void GoBackToSelection()
    {
        m_ConfirmationPanel.SetActive(false);
        SelectionPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Level0Button,null);
    }

    public void GobackToCharacterSelection()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

	public void DestroyMenuMusic()
	{
		foreach (GameObject manager in menuManagers) 
		{
			menuManagerScript = manager.GetComponent<MenuManager> ();
			if (menuManagerScript.IsMenuMusicPlaying ()) 
			{
				menuManagerScript.StopMenuMusic ();
			}

		}
		
	}
}
