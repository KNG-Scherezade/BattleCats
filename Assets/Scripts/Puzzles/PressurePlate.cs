using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private GameObject m_linkedGameObject;
    [SerializeField]
    private bool m_multipleUses = false;
    [SerializeField]
    private float m_refreshTime = 0.0f;

    public bool IsActivated { get; protected set; }
    protected bool m_yarnIsUsing = false;
    private float m_refreshTimer = 0.0f;

    private void Awake()
    {
        IsActivated = false;
    }

    private void Update()
    {
        if (IsActivated && !m_yarnIsUsing && m_multipleUses)
        {
            m_refreshTimer += Time.deltaTime;
            if (m_refreshTimer >= m_refreshTime)
            {
                m_refreshTimer = 0.0f;
                IsActivated = false;
                ToggleLinkedObject(false);
            }
        }
    }

    protected virtual void ToggleLinkedObject(bool toggle)
    {
        Gate gateScript = m_linkedGameObject.GetComponent<Gate>();
        if (gateScript != null)
        {
            if (toggle)
            {
                gateScript.IncrementActivations();
            }
            else
            {
                gateScript.DecrementActivations();
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics") && !m_yarnIsUsing)
        {
            ToggleLinkedObject(true);

            m_yarnIsUsing = true;
            IsActivated = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics") && m_yarnIsUsing)
        {
            m_yarnIsUsing = false;
        }
    }
}
