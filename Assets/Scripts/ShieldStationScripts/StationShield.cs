using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationShield : StationAbstract
{

    private StationGeneric generic_station;

    private GameObject m_Shield;
    private Transform m_ShieldTransform;
    private float m_ShieldSpeed = 150f;
    private float m_MinAngle = 60f;
    private float m_MaxAngle = 210f;
    private Collider2D m_Collider;
    private SpriteRenderer m_ShieldSprite;
    private bool m_FirstUse = true;

	private float shieldTimer;
	private bool shieldDirection;

	private AudioSource shield_full;

    [SerializeField]
    private bool m_keepActive = false;

    void Start()
    {
        m_Shield = GameObject.FindGameObjectWithTag("Shield");
        m_Collider = m_Shield.GetComponent<Collider2D>();
        m_ShieldSprite = m_Shield.GetComponentInChildren<SpriteRenderer>();
        m_ShieldTransform = m_Shield.transform;
        //handled by generic_station
        generic_station = gameObject.AddComponent(typeof(StationGeneric)) as StationGeneric;
        generic_station.startRoutine(this);

        m_Collider.enabled = false;
        m_ShieldSprite.enabled = false;

		shield_full = GetComponent<AudioSource> ();

		shieldTimer = 0.0f;
		shieldDirection = true;
    }

    void Update()
    {
        bool inUse = generic_station.get_in_use();

        if (m_FirstUse && inUse)
        {
            m_FirstUse = false;

            if (m_keepActive)
            {
                m_Collider.enabled = true;
                m_ShieldSprite.enabled = true;
            }
        }

        if (!m_keepActive)
        {
            m_Collider.enabled = inUse;
            m_ShieldSprite.enabled = inUse;
        }

		shieldTimer += Time.deltaTime;
		if (shieldTimer > 0.17f) 
		{
			shield_full.Stop ();
		}
    }

    public override bool actionPressed()
    {
        //handled by generic_station
        return generic_station.actionPressed();
    }

	public override void fire1Pressed()
	{      
	}

	public override void fire2Pressed()
	{
	}

	public override void fire3Pressed()
	{      
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
        if (dir_vec.x > 0) // rotate left
        {
			shieldTimer = 0.0f;
			if (!shield_full.isPlaying || !shieldDirection) 
			{
				shield_full.Play ();
				shieldDirection = true;
			}
            m_ShieldTransform.Rotate(0, 0, -Time.deltaTime * m_ShieldSpeed);
			if (m_ShieldTransform.localEulerAngles.z > m_MinAngle && m_ShieldTransform.localEulerAngles.z < m_MaxAngle) 
			{
			//m_ShieldTransform.localEulerAngles = new Vector3 (0, 0, m_MaxAngle);
			shield_full.Stop ();
            }            
        }
        if (dir_vec.x < 0) // rotate right
        {
			shieldTimer = 0.0f;
			if (!shield_full.isPlaying || shieldDirection) 
			{
				shield_full.Play ();
				shieldDirection = false;
			}
            m_ShieldTransform.Rotate(0, 0, Time.deltaTime * m_ShieldSpeed);
            if (m_ShieldTransform.localEulerAngles.z > m_MinAngle && m_ShieldTransform.localEulerAngles.z < m_MaxAngle)
            {
                //m_ShieldTransform.localEulerAngles = new Vector3 (0, 0, m_MinAngle);
                shield_full.Stop ();
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