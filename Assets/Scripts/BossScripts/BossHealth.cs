using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {

    [SerializeField]
    private GameObject m_ExplosionPrefab;
    [SerializeField]
    private GameObject m_FireworksPrefab;
	[SerializeField]
	private GameObject lightningGroup;
	private GameObject lightningClone;
	private GameObject lightningClone2;
    [SerializeField]
    private Slider m_HealthSlider;
    private Color m_NormalColor;

    [SerializeField]
    private float m_TotalHealth;
    private float m_CurrentHealth;

    [SerializeField]
    private float m_InvulnerabilityDuration;
    private float m_InvulnerabilityTimer;
    [SerializeField]
    private Color m_InvulnerabilityColor;
    [SerializeField]
    private Sprite m_InvulnerabilitySprite;
    private Sprite m_NormalSprite;
    private SpriteRenderer m_SpriteRenderer;

    public bool IsDefeated { get; private set; }

    // Use in BossMovement script if boss phases depend on its current health
    public float HealthRatio
    {
        get { return CalculateHealth(); }
    }
    public bool IsInvulnerable { get; set; }
	private bool isInvulnerableCustom;
	private bool endInvulnerableCustom;

    private void Awake()
    {
        m_CurrentHealth = m_TotalHealth;
        IsInvulnerable = false;
		isInvulnerableCustom = false;
		endInvulnerableCustom = false;
        IsDefeated = false;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_NormalSprite = m_SpriteRenderer.sprite;
    }

    private void Start()
    {
        m_HealthSlider.value = CalculateHealth();
        m_NormalColor = m_HealthSlider.fillRect.GetComponentInChildren<Image>().color;
    }

    void Update()
    {
        if (IsInvulnerable)
        {
            m_InvulnerabilityTimer += Time.deltaTime;
            m_HealthSlider.fillRect.GetComponentInChildren<Image>().color = m_InvulnerabilityColor;
            m_SpriteRenderer.sprite = m_InvulnerabilitySprite;
			lightningClone2.transform.position = (transform.position + new Vector3 (-0.5f, -3.5f, 0f));
            if (m_InvulnerabilityTimer > m_InvulnerabilityDuration)
            {
                IsInvulnerable = false;
                m_InvulnerabilityTimer = 0.0f;
                m_HealthSlider.fillRect.GetComponentInChildren<Image>().color = m_NormalColor;
                m_SpriteRenderer.sprite = m_NormalSprite;
				Destroy (lightningClone2);
            }
        }
		if (isInvulnerableCustom) 
		{
			m_HealthSlider.fillRect.GetComponentInChildren<Image>().color = m_InvulnerabilityColor;
			m_SpriteRenderer.sprite = m_InvulnerabilitySprite;
			lightningClone.transform.position =  transform.position + new Vector3(-0.5f, -3.5f, 0f);

			if (endInvulnerableCustom) 
			{
				isInvulnerableCustom = false;
				m_HealthSlider.fillRect.GetComponentInChildren<Image>().color = m_NormalColor;
				m_SpriteRenderer.sprite = m_NormalSprite;
				endInvulnerableCustom = false;
				Destroy (lightningClone);
			}
		}
    }

    private float CalculateHealth()
    {
        return m_CurrentHealth / m_TotalHealth;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("YarnABullet"))
        {
            DealDamage(25, col.transform.position, col.gameObject);
        }
        else if (col.CompareTag("YarnBBullet"))
        {
            DealDamage(50, col.transform.position, col.gameObject);
        }
        else if (col.CompareTag("YarnCBullet"))
        {
            DealDamage(75, col.transform.position, col.gameObject);
        }
    }

    void DealDamage(float damageValue, Vector3 ColPosition, GameObject bulletOject)
    {   
        Destroy(bulletOject);

		if (IsInvulnerable || isInvulnerableCustom)
        {
            return;
        }

        m_CurrentHealth -= damageValue;

        if (m_CurrentHealth <= 0.0f)
        {
            m_CurrentHealth = 0.0f;
            IsDefeated = true;
        }

        // For testing... might remove depending on boss invulnerability conditions
		if ((HealthRatio < 0.78f && HealthRatio > 0.75f) || (HealthRatio < 0.53f && HealthRatio > 0.50f) || (HealthRatio < 0.30f && HealthRatio > 0.25f))
        {
            IsInvulnerable = true;
			lightningClone2 = new GameObject ();
			lightningClone2 = Instantiate (lightningGroup, transform.position + new Vector3(-0.5f, -3.5f, 0f), transform.rotation);
		}

        m_HealthSlider.value = CalculateHealth();

        Instantiate(m_ExplosionPrefab, ColPosition, Quaternion.identity);

        if (IsDefeated)
        {
            Destroy(gameObject);
            GameObject.FindWithTag("YarnPhysics").GetComponent<HealthSlider>().BossEnded = true;
            Instantiate(m_FireworksPrefab, ColPosition, Quaternion.identity);
        }
    }

	public void StartInvulnerabilty ()
	{
		isInvulnerableCustom = true;
		lightningClone = new GameObject ();
		lightningClone = Instantiate (lightningGroup, transform.position + new Vector3(-0.5f, -3.5f, 0f), transform.rotation);

	}

	public void EndInvulnerability()
	{
		endInvulnerableCustom = true;
	}
}
