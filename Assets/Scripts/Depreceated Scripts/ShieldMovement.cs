using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMovement : MonoBehaviour {

    [SerializeField] private float m_ShieldSpeed;

    private float m_MinAngle = 60f;
    private float m_MaxAngle = 210f;

    void FixedUpdate () {

        if (Input.GetKey(KeyCode.K)) // rotate right
        {
            transform.Rotate(0, 0, -Time.deltaTime * m_ShieldSpeed);
            if (transform.localEulerAngles.z > m_MinAngle && transform.localEulerAngles.z < m_MaxAngle)
                transform.localEulerAngles = new Vector3(0, 0, m_MaxAngle);
            //Debug.Log(transform.localEulerAngles.z);
        }
        if (Input.GetKey(KeyCode.J)) // rotate left
        {
            transform.Rotate(0, 0, Time.deltaTime * m_ShieldSpeed);
            if (transform.localEulerAngles.z > m_MinAngle && transform.localEulerAngles.z < m_MaxAngle)
                transform.localEulerAngles = new Vector3(0, 0, m_MinAngle);
            //Debug.Log(transform.localEulerAngles.z);
        }

    }
}
