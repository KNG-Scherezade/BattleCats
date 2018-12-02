using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageHightlight : MonoBehaviour {

    RawImage m_Image;
    Color m_NormalColor;
    [SerializeField] Color m_HightlightColor;     // AB004AFF

    void Start () {
        m_Image = GetComponentInChildren<RawImage>();
        m_NormalColor = m_Image.color;
	}
	
	void Update () {
		
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            m_Image.color = m_HightlightColor;
        }
        else
        {
            m_Image.color = m_NormalColor;
        }
	}
}
