using System.Collections.Generic;
using UnityEngine;

public class SurvivorRecruitmentCenter : Singleton<SurvivorRecruitmentCenter>
{
    [SerializeField] private List<GameObject> m_survivorGOs;

    public void RecruitSurvivor()
    {
        if (m_survivorGOs.Count == 0)
        {
            return;
        }
        m_survivorGOs[^1].SetActive(true);
        m_survivorGOs.RemoveAt(m_survivorGOs.Count - 1);
    }
}