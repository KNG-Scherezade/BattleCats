using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_ExitDoor;
    [SerializeField]
    private ShootingTargetSequential[] m_ShootingTargets;
    private AudioSource m_BuzzerSound;

    private int m_ShootingIndex = 0;

    private void Awake()
    {
        m_BuzzerSound = GetComponent<AudioSource>();
    }

    public void OnTargetHit(ShootingTargetSequential target)
    {
        if (m_ShootingTargets[m_ShootingIndex] == target)
        {
            if (target.IsActivated)
                return;

            ++m_ShootingIndex;
            target.Activate(true);
            Debug.Log("Shooting index:" + m_ShootingIndex);
            if (m_ShootingIndex == m_ShootingTargets.Length)
            {
                Gate gateScript = m_ExitDoor.GetComponent<Gate>();
                gateScript.IncrementActivations();
            }
        }
        else
        {
            for (int i = 0; i <= m_ShootingIndex; ++i)
            {
                m_ShootingTargets[i].Activate(false);
            }

            m_ShootingIndex = 0;
            m_BuzzerSound.Play();
            Debug.Log("Shooting index:" + m_ShootingIndex);
        }
    }
}
