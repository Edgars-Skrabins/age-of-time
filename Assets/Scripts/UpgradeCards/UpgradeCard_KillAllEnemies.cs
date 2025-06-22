public class UpgradeCard_KillAllEnemies : UpgradeCard
{
    protected override void DoUpgrade()
    {
        EnemyManager.I.KillAllEnemies();
    }
}