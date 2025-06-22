public class UpgradeCard_RecruitSurvivor : UpgradeCard
{
    protected override void DoUpgrade()
    {
        SurvivorRecruitmentCenter.I.RecruitSurvivor();
    }
}