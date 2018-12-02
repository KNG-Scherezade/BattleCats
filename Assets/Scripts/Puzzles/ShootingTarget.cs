using System.Text.RegularExpressions;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject m_linkedGameObject;
    [SerializeField]
    private bool m_multipleUses = false;
    [SerializeField]
    private float m_refreshTime = 0.0f;

    public bool IsActivated { get; protected set; }
    protected float m_refreshTimer = 0.0f;

    private Animator m_animator;

    private void Awake()
    {
        IsActivated = false;
        m_animator = GetComponent<Animator>();
        if (m_animator)
            m_animator.SetBool("isActive", true);
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (Regex.Match(collision.gameObject.tag, @"Yarn\wBullet", RegexOptions.IgnoreCase).Success && !IsActivated)
        {
            Destroy(collision.gameObject);
            if (m_animator)
                m_animator.SetBool("isActive", false);
            ActivateGate();
        }
    }

    public void setM_linkedGameObject(GameObject linkedObject)
    {
        m_linkedGameObject = linkedObject;
    }

    public void ActivateGate()
    {
        Gate gateScript = m_linkedGameObject.GetComponent<Gate>();
        if (gateScript != null && !gateScript.IsActivated)
        {
            gateScript.IncrementActivations();
        }

        IsActivated = true;
    }
}
