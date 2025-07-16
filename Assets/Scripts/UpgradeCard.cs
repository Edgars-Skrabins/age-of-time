using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradeCard : MonoBehaviour
{
    [SerializeField] private int m_cost;
    [SerializeField] private TMP_Text m_costText;
    public Action<int> OnButtonClick;
    [SerializeField] private string m_description;
    [SerializeField] private Button m_button;

    private void Awake()
    {
        m_costText.text = m_cost.ToString();
    }

    private void Update()
    {
        CheckIfIsAffordable();
    }

    public virtual bool ShouldBeAvailable()
    {
        return true;
    }

    public virtual void CheckIfIsAffordable()
    {
        if (LevelManager.I.CanPlayerAffordToBuy(m_cost))
        {
            if (m_button.interactable)
            {
                return;
            }
            m_button.interactable = true;
            return;
        }

        if (!m_button.interactable)
        {
            return;
        }

        m_button.interactable = false;
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

    public string GetDescription()
    {
        return m_description;
    }

    protected abstract void DoUpgrade();
}