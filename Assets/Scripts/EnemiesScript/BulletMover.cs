using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class BulletMover : MonoBehaviour
{
    public float speed;

    public Vector2 target;

    //public AudioSource explosion;

    //[SerializeField]
    //public GameObject exploPrefab;

    void Awake()
    {
        //AudioSource[] audioSources = GetComponents<AudioSource>();
        //explosion = audioSources[0];
        target = GameObject.FindGameObjectWithTag("Yarn_inside_container").transform.position;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target) > 0.3)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, step);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //print(col.tag);
        if (col.tag == "YarnPhysics")
        {
            col.gameObject.GetComponent<HealthSlider>().BulletDamage(this.tag,this.transform.position, this.gameObject);
        }
        if (!col.gameObject.name.Contains("FlameColumn"))
        {
            Destroy(this.gameObject);
        }
    }

}
