using UnityEngine;

public class UpgradeCard_FireRate : UpgradeCard
{
    [SerializeField] private float m_fireRateIncrease;

    protected override void DoUpgrade()
    {
        PlayerController.I.IncreaseFireRate(m_fireRateIncrease);
    }
}