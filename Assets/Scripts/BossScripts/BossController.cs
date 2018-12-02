using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	private CanRotatorFire rightCanScript;
	private CanRotatorFire leftCanScript;
	private BossMovement bossMovementScript;

	private bool grenadeStart;
	private bool chooseOnce;
	private bool shootStart;
	private bool strikeAttackOn;
	public bool shoot;
	public bool grenade;
	public bool strikeAttack;
	public bool automated;
	public bool onceIdleTimer;

	private float grenadeTimestamp;
	private float grenadeTimer;
	public float idleTimer;
	private float idleTimestamp;
	private float shootTimestamp;
	public float shootTimer;

	// Use this for initialization
	void Start () {
		shoot = false;
		grenade = false;
		grenadeStart = true;
		shootStart = true;
		automated = false;
		onceIdleTimer = true;
		chooseOnce = true;
		idleTimer = 5f;
		idleTimestamp = 0f;

		grenadeTimer = 11f;
		shootTimer = 10f;

		bossMovementScript = GameObject.FindGameObjectWithTag ("TanukiSensei").GetComponent<BossMovement> ();
		leftCanScript = GameObject.FindGameObjectWithTag ("BossLeftCan").GetComponent<CanRotatorFire> ();
		rightCanScript = GameObject.FindGameObjectWithTag ("BossRightCan").GetComponent<CanRotatorFire> ();
		rightCanScript.isRightCan = true;

	}
	
	// Update is called once per frame
	void Update () {
		if (automated) {
			if (onceIdleTimer) 
			{
				setIdleTimestamp ();
			}
			if (Time.time - idleTimestamp >= idleTimer) 
			{
				if (chooseOnce) {
					chooseOnce = false;
					switch (Random.Range (0, 3)){
						case 0:
							shoot = true;
							break;
						case 1:
							grenade = true;
							break;
						case 2:
							strikeAttack = true;
							break;
					}

				}
			}
		}

		if (shoot) 
		{
			if (shootStart) 
			{
				setShootTimestamp ();
			}
			Shoot ();
			if (Time.time - shootTimestamp >= shootTimer) 
			{
				leftCanScript.noAimSequence = false;
				rightCanScript.noAimSequence = false;
				shoot = false;
				shootStart = true;
				onceIdleTimer = true;
			} 
		}

		if (grenade) 
		{
			if (grenadeStart) {
				setGrenadeTimestamp ();
			}
			Grenade ();
			if (Time.time - grenadeTimestamp >= grenadeTimer) {
				grenade = false;
				grenadeStart = true;
				rightCanScript.grenadeSequence = false;
				leftCanScript.grenadeSequence = false;
				bossMovementScript.fig8Path = false;
				bossMovementScript.hoverPath = true;
				onceIdleTimer = true;
			}
		}

		if (strikeAttack) 
		{
			bossMovementScript.hoverPath = false;
			StrikeAttack ();
		}
		if (strikeAttackOn) {
			if (!bossMovementScript.strikeAttack) 
			{
				onceIdleTimer = true;
				strikeAttackOn = false;
			}
		}
	}

	void Shoot()
	{
		//if boss is to the left of the yarn, fire with right can -on
		if (bossMovementScript.currentHoverPath == 0) {
			leftCanScript.noAimSequence = false;
			rightCanScript.noAimSequence = true;
		} else 
		{
			leftCanScript.noAimSequence = true;
			rightCanScript.noAimSequence = false;
		}
	}

	void Grenade ()
	{
		bossMovementScript.hoverPath = false;
		bossMovementScript.hover = false;
		bossMovementScript.fig8Path = true;
		leftCanScript.grenadeSequence = true;
		rightCanScript.grenadeSequence = true;
	}

	void setGrenadeTimestamp()
	{
		grenadeStart = false;
		grenadeTimestamp = Time.time;
	}

	void StrikeAttack()
	{	
		bossMovementScript.strikeAttack = true;
		strikeAttackOn = true;
		strikeAttack = false;
	}

	void setIdleTimestamp(){
		idleTimestamp = Time.time;
		onceIdleTimer = false;
		chooseOnce = true;
	}

	void setShootTimestamp()
	{
		shootTimestamp = Time.time;
		shootStart = false;
	}
}
