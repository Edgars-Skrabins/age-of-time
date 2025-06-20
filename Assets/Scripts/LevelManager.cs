using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private float m_startingMaxTime;
    [SerializeField] private float m_currentTimeValue;
    [SerializeField] private float m_totalTimePassed;

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
        m_currentTimeValue = m_startingMaxTime;
        UIManager.Instance.InitializeGameUI(m_startingMaxTime);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.M_CurrentState == GameState.Playing)
        {
            m_currentTimeValue -= Time.deltaTime;
            UIManager.Instance.UpdateTime(m_currentTimeValue);
        }
    }
}
