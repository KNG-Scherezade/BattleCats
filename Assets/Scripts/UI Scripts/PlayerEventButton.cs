using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerEventButton : Button {

    public EventSystem eventSystem;
    public GameObject playerIcon;
    public Transform iconTransform;
    public GameObject button;

    protected override void Awake()
    {
        base.Awake();
        eventSystem = GetComponent<PlayerEventSystemProvider>().eventSystem;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (IsInteractable() && navigation.mode != Navigation.Mode.None)
            eventSystem.SetSelectedGameObject(gameObject, eventData);

        base.OnPointerDown(eventData);
    }

    public override void Select()
    {
        if (eventSystem.alreadySelecting)
            return;

        eventSystem.SetSelectedGameObject(gameObject);
    }

}
