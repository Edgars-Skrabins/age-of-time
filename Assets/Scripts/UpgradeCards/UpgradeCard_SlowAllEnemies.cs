using UnityEngine;

public class UpgradeCard_SlowAllEnemies : UpgradeCard
{
    [SerializeField] private float m_slowDuration;

    protected override void DoUpgrade()
    {
        EnemyManager.I.SlowAllEnemies(m_slowDuration);
    }
}