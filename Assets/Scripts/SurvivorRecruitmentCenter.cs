using System.Collections.Generic;
using UnityEngine;

public class SurvivorRecruitmentCenter : Singleton<SurvivorRecruitmentCenter>
{
    [SerializeField] private List<Survivor> m_survivors;
    private List<Survivor> m_inactiveSurvivors;
    private readonly List<Survivor> m_activeSurvivors = new List<Survivor>();
    private int m_timesFireRateWasIncreased;
    private bool m_allSurvivorsAreActive;

    protected override void Awake()
    {
        base.Awake();
        m_inactiveSurvivors = new List<Survivor>(m_survivors);
    }

    public void RecruitSurvivor()
    {
        if (m_inactiveSurvivors.Count == 0)
        {
            return;
        }

        Survivor survivor = m_inactiveSurvivors[^1];
        survivor.gameObject.SetActive(true);
        m_activeSurvivors.Add(survivor);
        m_inactiveSurvivors.RemoveAt(m_inactiveSurvivors.Count - 1);

        if (m_inactiveSurvivors.Count == 0)
        {
            m_allSurvivorsAreActive = true;
        }
    }

    public bool CanRecruitSurvivor()
    {
        return m_inactiveSurvivors.Count > 0;
    }

    public void UpgradeSurvivorFireRate()
    {
        foreach (Survivor survivor in m_activeSurvivors)
        {
            survivor.IncreaseFireRate();
        }
        m_timesFireRateWasIncreased += 1;
    }

    public bool CanIncreaseFireRate()
    {
        return m_timesFireRateWasIncreased < 5 && m_allSurvivorsAreActive;
    }
}