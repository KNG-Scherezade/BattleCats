using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakableTerrain : MonoBehaviour
{
    public HealthSlider m_yarnPhysics;
    public float m_tresholdVelocity = 40.0f;

    private Tilemap m_tilemap;
    private Vector3Int m_checkOffset = new Vector3Int(-5, -5, 0);
    private Vector3Int m_checkSize = new Vector3Int(10, 10, 0);

    private void Awake()
    {
        m_tilemap = GetComponent<Tilemap>();
        m_yarnPhysics = GameObject.FindGameObjectWithTag("YarnPhysics").GetComponent<HealthSlider>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics"))
        {
            Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            float velocityMagnitude = Vector3.Magnitude(m_yarnPhysics.CurrentYarnVelocity);

            if (velocityMagnitude >= m_tresholdVelocity)
            {
                ContactPoint2D[] contact = new ContactPoint2D[1];
                GetComponent<CompositeCollider2D>().GetContacts(contact);

                Vector3Int tilePosition = m_tilemap.WorldToCell(new Vector3(
                    contact[0].point.x,
                    contact[0].point.y,
                    0.0f));

                // Pick the tile which collided with the yarn as the center of a 10x10 bounding box
                BoundsInt checkBounds = new BoundsInt(tilePosition + m_checkOffset, m_checkSize);

                for (int x = checkBounds.xMin; x != checkBounds.xMax; ++x)
                {
                    for (int y = checkBounds.yMin; y != checkBounds.yMax; ++y)
                    {
                        Vector3Int tilePos = new Vector3Int(x, y, 0);
                        TileBase tile = m_tilemap.GetTile(tilePos);
                        if (tile != null)
                        {
                            m_tilemap.SetTile(tilePos, null);
                        }
                    }
                }

                // Restore the yarn's original velocity pre-collision
                rigidbody.velocity = m_yarnPhysics.CurrentYarnVelocity;
            }
        }
    }
}
