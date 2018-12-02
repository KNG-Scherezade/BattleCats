using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour {

	public float radius;
	public int explosiveDelay;

	public Transform explosion;


	// Use this for initialization
	void Start () {
		radius = 5.0f;
		explosiveDelay = 5;
		StartCoroutine (Explode ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Explode(){
		yield return new WaitForSeconds (explosiveDelay);
		Instantiate (explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "YarnPhysics")
        {
            col.gameObject.GetComponent<HealthSlider>().BulletDamage(this.tag, this.transform.position, this.gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
        else if (col.gameObject.tag == "YarnABullet" || col.gameObject.tag == "YarnBBullet" || col.gameObject.tag == "YarnCBullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            StopAllCoroutines();
            Destroy(this.gameObject);
            Destroy(col.gameObject);
        }
    }
}
