using TMPro;
using UnityEngine;

public class RecruitButton : MonoBehaviour
{
    [SerializeField] private GameObject m_survivorGameObject;
    [SerializeField] private GameObject m_recruitButtonGameObject;
    [SerializeField] private TMP_Text m_recruitCostText;
    [SerializeField] private int m_recruitCost;

    private void Start()
    {
        m_recruitCostText.text = "Recruit survivor" + m_recruitCost;
    }

    public void AttemptRecruit()
    {
        if (LevelManager.Instance.GetTime() >= m_recruitCost)
        {
            Recruit();
        }
    }

    private void Recruit()
    {
        m_recruitButtonGameObject.SetActive(false);
        m_survivorGameObject.SetActive(true);
        LevelManager.Instance.RemoveTime(m_recruitCost);
    }
}