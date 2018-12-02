using UnityEngine;

public class SineMove : MonoBehaviour
{
    public float m_amplitude = 1.0f;
    public float m_frequency = 5.0f;

    private void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + m_amplitude * Mathf.Sin(Time.time * m_frequency),
            0.0f);
    }
}
