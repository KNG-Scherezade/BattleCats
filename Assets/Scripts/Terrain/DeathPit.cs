using UnityEngine;

// Kind of deprecated if we want HealthSlider.cs to take care of this stuff
public class DeathPit : MonoBehaviour
{
    public GameObject m_splashEffect;
	private ExtraSFX sfx;

    private bool m_lifeTicking = false;
    private float m_splashLifetime;
    private float m_splashTimer = 0.0f;

    private void Start()
    {
        m_splashEffect = transform.Find("Splash").gameObject;
        m_splashLifetime = m_splashEffect.GetComponent<ParticleSystem>().main.startLifetime.constant;
		sfx = GameObject.FindGameObjectWithTag ("ExtraSFX").GetComponent<ExtraSFX>();
    }

    private void Update()
    {
        if (m_lifeTicking)
        {
            m_splashTimer += Time.deltaTime;
            if (m_splashTimer >= m_splashLifetime)
            {
                m_splashEffect.SetActive(false);
                m_lifeTicking = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics"))
        {
			sfx.PlaySplashSound ();
            m_splashEffect.SetActive(true);
            m_splashEffect.transform.position = collision.transform.position;
            m_lifeTicking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics"))
        {
            //collision.gameObject.GetComponent<HealthSlider>().DealDamage(MaxHealth);

            Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0.0f;
            rigidbody.angularVelocity = 0.0f;
            rigidbody.velocity = Vector3.zero;
            rigidbody.simulated = false;

            Debug.LogError("Through death pit; game over!");
            StartCoroutine(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RestartLevel(GameManager.CauseOfDeath.Water));
        }
    }
}
