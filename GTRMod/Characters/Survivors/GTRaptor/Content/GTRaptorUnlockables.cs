using GTRMod.Survivors.GTRaptor.Achievements;
using RoR2;
using UnityEngine;

namespace GTRMod.Survivors.GTRaptor
{
    public static class GTRaptorUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                GTRaptorMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(GTRaptorMasteryAchievement.identifier),
                GTRaptorSurvivor.instance.gtrassets.LoadAsset<Sprite>("texGTRAchiveIcon"));
        }
    }
}
