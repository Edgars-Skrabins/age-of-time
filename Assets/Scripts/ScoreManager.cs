using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class ScoreManager : Singleton<ScoreManager>
{
    public int m_Score { get; private set; }

    [SerializeField] private int m_killPoint;
    [SerializeField] private int m_headshotPoint;

    private int m_totalKills;
    private int m_headshots;
    private float m_timePlayed;

    private void Update()
    {
        if (GameManager.I.M_CurrentState == GameState.Playing)
            CalculateScore();
    }

    public int CalculateScore()
    {
        m_timePlayed = LevelManager.I.GetGameTime();
        return m_Score = Mathf.FloorToInt(m_timePlayed) + m_headshots * m_headshotPoint + m_totalKills * m_killPoint;
    }

    public void AddKill()
    {
        m_totalKills++;
    }

    public void AddHeadshot()
    {
        m_headshots++;
    }
}
