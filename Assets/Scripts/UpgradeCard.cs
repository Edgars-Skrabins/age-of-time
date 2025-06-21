using System;
using TMPro;
using UnityEngine;

public abstract class UpgradeCard : MonoBehaviour
{
    [SerializeField] private int m_cost;
    [SerializeField] private TMP_Text m_costText;
    public Action<int> OnButtonClick;

    private void Awake()
    {
        m_costText.text = m_cost.ToString();
    }

    public void Invoke_OnButtonClick()
    {
        if (m_cost > LevelManager.Instance.GetTime())
        {
            return;
        }
        DoUpgrade();
        OnButtonClick?.Invoke(m_cost);
    }

    protected abstract void DoUpgrade();
}