using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CharacterSelection : MonoBehaviour {

    bool m_IsCreated = false;

    [SerializeField] GameObject[] m_Icons;
    [SerializeField] PlayerIconPositions m_PlayerIconPositions;
    [SerializeField] GameObject m_ConfirmationPanel;
    [SerializeField] GameObject m_SelectionPanel;
    [SerializeField] GameObject m_ReadyText;
    [SerializeField] GameObject m_CancelButton;

    PlayerEventButton[] m_PlayerButtons;
    bool m_ButtonsActivated = false;
    bool[] m_PlayerIconActivated = new bool[4];

	AudioSource [] playerIns;
	AudioSource p1_In;
	AudioSource p2_In;
	AudioSource p3_In;
	AudioSource p4_In;


    private void Start()
    {
        m_PlayerButtons = m_PlayerIconPositions.playerEventButtons;
        for (int i = 0; i < 4; i++)
        {
            m_Icons[i].SetActive(false);
            m_PlayerButtons[i].interactable = false;
            PlayerSettings.player_settings[i] = 0;
        }
        m_ConfirmationPanel.SetActive(false);
        m_ReadyText.SetActive(false);

		playerIns = gameObject.GetComponents<AudioSource> ();
		p1_In = playerIns [0];
		p2_In = playerIns [1];
		p3_In = playerIns [2];
		p4_In = playerIns [3];

    }

    private void Update()
    {
        CheckInitialize();
        ReadyCheck();
    }

    private void CheckInitialize()
    {
        if (!m_ButtonsActivated && Input.GetButton("Start"))
        {
            m_ButtonsActivated = true;
            for (int i = 0; i < 4; i++)
            {
                m_PlayerButtons[i].interactable = true;
                m_PlayerButtons[i].enabled = false; // necessary to prevent highlighted animation...
                m_PlayerButtons[i].enabled = true;
            }
            m_ReadyText.SetActive(true);
        }
        if (!m_PlayerIconActivated[0] && Input.GetButton("Start_P1"))
        {
            m_Icons[0].SetActive(true);
            m_PlayerIconActivated[0] = true;
			p1_In.Play ();
        }
        if (!m_PlayerIconActivated[1] && Input.GetButton("Start_P2"))
        {
            m_Icons[1].SetActive(true);
            m_PlayerIconActivated[1] = true;
			p2_In.Play ();
        }
        if (!m_PlayerIconActivated[2] && Input.GetButton("Start_P3"))
        {
            m_Icons[2].SetActive(true);
            m_PlayerIconActivated[2] = true;
			p3_In.Play ();
        }
        if (!m_PlayerIconActivated[3] && Input.GetButton("Start_P4"))
        {
            m_Icons[3].SetActive(true);
            m_PlayerIconActivated[3] = true;
			p4_In.Play ();
        }
    }

    private void ReadyCheck()
    {
        if (PlayerSettings.CharactersSelectedByPlayers() && Input.GetButton("Jump"))
        {
            m_SelectionPanel.SetActive(false);
            m_ConfirmationPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(m_CancelButton, null);
        }
    }

    public void BackToSelection()
    {
        m_SelectionPanel.SetActive(true);
        m_ConfirmationPanel.SetActive(false);
    }

}
