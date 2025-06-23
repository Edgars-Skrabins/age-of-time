public class UpgradeCard_RecruitSurvivor : UpgradeCard
{
    public override bool ShouldBeAvailable()
    {
        return SurvivorRecruitmentCenter.I.CanRecruitSurvivor();
    }

    protected override void DoUpgrade()
    {
        SurvivorRecruitmentCenter.I.RecruitSurvivor();
    }
}