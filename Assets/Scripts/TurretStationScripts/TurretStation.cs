using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStation : StationAbstract
{
    private StationGeneric generic_station;

    private Animator m_Animator;

    [SerializeField]
    private GameObject m_Turret;
    private Transform m_TurretTransform;
    private float m_TurretRotationSpeed = 150f;
    [SerializeField]
    private float m_MinAngle;
    [SerializeField]
    private float m_MaxAngle;
    [SerializeField]
    private bool m_IsTopTurret;

    private SpriteRenderer m_TurretSprite;
    private bool m_FirstUse = true;
    private bool FireDown = false;

    public float munitionAFiringDelay = 0.3f;
    private float munitionATimer = 0.0f;

    public float munitionBFiringDelay = 0.7f;
    private float munitionBTimer = 0.0f;

    public float munitionCFiringDelay = 0.5f;
    private float munitionCTimer = 0.0f;

	private float turretTimer;
	private bool turretDirection;

    [SerializeField]
    private Transform nozzle;
    private Vector3 nozzlePosition;

    public GameObject munitionA;
    public GameObject munitionB;
    public GameObject munitionC;

	private AudioSource turret_full;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_TurretSprite = m_Turret.GetComponentInChildren<SpriteRenderer>();
        m_TurretTransform = m_Turret.transform;
        //handled by generic_station
        generic_station = gameObject.AddComponent(typeof(StationGeneric)) as StationGeneric;
        generic_station.startRoutine(this);

		turret_full = GetComponent<AudioSource> ();

		turretTimer = 0.0f;
		turretDirection = true;

        //m_TurretSprite.enabled = false;
    }

    void Update()
    {
        if (m_FirstUse && generic_station.get_in_use())
        {
            //m_TurretSprite.enabled = true;
            m_FirstUse = false;
        }

        munitionATimer += Time.deltaTime;
        munitionBTimer += Time.deltaTime;
        munitionCTimer += Time.deltaTime;

		turretTimer += Time.deltaTime;
		if (turretTimer > 0.17f) 
		{
			turret_full.Stop ();
		}

        m_Animator.SetBool("isActive", false);
        if (generic_station.get_in_use())
        {
            m_Animator.SetBool("isActive", true);
            nozzlePosition = nozzle.position;
        }

    }
    public override bool actionPressed()
    {      
        //handled by generic_station
        return generic_station.actionPressed();
    }

	public override void fire1Pressed()
	{      
		//munition 1,leftMouse,L1,TriggerL1
		if (munitionATimer > munitionAFiringDelay)
		{
			Instantiate(munitionA, nozzlePosition, m_TurretTransform.rotation);
			munitionATimer = 0.0f;
		}
	}

	public override void fire2Pressed()
	{      
		//munition 2,middle click,R2,TriggerR2
        if (GameManager.ammoTypeUnlocked[0]&&munitionBTimer > munitionBFiringDelay)
		{
			Instantiate(munitionB, nozzlePosition, m_TurretTransform.rotation);
			munitionBTimer = 0.0f;
		};
	}

	public override void fire3Pressed()
	{      
		//munition 3,right click,R1,TriggerR1
		if (GameManager.ammoTypeUnlocked[1] && munitionCTimer > munitionCFiringDelay)
		{
			Instantiate(munitionC, nozzlePosition, m_TurretTransform.rotation);
			munitionCTimer = 0.0f;
		};
	}

    public override bool checkDistance(Vector2 caller_location, StationAbstract station)
    {
        return generic_station.checkDistance(caller_location, station);
    }

    public override bool IsOccupied(Vector2 caller_location, StationAbstract station)
    {
        return generic_station.IsOccupied(caller_location, station);
    }

    public override void directionPressed(Vector3 dir_vec)
    {
		if (dir_vec.x > 0 || dir_vec.y > 0) // rotate left
        {
			turretTimer = 0.0f;
            m_TurretTransform.Rotate(0, 0, -Time.deltaTime * m_TurretRotationSpeed);
			if (!turret_full.isPlaying || !turretDirection) 
			{
				turret_full.Play ();
				turretDirection = true;
			}
            if (m_TurretTransform.localEulerAngles.z < m_MaxAngle)
            {
                if (!m_IsTopTurret || m_TurretTransform.localEulerAngles.z > m_MinAngle)
                    m_TurretTransform.localEulerAngles = new Vector3(0, 0, m_MaxAngle);
					turret_full.Stop ();
            }
        }
		if (dir_vec.x < 0  || dir_vec.y < 0) // rotate right
        {
			turretTimer = 0.0f;
            m_TurretTransform.Rotate(0, 0, Time.deltaTime * m_TurretRotationSpeed);
			if (!turret_full.isPlaying || turretDirection) 
			{
				turret_full.Play ();
				turretDirection = false;
			}
            if (m_TurretTransform.localEulerAngles.z > m_MinAngle)
            {
                if (!m_IsTopTurret || m_TurretTransform.localEulerAngles.z < m_MaxAngle)
                    m_TurretTransform.localEulerAngles = new Vector3(0, 0, m_MinAngle);
					turret_full.Stop ();
            }
        }
    }

    protected override void enterStation()
    {
        //handled by generic_station

    }

    protected override void exitStation()
    {
        //handled by generic_station

    }
}