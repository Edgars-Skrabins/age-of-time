using UnityEngine;

public class UpgradeCard_FireRate : UpgradeCard
{
    [SerializeField] private float m_fireRateIncrease;

    public override bool ShouldBeAvailable()
    {
        return PlayerController.I.CanIncreaseFireRate();
    }

    protected override void DoUpgrade()
    {
        PlayerController.I.IncreaseFireRate(m_fireRateIncrease);
    }
}