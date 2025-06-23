public class UpgradeCard_IncreaseSurvivorFireRate : UpgradeCard
{
    public override bool ShouldBeAvailable()
    {
        return SurvivorRecruitmentCenter.I.CanIncreaseFireRate();
    }

    protected override void DoUpgrade()
    {
        SurvivorRecruitmentCenter.I.UpgradeSurvivorFireRate();
    }
}