using System;
using TMPro;
using UnityEngine;

public abstract class UpgradeCard : MonoBehaviour
{
    [SerializeField] private int m_cost;
    [SerializeField] private TMP_Text m_costText;
    public Action<int> OnButtonClick;
    [SerializeField] private string m_description;

    private void Awake()
    {
        m_costText.text = m_cost.ToString();
    }

    public void Invoke_OnButtonClick()
    {
        if (m_cost > LevelManager.I.GetTime())
        {
            return;
        }
        DoUpgrade();
        OnButtonClick?.Invoke(m_cost);
    }

    public string GetDescription() { return m_description; }

    protected abstract void DoUpgrade();
}