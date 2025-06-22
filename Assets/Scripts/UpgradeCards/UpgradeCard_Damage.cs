using UnityEngine;

public class UpgradeCard_Damage : UpgradeCard
{
    [SerializeField] private float m_damageIncrease;

    protected override void DoUpgrade()
    {
        PlayerController.I.IncreaseDamage(m_damageIncrease);
    }
}