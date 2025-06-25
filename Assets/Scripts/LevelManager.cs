using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private float m_maxTime;
    [SerializeField] private float m_currentTimeValue;
    [SerializeField] private float m_totalTimePassed;
    [SerializeField] private float m_timeReductionMultiplier;
    public Transform m_BaseTarget;

    private float m_gameTime;

    private void Update()
    {
        if (GameManager.I.M_CurrentState == GameState.Playing)
        {
            CountGameTime();
        }
    }

    public float GetGameTime()
    {
        return m_gameTime;
    }

    private void CountGameTime()
    {
        m_gameTime += Time.deltaTime;
    }

    public float GetTime()
    {
        return m_currentTimeValue;
    }

    public void StartLevel()
    {
        m_currentTimeValue = m_maxTime;
        m_totalTimePassed = 0f;
        UIManager.Instance.InitializeGameUI(m_maxTime);
    }

    private void FixedUpdate()
    {
        UpdateGameTime();
    }

    public void SetTimeReductionMultiplier(float _value)
    {
        m_timeReductionMultiplier = _value;
    }

    private void UpdateGameTime()
    {
        if (GameManager.I.M_CurrentState == GameState.Playing)
        {
            m_currentTimeValue -= Time.deltaTime * m_timeReductionMultiplier;
            UIManager.Instance.UpdateTime(m_currentTimeValue);

            if (m_currentTimeValue <= 0)
            {
                GameManager.I.ChangeState(GameState.GameOver);
            }

            if (m_currentTimeValue > m_maxTime)
            {
                m_maxTime = m_currentTimeValue;
            }

            m_totalTimePassed += Time.deltaTime;
        }
    }

    public void AddTime(float _value)
    {
        m_currentTimeValue += _value;
    }

    public void RemoveTime(float _value)
    {
        m_currentTimeValue -= _value;
    }
}