using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CatSelection : MonoBehaviour
{
    [SerializeField] Color m_WhiteColor;
    [SerializeField] Color m_SelectedColor;
    Color m_OriginalColor;
    RawImage m_Image;
    PlayerEventButton m_Button;

    bool m_IsClicked = false;

	private AudioSource pX_sel;

    [SerializeField] CharacterSelection characterSelection;

    private void Start()
    {
        m_Image = GetComponentInParent<RawImage>();
        m_Button = GetComponent<PlayerEventButton>();
		pX_sel = GetComponent<AudioSource> ();
    }

    public void OnCatSelected()
    {
        PlayerEventSystem eventSystem = EventSystem.current as PlayerEventSystem;

        if (!m_IsClicked)
        {
            m_Image.color = m_SelectedColor;
            m_IsClicked = true;
            m_Button.enabled = false;
            m_Button.enabled = true;
			pX_sel.Play ();

            // Lock player on cat
            GetComponent<Animator>().SetTrigger("Normal");
            eventSystem.hasSelected = true;
            eventSystem.canMove = false;
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.None;
            m_Button.navigation = navigation;

            AssignCatToPlayer(eventSystem);
        }
        else
        {
            m_Image.color = m_WhiteColor;
            m_IsClicked = false;

            // Unlock player
            eventSystem.hasSelected = false;
            eventSystem.canMove = true;
            m_Button.navigation = Navigation.defaultNavigation;

            FreeCatForSelection(eventSystem);
        }
    }

    private void AssignCatToPlayer(PlayerEventSystem eventSystem)
    {
        int playerID = eventSystem.controller_number - 1;
        switch (m_Button.name)
        {
            case "BlueCat":
                PlayerSettings.player_settings[playerID] = 'b';
                PlayerSettings.buttonSelected[0] = true;
                Debug.Log("Blue cat selected");
                break;
            case "YellowCat":
                PlayerSettings.player_settings[playerID] = 'y';
                PlayerSettings.buttonSelected[1] = true;
                Debug.Log("Yellow cat selected");
                break;
            case "RedCat":
                PlayerSettings.player_settings[playerID] = 'r';
                PlayerSettings.buttonSelected[2] = true;
                Debug.Log("Red cat selected");
                break;
            case "GreenCat":
                PlayerSettings.player_settings[playerID] = 'g';
                PlayerSettings.buttonSelected[3] = true;
                Debug.Log("Green cat selected");
                break;
            default:
                break;
        }
    }

    private void FreeCatForSelection(PlayerEventSystem eventSystem)
    {
        int playerID = eventSystem.controller_number - 1;
        PlayerSettings.player_settings[playerID] = 0;

        switch (m_Button.name)
        {
            case "BlueCat":
                PlayerSettings.buttonSelected[0] = false;
                Debug.Log("Blue cat unselected");
                break;
            case "YellowCat":
                PlayerSettings.buttonSelected[1] = false;
                Debug.Log("Yellow cat unselected");
                break;
            case "RedCat":
                PlayerSettings.buttonSelected[2] = false;
                Debug.Log("Red cat unselected");
                break;
            case "GreenCat":
                PlayerSettings.buttonSelected[3] = false;
                Debug.Log("Green cat unselected");
                break;
            default:
                break;
        }
    }
}

