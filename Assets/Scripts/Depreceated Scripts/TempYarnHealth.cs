using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempYarnHealth : MonoBehaviour {

    [SerializeField] 
    float Health;


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("EnemyA") || col.collider.CompareTag("EnemyB"))
        {
            TakeDamage(25, col);
        }
        else if (col.collider.CompareTag("EnemyC"))
        {
            TakeDamage(300, col);
        }
        else if (col.collider.CompareTag("EnemyABullet") || col.collider.CompareTag("EnemyCBullet"))
        {
            TakeDamage(15, col);
        }
        else if (col.collider.CompareTag("EnemyBBullet"))
        {
            TakeDamage(50, col);
        }
        else if (col.collider.CompareTag("EnemyBShield"))
        {
            Destroy(col.gameObject);
        }
    }

    public void TakeDamage(float damageDone, Collision2D col)
    {
        Health -= damageDone;
        print(Health);
        Destroy(col.gameObject);   

    }
}
