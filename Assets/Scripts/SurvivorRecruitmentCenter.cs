using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SurvivorRecruitmentCenter : Singleton<SurvivorRecruitmentCenter>
{
    [FormerlySerializedAs("m_survivorGOs"), SerializeField] private List<Survivor> m_survivors;
    private int m_timesFireRateWasIncreased;
    private bool m_allSurvivorsAreActive;

    public void RecruitSurvivor()
    {
        if (m_survivors.Count == 0)
        {
            return;
        }

        m_survivors[^1].gameObject.SetActive(true);
        m_survivors.RemoveAt(m_survivors.Count - 1);
        if (m_survivors.Count == 0)
        {
            m_allSurvivorsAreActive = true;
        }
    }

    public bool CanRecruitSurvivor()
    {
        return m_survivors.Count > 0;
    }

    public void UpgradeSurvivorFireRate()
    {
        foreach (Survivor survivor in m_survivors)
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