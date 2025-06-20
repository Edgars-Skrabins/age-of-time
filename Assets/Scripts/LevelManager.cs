using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private float m_maxTime;
    [SerializeField] private float m_currentTimeValue;
    [SerializeField] private float m_totalTimePassed;
    [SerializeField] private float m_timeReductionMultiplier;

    public Transform m_BaseTarget;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        if (GameManager.Instance.M_CurrentState == GameState.Playing)
        {
            m_currentTimeValue -= Time.deltaTime * m_timeReductionMultiplier;
            UIManager.Instance.UpdateTime(m_currentTimeValue);

            if (m_currentTimeValue <= 0)
            {
                GameManager.Instance.ChangeState(GameState.GameOver);
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