using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSceneStarter : MonoBehaviour {

	private Transform yarn;
	private BossMovement boss;
	private BossController bossController;
	private GameObject gridOpen;
	private GameObject gridClosed;
	[SerializeField]
	private GameObject m_HealthSlider;
	[SerializeField]
	private GameObject m_HealthSliderText;

	private bool update;
	private bool once;

	// Use this for initialization
	void Start () {
		yarn = GameObject.FindGameObjectWithTag ("YarnPhysics").GetComponent<Transform>();
		boss = GameObject.FindGameObjectWithTag ("TanukiSensei").GetComponent<BossMovement>();
		bossController = GameObject.FindGameObjectWithTag ("TanukiSensei").GetComponent<BossController>();
		gridOpen = GameObject.FindGameObjectWithTag ("BossGridOpen");
		gridClosed = GameObject.FindGameObjectWithTag ("BossGridClosed");
		gridClosed.SetActive (false);
		m_HealthSlider.SetActive (false);
		m_HealthSliderText.SetActive (false);

		update = true;
		once = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (update) {
			if (yarn.position.x > -30f && once) 
			{
				gridClosed.SetActive (true);
				gridOpen.SetActive (false);
				m_HealthSlider.SetActive (true);
				m_HealthSliderText.SetActive (true);
				once = false;
			}

			if (yarn.position.x > -19.7) {
				boss.hoverPath = true;
				bossController.automated = true;
				update = false;
			}
		}
		
	}
}
