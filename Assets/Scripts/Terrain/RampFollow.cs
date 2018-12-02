using UnityEngine;
using UnityEngine.Tilemaps;

public class RampFollow : MonoBehaviour
{
    [SerializeField]
    private float m_snapForce = 3.0f;

    private CompositeCollider2D m_collider;
    private Tilemap m_tilemap;
    private Vector2 m_lastContactPoint;

    private GameObject m_yarnParent;
    private GameObject m_yarnPhysics;
    private Rigidbody2D m_yarnRigidbody;
    private float m_yarnRadius;

    private Vector2 m_rayDirection = new Vector2(0.05f, -1.0f);
    private Vector3 m_rayHitPosition;
    private Vector3 m_localSnapPosition;
    private bool m_yarnGoingDown = false;

    private void Awake()
    {
        m_collider = GetComponent<CompositeCollider2D>();
        m_tilemap = GetComponent<Tilemap>();

        m_yarnPhysics = GameObject.FindGameObjectWithTag("YarnPhysics");
        m_yarnParent = m_yarnPhysics.transform.parent.gameObject;
        m_yarnRigidbody = m_yarnPhysics.GetComponent<Rigidbody2D>();
        m_yarnRadius = m_yarnPhysics.GetComponent<CircleCollider2D>().radius;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_yarnGoingDown)
        {
            m_yarnGoingDown = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics"))
        {
            ContactPoint2D[] contact = new ContactPoint2D[1];
            m_collider.GetContacts(contact);

            m_lastContactPoint = contact[0].point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(m_lastContactPoint, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_localSnapPosition, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(m_rayHitPosition, 0.5f);
    }

    private void FixedUpdate()
    {
        if (m_yarnGoingDown)
        {
            GameObject yarn = GameObject.FindGameObjectWithTag("YarnPhysics");
            yarn.GetComponent<Rigidbody2D>().AddForce(Vector2.down * m_snapForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics"))
        {
            if (m_yarnRigidbody.velocity.y < -0.001f)
            {
                m_yarnGoingDown = true;

                Vector3 yarnLocalPositionOffset = m_yarnPhysics.transform.position - m_yarnPhysics.transform.localPosition;

                RaycastHit2D rayHit = Physics2D.Raycast(m_yarnPhysics.transform.position, m_rayDirection, Mathf.Infinity, LayerMask.GetMask("Default"));
                if (rayHit.collider != null)
                {
                    m_rayHitPosition = new Vector3(
                        rayHit.point.x,
                        rayHit.point.y,
                        0.0f);

                    m_localSnapPosition = new Vector3(
                        m_rayHitPosition.x - m_yarnParent.transform.position.x,
                        m_rayHitPosition.y - m_yarnParent.transform.position.y + m_yarnRadius,
                        0.0f);
//                    Debug.Log("Snapping to: " + m_localSnapPosition);
                    //m_yarnPhysics.transform.localPosition = m_localSnapPosition;
                }
            }
        }
    }
}
