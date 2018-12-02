using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLevelsConnector : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("YarnPhysics"))
            return;

        HealthSlider yarnHealth = collision.GetComponent<HealthSlider>();
        if (yarnHealth)
        {
            yarnHealth.DealMaxHeal();
        }

        SceneManager.LoadScene("boss");
    }
}
