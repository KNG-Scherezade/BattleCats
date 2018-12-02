using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerEventSystem : EventSystem
{
    public int controller_number;
    public GameObject playerIcon;
    [SerializeField] PlayerIconPositions playerIconPositions;
    public bool canMove = true;
    public bool hasSelected = false;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        EventSystem originalCurrent = current;
        current = this;
        CheckButtonSelected();
        UpdatePlayerIcon();
        base.Update();
        current = originalCurrent;
    }

    private void CheckButtonSelected()
    {
        if (playerIcon.activeInHierarchy && !hasSelected)
        {
            if (currentSelectedGameObject == playerIconPositions.buttons[0] && PlayerSettings.buttonSelected[0])
            {
                playerIcon.transform.position = playerIconPositions.iconTransform[1].position;
                SetSelectedGameObject(playerIconPositions.buttons[1], null);
            }
            else if (currentSelectedGameObject == playerIconPositions.buttons[1] && PlayerSettings.buttonSelected[1])
            {
                playerIcon.transform.position = playerIconPositions.iconTransform[2].position;
                SetSelectedGameObject(playerIconPositions.buttons[2], null);
            }
            else if (currentSelectedGameObject == playerIconPositions.buttons[2] && PlayerSettings.buttonSelected[2])
            {
                playerIcon.transform.position = playerIconPositions.iconTransform[3].position;
                SetSelectedGameObject(playerIconPositions.buttons[3], null);
            }
            else if (currentSelectedGameObject == playerIconPositions.buttons[3] && PlayerSettings.buttonSelected[3])
            {
                playerIcon.transform.position = playerIconPositions.iconTransform[0].position;
                SetSelectedGameObject(playerIconPositions.buttons[0], null);
            }
        }
    }

    private void UpdatePlayerIcon()
    {
        if (playerIcon.activeInHierarchy && canMove)
        {
            if (currentSelectedGameObject == playerIconPositions.buttons[0])
            {
                playerIcon.transform.position = playerIconPositions.iconTransform[0].position;
            }
            else if (currentSelectedGameObject == playerIconPositions.buttons[1])
            {
                playerIcon.transform.position = playerIconPositions.iconTransform[1].position;
            }
            else if (currentSelectedGameObject == playerIconPositions.buttons[2])
            {
                playerIcon.transform.position = playerIconPositions.iconTransform[2].position;
            }
            else if (currentSelectedGameObject == playerIconPositions.buttons[3])
            {
                playerIcon.transform.position = playerIconPositions.iconTransform[3].position;
            }
        }
    }
}
