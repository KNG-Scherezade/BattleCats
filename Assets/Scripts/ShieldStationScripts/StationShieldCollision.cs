using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * future script for the shield to get the damage from enemy C
 */
public class StationShieldCollision : MonoBehaviour
{

    public GameObject LaserExplosionPrefab;

    void OnCollisionEnter2D(Collision2D col)
    {        
        if (IsEnemyType(col)) 
        {
            Instantiate(LaserExplosionPrefab, col.contacts[0].point, Quaternion.identity);
            Destroy(col.gameObject);
        }

        if (IsBulletType(col))
        {
            Destroy(col.gameObject);
        }

        if (IsShieldType(col))
        {
            Instantiate(LaserExplosionPrefab, col.contacts[0].point, Quaternion.identity);
            Destroy(col.gameObject);
        }
    }

    private bool IsEnemyType(Collision2D col)
    {
        return col.collider.CompareTag("EnemyA") || col.collider.CompareTag("EnemyC") || col.collider.CompareTag("EnemyB");
    }

    private bool IsBulletType(Collision2D col)
    {
        return col.collider.CompareTag("EnemyABullet") || col.collider.CompareTag("EnemyCBullet") || col.collider.CompareTag("EnemyBBullet");
    }

    private bool IsShieldType(Collision2D col)
    {
        return col.collider.CompareTag("EnemyBShield");
    }
}
