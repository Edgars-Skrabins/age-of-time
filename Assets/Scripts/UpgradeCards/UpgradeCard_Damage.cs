using UnityEngine;

public class UpgradeCard_Damage : UpgradeCard
{
    [SerializeField] private float m_damageIncrease;

    public override bool ShouldBeAvailable()
    {
        return PlayerController.I.CanIncreaseDamage();
    }

    protected override void DoUpgrade()
    {
        PlayerController.I.IncreaseDamage(m_damageIncrease);
    }
}