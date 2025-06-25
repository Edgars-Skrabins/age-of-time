using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tips : MonoBehaviour
{
    [SerializeField] private TMP_Text m_tipsText;
    [SerializeField] private string[] m_tips;

    private void Start()
    {
        string randomTip = m_tips[Random.Range(0, m_tips.Length)];
        m_tipsText.text = randomTip;
    }
}