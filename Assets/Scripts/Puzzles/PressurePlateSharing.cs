using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// When 2 pressure plates control the same gate

public class PressurePlateSharing : PressurePlate {

    [SerializeField]
    private bool m_isFirstObject;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics") && !m_yarnIsUsing)
        {
            if (m_isFirstObject)
            {
                ToggleLinkedObject(true);
            }
            else
            {
                ToggleLinkedObject(false);
            }
                
            m_yarnIsUsing = true;
            IsActivated = true;
        }
    }
}
