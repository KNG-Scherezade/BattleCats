using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObject : MonoBehaviour {

    [SerializeField]
    private GameObject m_linkedGameObject;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics"))
        {
            Destroy(m_linkedGameObject);
        }
    }
}
