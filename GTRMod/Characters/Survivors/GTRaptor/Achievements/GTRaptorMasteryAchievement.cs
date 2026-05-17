using RoR2;
using GTRMod.Modules.Achievements;

namespace GTRMod.Survivors.GTRaptor.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class GTRaptorMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = GTRaptorSurvivor.GTRaptor_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = GTRaptorSurvivor.GTRaptor_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => GTRaptorSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}