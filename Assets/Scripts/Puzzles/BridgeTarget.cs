using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class BridgeTarget : MonoBehaviour {

    private void Awake()
    {
    }

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("PLZ PRINT SOMETHING PLZ COLLIDE");
        if (Regex.Match(collision.gameObject.tag, @"Yarn\wBullet", RegexOptions.IgnoreCase).Success)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);

            foreach (Transform child in transform.parent)
            {
                if (child.name.Contains("Rope"))
                {
                    child.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
    }
}
