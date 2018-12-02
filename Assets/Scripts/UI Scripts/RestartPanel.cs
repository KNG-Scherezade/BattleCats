using UnityEngine;
using UnityEngine.UI;

public class RestartPanel : MonoBehaviour
{
    private Text m_text;

    private void Awake()
    {
        m_text = transform.Find("RestartLevelText").GetComponent<Text>();
    }

    public void DisplayWithText(string text)
    {
        m_text.text = text;
    }
}
