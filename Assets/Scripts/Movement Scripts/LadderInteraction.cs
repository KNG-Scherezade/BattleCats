using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderInteraction : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cat"))
        {
            other.gameObject.GetComponent<CatMovement>().SetOnLadder(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cat"))
        {
            other.gameObject.GetComponent<CatMovement>().SetOnLadder(false);
        }
    }
}
