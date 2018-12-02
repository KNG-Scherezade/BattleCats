using System.Text.RegularExpressions;
using UnityEngine;


public class ShootingTargetSequential : MonoBehaviour {

    [SerializeField]
    private PuzzleManager m_PuzzleManager;

    private Animator m_Animator;

    public bool IsActivated { get; private set; }

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Regex.Match(collision.gameObject.tag, @"Yarn\wBullet", RegexOptions.IgnoreCase).Success && !IsActivated)
        {
            Destroy(collision.gameObject);
            m_PuzzleManager.OnTargetHit(this);
        }
    }

    public void Activate(bool setActive)
    {
        if (setActive)
        {
            m_Animator.SetBool("isActive", false);
            IsActivated = true;
        }
        else
        {
            m_Animator.SetBool("isActive", true);
            IsActivated = false;
        }

    }
}
