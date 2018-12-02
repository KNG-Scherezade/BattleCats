using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public bool IsActivated { get; private set; }

    private void Awake()
    {
        IsActivated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("YarnPhysics") && !IsActivated)
        {
            IsActivated = true;
        }
    }
}
