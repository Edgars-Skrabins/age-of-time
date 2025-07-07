using UnityEngine;

public class ScreenShake : Singleton<ScreenShake>
{
    [SerializeField] private float m_traumaDecay;
    [SerializeField] private float m_maxShakeMagnitude;

    [Range(0, 1)]
    private float m_trauma;
    private Vector3 m_originalPosition;

    protected override void Awake()
    {
        base.Awake();
        m_originalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (m_trauma > 0f)
        {
            float shake = m_trauma * m_trauma * m_maxShakeMagnitude;

            float offsetX = (Random.value * 2f - 1f) * shake;
            float offsetY = (Random.value * 2f - 1f) * shake;

            transform.localPosition = m_originalPosition + new Vector3(offsetX, offsetY, 0f);

            m_trauma = Mathf.Max(0, m_trauma - m_traumaDecay * Time.deltaTime);
        }
        else
        {
            transform.localPosition = m_originalPosition;
        }
    }

    public void AddTrauma(float _amount)
    {
        m_trauma = Mathf.Clamp01(m_trauma + _amount);
    }
}