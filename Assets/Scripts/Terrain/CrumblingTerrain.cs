using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CrumblingTerrain : MonoBehaviour
{
    private class TileToRemove
    {
        private static Dictionary<Vector3Int, bool> s_tilesRemoved = new Dictionary<Vector3Int, bool>();

        public TileBase m_tile;
        public Vector3Int m_position;
        public float m_lifeTimer;
        public float m_flashTimer;

        public TileToRemove(TileBase tile, Vector3Int position)
        {
            s_tilesRemoved.Add(position, true);

            m_tile = tile;
            m_position = position;
            m_lifeTimer = 0.0f;
            m_flashTimer = 0.0f;
        }

        public void Update(float deltaTime)
        {
            UpdateTimers(deltaTime);
        }

        void UpdateTimers(float deltaTime)
        {
            m_lifeTimer += deltaTime;
            m_flashTimer += deltaTime;
        }

        public static void RemoveReference(Vector3Int position)
        {
            s_tilesRemoved.Remove(position);
        }

        public static bool IsInList(List<TileToRemove> list, Vector3Int position)
        {
            foreach (TileToRemove tile in list)
            {
                if (tile.m_position == position)
                {
                    return true;
                }
            }

            return false;
        }
    }

    private Tilemap m_tilemap;
    private List<TileToRemove> m_tilesToRemove = new List<TileToRemove>();

    public Color m_tileFlashColor = Color.red;
    public float m_tileLifeTime = 1.0f;

    private float m_tileFlashInterval;

    private void Awake()
    {
        m_tilemap = GetComponent<Tilemap>();

        m_tileFlashInterval = m_tileLifeTime / 6.0f;
    }

    void Update ()
    {
        foreach (TileToRemove tile in m_tilesToRemove.ToArray())
        {
            tile.Update(Time.deltaTime);
            Crumble(tile);
            Flash(tile);
        }
	}

    void Crumble(TileToRemove tile)
    {
        if (tile.m_lifeTimer >= m_tileLifeTime)
        {
            m_tilemap.SetTile(tile.m_position, null);            
            m_tilesToRemove.Remove(tile);
            TileToRemove.RemoveReference(tile.m_position);
        }
    }

    void Flash(TileToRemove tile)
    {
        if (tile.m_flashTimer >= m_tileFlashInterval)
        {
            tile.m_flashTimer = 0.0f;
            Color currentColor = m_tilemap.GetColor(tile.m_position);
            m_tilemap.SetColor(tile.m_position, (currentColor == Color.white ? m_tileFlashColor : Color.white));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics"))
        {
            ContactPoint2D[] contact = new ContactPoint2D[1];
            GetComponent<CompositeCollider2D>().GetContacts(contact);

            // Totally a hack; because it's very hard to get a world-to-grid-cell conversion,
            // the contact position is artificially incremented to find one viable location.
            // This also prevents the yarn from getting stuck.
            Vector2[] possibleContacts = new Vector2[9]
            {
                contact[0].point,
                contact[0].point + Vector2.up,
                contact[0].point + Vector2.down,
                contact[0].point + Vector2.left,
                contact[0].point + Vector2.right,
                contact[0].point + Vector2.up + Vector2.left,
                contact[0].point + Vector2.up + Vector2.right,
                contact[0].point + Vector2.down + Vector2.left,
                contact[0].point + Vector2.down + Vector2.right,
            };

            TileBase tile = null;
            Vector3Int tilePosition = Vector3Int.zero;

            for (uint i = 0; i != possibleContacts.Length; ++i)
            {
                tilePosition = m_tilemap.WorldToCell(possibleContacts[i]);

                if ((tile = m_tilemap.GetTile(tilePosition)) != null)
                {
                    break;
                }
            }

            if (tile != null && !TileToRemove.IsInList(m_tilesToRemove, tilePosition))
            {
                m_tilesToRemove.Add(new TileToRemove(tile, tilePosition));
            }
        }
    }
}
