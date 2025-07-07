using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : Singleton<ScreenFlash>
{
    [SerializeField] private Image m_flashImage;
    [SerializeField] private Color m_flashColor;
    [SerializeField] private float m_flashDuration;

    private void Start()
    {
        if (m_flashImage != null)
        {
            m_flashImage.color = new Color(m_flashColor.r, m_flashColor.g, m_flashColor.b, 0);
        }
    }

    public void TriggerFlash()
    {
        StartCoroutine(FlashCoroutine(m_flashDuration));
    }

    private IEnumerator FlashCoroutine(float flashDuration)
    {
        float elapsedTime = 0f;
        m_flashImage.color = new Color(m_flashColor.r, m_flashColor.g, m_flashColor.b, 1);

        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / flashDuration);
            m_flashImage.color = new Color(m_flashColor.r, m_flashColor.g, m_flashColor.b, alpha);
            yield return null;
        }

        m_flashImage.color = new Color(m_flashColor.r, m_flashColor.g, m_flashColor.b, 0);
    }
}