using System.Collections;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TMP_Text m_popUpText;
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private float m_animationTime;
    [SerializeField] private float m_maxHeight;
    [SerializeField] private bool m_destroyOnEnd;

    public void ShowPopup(string _value)
    {
        m_popUpText.text = _value;
        m_canvasGroup.alpha = 1;
        StartCoroutine(nameof(StartMovingUp));
    }

    public void ShowPopup(string _value, Color _color)
    {
        m_popUpText.color = _color;
        m_popUpText.text = _value;
        m_canvasGroup.alpha = 1;
        StartCoroutine(nameof(StartMovingUp));
    }



    private IEnumerator StartMovingUp()
    {
        float elapsed = 0f;

        Vector3 startPos = m_popUpText.transform.localPosition;
        Vector3 targetPos = startPos + Vector3.up * m_maxHeight;

        while (elapsed < m_animationTime)
        {
            float t = elapsed / m_animationTime;

            m_popUpText.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);

            m_canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        m_popUpText.transform.localPosition = targetPos;
        m_canvasGroup.alpha = 0f;

        if (m_destroyOnEnd)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        m_popUpText.transform.localPosition = Vector3.zero;
    }
}