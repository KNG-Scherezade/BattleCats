using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZEffects;

public class YarnBullet : MonoBehaviour {
    public float speed = 2.5f;   

    public EffectImpact ImpactEffect;

    private Vector3 Direction;
       
    void Awake()
    {
        //AudioSource[] audioSources = GetComponents<AudioSource>();
        //explosion = audioSources[0];
        ImpactEffect.SetupPool();       
        Direction = this.transform.position - GameObject.FindGameObjectWithTag("Yarn_inside_container").transform.position;
    }

    void Update()
    {      
        float step = speed * Time.deltaTime;
        
        transform.Translate(Direction.normalized * step,Space.World);    
    }  

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("EnemyA") ||
            col.collider.CompareTag("EnemyB") ||
            col.collider.CompareTag("EnemyC"))
        {
            col.gameObject.GetComponent<Enemy>().BulletCollision(this.tag);
            ImpactEffect.ShowImpactEffect(this.transform.position, Vector3.zero);
            Destroy(gameObject);
        }

        if (col.collider.CompareTag("EnemyBShield"))
        {
            col.transform.parent.GetComponent<Enemy>().BulletCollision(this.tag);
            ImpactEffect.ShowImpactEffect(this.transform.position, Vector3.zero);
            Destroy(gameObject);
        }
        
        if (col.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("OrderedTarget"))
        {
            ShootingOrder shootingOrderObject = col.transform.parent.gameObject.GetComponent<ShootingOrder>();

            if (col.gameObject.name.Contains("Red"))
            {
                shootingOrderObject.redShot = true;
                col.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            }
            else if (col.gameObject.name.Contains("Yellow"))
            {
                shootingOrderObject.yellowShot = true;
                col.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            }
            else if (col.gameObject.name.Contains("Green"))
            {
                shootingOrderObject.greenShot = true;
                col.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            }
            else if (col.gameObject.name.Contains("Blue"))
            {
                shootingOrderObject.blueShot = true;
                col.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            }

            Destroy(gameObject);
        }
    }
}
