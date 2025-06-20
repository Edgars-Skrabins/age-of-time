using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private float m_maxTime;
    [SerializeField] private float m_currentTimeValue;
    [SerializeField] private float m_totalTimePassed;

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

    private void UpdateGameTime()
    {
        if (GameManager.Instance.M_CurrentState == GameState.Playing)
        {
            m_currentTimeValue -= Time.deltaTime;
            UIManager.Instance.UpdateTime(m_currentTimeValue);

            if (m_currentTimeValue <= 0)
            {
                GameManager.Instance.ChangeState(GameState.GameOver);
            }

            if (m_currentTimeValue > m_maxTime)
            {
                m_maxTime = m_currentTimeValue;
                UIManager.Instance.UpdateMaxTime(m_maxTime);
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
