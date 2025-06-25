using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private float shakeMagnitude = 10f;
    [SerializeField] private float shakeSpeed = 50f;

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (shakeRoutine == null)
            shakeRoutine = StartCoroutine(ShakeLoop());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
            shakeRoutine = null;
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    private IEnumerator ShakeLoop()
    {
        while (true)
        {
            float offsetX = Mathf.PerlinNoise(Time.unscaledTime * shakeSpeed, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, Time.unscaledTime * shakeSpeed) * 2f - 1f;

            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f) * shakeMagnitude;
            rectTransform.anchoredPosition = originalPosition + shakeOffset;

            yield return null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.I.PlaySound("SFX_Click");
    }
}