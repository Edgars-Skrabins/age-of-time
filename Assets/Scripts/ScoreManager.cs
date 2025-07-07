using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public int m_Score {get; private set;}

    [SerializeField] private int m_killPoint;
    [SerializeField] private int m_headshotPoint;
    [Header("UI")]
    [SerializeField] private TMP_Text m_timeScoreText;
    [SerializeField] private TMP_Text m_killScoreText;
    [SerializeField] private TMP_Text m_headshotScoreText;
    [SerializeField] private GameObject m_ScorePanel;

    private int m_totalKills;
    private int m_headshots;
    private float m_timePlayed;

    [SerializeField] private int m_bonusPoints;
    private bool m_won;

    private void Update()
    {
        if (GameManager.I.M_CurrentState == GameState.Playing)
        {
            CalculateScore();
            m_ScorePanel.SetActive(false);
        }

        if (GameManager.I.M_CurrentState == GameState.MainMenu)
        {
            m_ScorePanel.SetActive(false);
        }

        if (GameManager.I.M_CurrentState == GameState.Paused || GameManager.I.M_CurrentState == GameState.GameOver ||
            GameManager.I.M_CurrentState == GameState.Victory)
        {
            m_ScorePanel.SetActive(true);
        }
    }

    public int CalculateScore()
    {
        m_timePlayed = Mathf.FloorToInt(LevelManager.I.GetGameTime());

        m_timeScoreText.text = ((int)m_timePlayed).ToString();
        m_killScoreText.text = m_totalKills.ToString();
        m_headshotScoreText.text = m_headshots.ToString();

        if (m_won)
        {
            m_timePlayed += m_bonusPoints;
        }

        return m_Score = (int)m_timePlayed + m_headshots * m_headshotPoint + m_totalKills * m_killPoint;
    }

    public void AddKill()
    {
        m_totalKills++;
    }

    public void AddHeadshot()
    {
        m_headshots++;
    }

    public void AddBonusPoints()
    {
        m_won = true;
    }
}