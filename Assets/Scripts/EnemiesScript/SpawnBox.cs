using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour {

    [SerializeField]
    GameObject Wave;

    [SerializeField]
    GameObject camera;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "YarnPhysics")
            Instantiate(Wave, transform.position, Quaternion.identity).GetComponent<Wave>().SetCamera(camera);
    }
}
