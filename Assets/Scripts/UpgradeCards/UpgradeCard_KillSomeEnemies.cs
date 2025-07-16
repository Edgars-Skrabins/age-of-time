using UnityEngine;

public class UpgradeCard_KillSomeEnemies : UpgradeCard
{
    [SerializeField] private Vector2 m_minMaxAmountToKill;

    protected override void DoUpgrade()
    {
        int m_amountToKill = Random.Range((int)m_minMaxAmountToKill.x, (int)m_minMaxAmountToKill.y + 1);
        EnemyManager.I.KillSpecificAmountOfEnemies(m_amountToKill);
    }
}