using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private GateMoveDirection m_moveDirection;
    private GateMoveDirection m_originalMoveDirection;
    [SerializeField]
    private uint m_activationsNeeded = 1;
    [SerializeField]
    private float m_cellSize = 2.56f;
    [SerializeField]
    private float m_smoothingFactor = 0.5f;

    public enum GateMoveDirection
    {
        Up      = 0,
        Down    = 1,
        Left    = 2,
        Right   = 3
    }

    private Dictionary<GateMoveDirection, GateMoveDirection> m_oppositeDirections = new Dictionary<GateMoveDirection, GateMoveDirection>();

    public bool IsActivated { get; private set; }
    private uint m_activationCount = 0;
    
    private Rigidbody2D m_rigidbody;
    private Vector3 m_originalPosition;
    private uint m_size;

    private void Awake()
    {
        IsActivated = false;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_originalPosition = transform.position;
        
        foreach (Transform child in transform)
        {
            if (child.name.Contains("GateTile"))
            {
                ++m_size;
            }
        }

        m_originalMoveDirection = m_moveDirection;

        m_oppositeDirections.Add(GateMoveDirection.Up, GateMoveDirection.Down);
        m_oppositeDirections.Add(GateMoveDirection.Down, GateMoveDirection.Up);
        m_oppositeDirections.Add(GateMoveDirection.Left, GateMoveDirection.Right);
        m_oppositeDirections.Add(GateMoveDirection.Right, GateMoveDirection.Left);
    }

    public void DecrementActivations()
    {
        --m_activationCount;
        if (m_activationCount < m_activationsNeeded && IsActivated)
        {
            IsActivated = false;
            StopAllCoroutines();
            m_moveDirection = m_oppositeDirections[m_originalMoveDirection];
            StartCoroutine(MoveGate());
        }
    }

    public void IncrementActivations()
    {
        ++m_activationCount;
        if (m_activationCount >= m_activationsNeeded && !IsActivated)
        {
            IsActivated = true;
            StopAllCoroutines();
            m_moveDirection = m_originalMoveDirection;
            StartCoroutine(MoveGate());
        }
    }

    IEnumerator MoveGate()
    {
        Vector3 direction = Vector3.zero;

        switch (m_moveDirection)
        {
            case GateMoveDirection.Up:
                direction = Vector3.up;
                break;
            case GateMoveDirection.Down:
                direction = Vector3.down;
                break;
            case GateMoveDirection.Left:
                direction = Vector3.left;
                break;
            case GateMoveDirection.Right:
                direction = Vector3.right;
                break;
        }

        while (!ReachedFinalPosition(direction))
        {
            transform.position += direction * m_smoothingFactor;

            yield return null;
        }

        m_originalPosition = transform.position;
    }

    bool ReachedFinalPosition(Vector3 direction)
    {
        Vector3 finalPosition = m_originalPosition + m_size * m_cellSize * direction;
        bool reachedIt = false;

        switch (m_moveDirection)
        {
            case GateMoveDirection.Up:
                reachedIt = transform.position.y >= finalPosition.y;
                break;
            case GateMoveDirection.Down:
                reachedIt = transform.position.y <= finalPosition.y;
                break;
            case GateMoveDirection.Left:
                reachedIt = transform.position.x <= finalPosition.x;
                break;
            case GateMoveDirection.Right:
                reachedIt = transform.position.x >= finalPosition.x;
                break;
        }

        // Snap into position once target is reached, to prevent misalignments with terrain
        if (reachedIt)
        {
            transform.position = finalPosition;
        }

        return reachedIt;
    }
}
