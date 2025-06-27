using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private float m_shakeMagnitude = 10f;
    [SerializeField] private float m_shakeSpeed = 50f;

    private RectTransform m_rectTransform;
    private Vector3 m_originalPosition;
    private Coroutine m_shakeRoutine;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_originalPosition = m_rectTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_shakeRoutine == null)
            m_shakeRoutine = StartCoroutine(ShakeLoop());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_shakeRoutine != null)
        {
            StopCoroutine(m_shakeRoutine);
            m_shakeRoutine = null;
            m_rectTransform.anchoredPosition = m_originalPosition;
        }
    }

    private IEnumerator ShakeLoop()
    {
        while (true)
        {
            float offsetX = Mathf.PerlinNoise(Time.unscaledTime * m_shakeSpeed, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, Time.unscaledTime * m_shakeSpeed) * 2f - 1f;

            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f) * m_shakeMagnitude;
            m_rectTransform.anchoredPosition = m_originalPosition + shakeOffset;

            yield return null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.I.PlaySound("SFX_Click");
    }
}