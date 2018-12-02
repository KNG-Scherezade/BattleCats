using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameColumn : MonoBehaviour {

    [SerializeField]
    private float m_FireDuration = 2.5f;
    [SerializeField]
    private float m_NoFireDuration = 3.0f;
    [SerializeField]
    private float m_FireDamage = 10.0f;
	[SerializeField]
	private float m_FireOffset = 0.0f;
    private GameObject m_Fire;

    void Start () {
        m_Fire = transform.Find("FireParticle").gameObject;
        StartCoroutine(StartFire());
    }
	
	IEnumerator StartFire()
    {
		yield return new WaitForSeconds (m_FireOffset);
        while (true)
        {
            yield return new WaitForSeconds(m_FireDuration);
            m_Fire.SetActive(false);
            yield return new WaitForSeconds(m_NoFireDuration);
            m_Fire.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!m_Fire.activeInHierarchy || !collision.CompareTag("YarnPhysics"))
            return;

        HealthSlider yarnHealth = collision.GetComponent<HealthSlider>();
        if (yarnHealth && !yarnHealth.BossEnded)
        {
            yarnHealth.FireDamage(m_FireDamage);
        }
    }
}