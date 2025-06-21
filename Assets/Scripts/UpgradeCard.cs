using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] private Image m_buttonImage;
    public Action OnButtonClick;

    public void SetButtonImage(Sprite _sprite)
    {
        m_buttonImage.sprite = _sprite;
    }

    public void Invoke_OnButtonClick()
    {
        OnButtonClick?.Invoke();
    }
}