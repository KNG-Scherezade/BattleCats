using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using EZEffects;
using System;
using UnityEngine.SceneManagement;

public class HealthSlider : MonoBehaviour
{
    public static float MaxHealth = 1500;
	public float hp_set_var;

    public Slider healthSlider;

    public float CurrentHealth { get; set; }
    public bool IsDead { get; private set; }

    public float ShakeMagnitude = 5.0f;
    public float ShakeRoughness = 4.0f;
    public float ShakeFade = 0.4f;

    private const float MAXDAMAGE = 300.0f;

    public static float InvulnerabilityDuration = 1.0f;
    private float DamageAmount;
    private bool Invulnerability;
    private float TriggerAmount;
    private float InvulnerabilityTimer;

    public GameObject ExplosionPrefab;
    public EffectImpact ImpactEffect;
    private GameManager gameManager;
	private ExtraSFX sfx;

    private Rigidbody2D YarnRigidbody;
    public Vector3 CurrentYarnVelocity { get; private set; }

    public bool BossEnded { get; set; }
    public bool LevelEnded { get; private set; }
    public bool HasLevelKey { get; private set; }

	private Vector2 last_known_velocity;
	public Vector2 knockback;
    public static float CurrentHealthMaintained = MaxHealth;

    // Use this for initialization
    void Start()
    {
		if (hp_set_var > 0) {
			MaxHealth = hp_set_var;
		}
        if (SceneManager.GetActiveScene().name == "Level2b")
        { 
            CurrentHealth = CurrentHealthMaintained;
            CurrentHealthMaintained = MaxHealth;
        }
        else 
            CurrentHealth = MaxHealth;

        print("ACTIVE SCENE IS " + SceneManager.GetActiveScene().name + " AND HEALTH IS " + CurrentHealth);

        IsDead = false;
        healthSlider.value = CalculateHealth();
        Invulnerability = false;
        TriggerAmount = MaxHealth * 0.2f;
        InvulnerabilityTimer = 0.0f;

        Debug.Log("InvulnerabilityDuration" + InvulnerabilityDuration);
        Debug.Log("MaxHealth" + MaxHealth);

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		sfx = GameObject.FindGameObjectWithTag ("ExtraSFX").GetComponent<ExtraSFX> ();
    }

    void Awake()
    {
        ImpactEffect.SetupPool();
        YarnRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        CurrentYarnVelocity = YarnRigidbody.velocity;
    }

    void Update()
    {
		last_known_velocity = this.YarnRigidbody.velocity;
        if (!gameManager.isGamePaused())
        {
            if (Invulnerability)
            {
                InvulnerabilityTimer += Time.deltaTime;
                healthSlider.fillRect.GetComponentInChildren<Image>().color = Color.magenta;
                if (InvulnerabilityTimer > InvulnerabilityDuration)
                {
                    DamageAmount = 0.0f;
                    Invulnerability = false;
                    InvulnerabilityTimer = 0.0f;
                    healthSlider.fillRect.GetComponentInChildren<Image>().color = Color.red;
                    healthSlider.value = CalculateHealth();
                }
            }

            healthSlider.value = CalculateHealth();
            //GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
            //gameManager = gameManagerObject.GetComponent<GameManager>();
        }
    }


    private void OnEnable()
    {
        // Putting this in OnEnable() so that it's reset every time a new level is loaded
        LevelEnded = false;
        //HasLevelKey = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "EnemyA" || col.tag == "EnemyB")
        {
			this.YarnRigidbody.velocity = collisionVelocity();
            DealDamage(25, col.transform.position, col.gameObject);
            DestroyEnemy(col);
        }
        else if (col.tag == "EnemyC")
        {
			this.YarnRigidbody.velocity = collisionVelocity();
            DealDamage(MAXDAMAGE, col.transform.position, col.gameObject);
            ImpactEffect.ShowImpactEffect(col.transform.position, Vector3.zero);
            DestroyEnemy(col);
        }
        else if (col.tag == "Bee")
        {
			this.YarnRigidbody.velocity = collisionVelocity();
            DealDamage(150, col.transform.position, col.gameObject);
        }
        else if (col.tag == "EnemyBShield")
        {
			this.YarnRigidbody.velocity = collisionVelocity();
            //DealDamage(25, col.transform.position, col.gameObject);
            DestroyEnemy(col);
        }
        else if (col.CompareTag("Coin"))
        {
			sfx.PlayCoinSound ();
            Destroy(col.gameObject);
            gameManager.getCoin(GameManager.coinValue);
        }
        if (col.CompareTag("EndLevel2A"))
        {
            CurrentHealthMaintained = CurrentHealth;
            SceneManager.LoadSceneAsync("Level2b");
        }

    }

	private Vector2 collisionVelocity(){
		return last_known_velocity - new Vector2(Mathf.Sign(last_known_velocity.x) * knockback.x,
			Mathf.Sign(last_known_velocity.y)*knockback.y);
	}

    private void DestroyEnemy(Collider2D col)
    {
        print(col.tag);
        if (col.gameObject.transform.parent != null && col.gameObject.transform.parent.name != "Enemies"
            &&  col.gameObject.transform.parent.name != "FlyingFish")
        {
            Destroy(col.transform.parent.gameObject);
        }
        else
        {
            Destroy(col.gameObject);
        }
    }

    public void BulletDamage(string tag, Vector3 position, GameObject gameObject)
    {
		if (tag == "EnemyABullet" || tag == "EnemyCBullet") {
			DealDamage (15, position, gameObject);
		} else if (tag == "EnemyBBullet") {
			DealDamage (50, position, gameObject);
		} else if (tag == "BossBullet") {
			DealDamage (50, position, gameObject);
		} else if (tag == "BossGrenade") {
			DealDamage (75, position, gameObject);
		} else if (tag == "BossStrike") {
			DealDamage (150, position, gameObject);
		}
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("LevelGoal"))
        {
            LevelGoalInfo goalInfo = col.gameObject.GetComponent<LevelGoalInfo>();
            //if (!goalInfo.m_requiresKey || (goalInfo.m_requiresKey && HasLevelKey))
            if (!LevelEnded)
            {
                LevelEnded = true;
                HasLevelKey = false;
                Transform fireworks = col.transform.Find("Fireworks");
                fireworks.position = transform.position;
                fireworks.gameObject.SetActive(true);
				sfx.PlayEndLevelSound ();
                Debug.Log("Level ended, queue fireworks");
                if (gameManager.HelloGrandma())
                {
                    // TODO something, ex. success window
                }
            }

            YarnRigidbody.velocity = Vector3.zero;    // Be nice and prevent the ball from falling off a cliff
            YarnRigidbody.simulated = false;

			foreach (GameObject cat in GameObject.FindGameObjectsWithTag("Cat")) {
				cat.GetComponent<PlayerScript> ().controller_number = 0;
				cat.GetComponent<CatMovement> ().controller_number = 0;

			}
        }
    }


    public void DealDamage(float damageValue, Vector3 ColPosition, GameObject gameObject)
    {        
        if (gameObject.transform.parent != null && gameObject.transform.parent.tag.Contains("Enemy"))
        {
            Destroy(gameObject.transform.parent.gameObject);
            //Destroy(col.gameObject);            
        }
        else if (gameObject.CompareTag("Bee"))
        {
            gameObject.GetComponentInParent<EnemyRespawn>().OnHit();
        }
        else if(!gameObject.transform.CompareTag("Spikes"))
        {
            Destroy(gameObject);
        }

        if (Invulnerability)
        {
            return;
        }
        CurrentHealth -= damageValue;

        // invulnerability
        DamageAmount += damageValue;
        Invulnerability |= DamageAmount >= TriggerAmount;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0.0f;
            IsDead = true;
        }
        healthSlider.value = CalculateHealth();

        float damageMagnitude = damageValue / MAXDAMAGE;

        Instantiate(ExplosionPrefab, ColPosition, Quaternion.identity);        

        if (IsDead)
        {
            StartCoroutine(gameManager.RestartLevel(GameManager.CauseOfDeath.Enemy));
        }

    }

    // For when the yarn stays on fire columns in last level
    public void FireDamage(float damageValue)
    {
        if (Invulnerability)
        {
            return;
        }
        CurrentHealth -= damageValue;

        // invulnerability
        DamageAmount += damageValue;
        Invulnerability |= DamageAmount >= TriggerAmount;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0.0f;
            IsDead = true;
        }
        healthSlider.value = CalculateHealth();
  
        if (IsDead)
        {
            StartCoroutine(gameManager.RestartLevel(GameManager.CauseOfDeath.Fire));
        }
    }

    public void UpGradeInvulnerability(float invulnerabilityTimerIncrease)
    {
        InvulnerabilityDuration += invulnerabilityTimerIncrease;
    }

    void DealHeal(float healValue)
    {
        CurrentHealth += healValue;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        healthSlider.value = CalculateHealth();
    }

    // Before entering final boss room, after deling with annoying bees and fire columns :)
    public void DealMaxHeal()
    {
        CurrentHealth = MaxHealth;
        healthSlider.value = CalculateHealth();
    }

    private float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }

    public void UpgradeMaxHealth(int v)
    {
        MaxHealth += v;
        Debug.Log(MaxHealth);
    }


}
