using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingOrder : MonoBehaviour {

    [SerializeField]
    private GameObject m_linkedGameObject;

    public bool redShot, yellowShot, greenShot, blueShot;
    private bool isActivated;

    // Use this for initialization
    void Start ()
    {
        redShot = false;
        yellowShot = false;
        greenShot = false;
        blueShot = false;
        isActivated = false;
    }
	
	// Update is called once per frame
	void Update ()
	{
        if ((yellowShot && !redShot) 
            || (greenShot && !yellowShot)
            || (blueShot && !greenShot))
	    {
	        ResetAllTargetsToFalse();
	    }

	    if (!isActivated && redShot && yellowShot && greenShot && blueShot)
	    {
	        m_linkedGameObject.transform.GetComponent<ShootingTarget>().ActivateGate();
            isActivated = true;
	    }
	}

    private void ResetAllTargetsToFalse()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        redShot = false;
        yellowShot = false;
        greenShot = false;
        blueShot = false;
    }
}
