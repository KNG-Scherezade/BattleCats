using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour {

    [SerializeField] float m_RespawnTime;
    [SerializeField] GameObject m_Child;

	public void OnHit()
    {
        StartCoroutine(HideAndRespawn());
    }

    IEnumerator HideAndRespawn()
    {
        m_Child.SetActive(false);
        yield return new WaitForSeconds(m_RespawnTime);
        m_Child.SetActive(true);
    }
}
