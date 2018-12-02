using System.Collections;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField]
    private bool m_preserveYarnVelocity = true;
    [SerializeField]
    private bool m_multipleUses = true;
    [SerializeField]
    private float m_refreshTime = 2.0f;
    [SerializeField]
    private float m_springValue = 350.0f;
    [SerializeField]
    private Sprite m_spriteActive;
    private Sprite m_spriteInactive;
    private SpriteRenderer m_spriteRenderer;

    public bool IsActivated { get; private set; }
    private float m_refreshTimer = 0.0f;

    private void Awake()
    {
        IsActivated = false;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteInactive = m_spriteRenderer.sprite;
    }

    private void Update()
    {
        if (IsActivated && m_multipleUses)
        {
            m_refreshTimer += Time.deltaTime;
            if (m_refreshTimer >= m_refreshTime)
            {
                m_refreshTimer = 0.0f;
                IsActivated = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("YarnPhysics") && !IsActivated)
        {
            Rigidbody2D rigidbody = other.GetComponent<Rigidbody2D>();

            if (!m_preserveYarnVelocity)
            {
                rigidbody.velocity = Vector3.zero;
            }
            rigidbody.AddForce(transform.up * m_springValue, ForceMode2D.Impulse);

            IsActivated = true;
            StartCoroutine(Activate());
        }
    }

    IEnumerator Activate()
    {
        m_spriteRenderer.sprite = m_spriteActive;
        yield return new WaitForSeconds(3);
        m_spriteRenderer.sprite = m_spriteInactive;
    }
}
