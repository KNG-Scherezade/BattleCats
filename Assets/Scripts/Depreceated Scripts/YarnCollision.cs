using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnCollision : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("EnemyA") || 
            col.collider.CompareTag("EnemyB") || 
            col.collider.CompareTag("EnemyC") || 
            col.collider.CompareTag("EnemyABullet") ||
            col.collider.CompareTag("EnemyBBullet") ||
            col.collider.CompareTag("EnemyCBullet") ||
            col.collider.CompareTag("EnemyBShield"))
        {
            Destroy(col.gameObject);
        }
    }
}
