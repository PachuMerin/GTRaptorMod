using System;
using GTRMod.Modules;
using GTRMod.Survivors.GTRaptor.Achievements;

namespace GTRMod.Survivors.GTRaptor
{
    public static class GTRaptorTokens
    {
        public static void Init()
        {
            AddGTRaptorTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("GTRaptor.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddGTRaptorTokens()
        {
            string prefix = GTRaptorSurvivor.GTRaptor_PREFIX;

            string desc = "GTRaptor is an enhanced variant of the Green Tech Corporation's. Synth GT-kun. A scout armed with twin vulcans and a shield genorator along with a raptor-like frame to handle nearly any terrain.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Dual Vulcan - a good all-rounder with the options of consistant fire from range or firing shotgun blasts for close quarters" + Environment.NewLine + Environment.NewLine
             + "< ! > Armored Rush -  has a lingering armor buff that can help getting out of tight situations." + Environment.NewLine + Environment.NewLine
             + "< ! > Light Barrier - Deploy a Shield that blocks all enemy projectiles but slows you down while in use" + Environment.NewLine + Environment.NewLine;

            string outro = "Survey Complete! Returning to base. . . . no an entity tried to eliminate me by doing that.";
            string outroFailure = "...S I G N A L  L O S T. Load another backup and send him back out.";

            Language.Add(prefix + "NAME", "GT-Raptor");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Enhanced Scouting Unit");
            Language.Add(prefix + "LORE", "Green Tech's Enhanced Scouting Unit. He will do his best.");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Ashend");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "GTRaptor passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "With a modified Item Receiver attached to the tip of the tail, you can collect items droped by your enemies from a much farther distance than others.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SHOT_NAME", "Twin Vulcans");
            Language.Add(prefix + "PRIMARY_SHOT_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * GTRaptorStaticValues.VulcanDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Repeling Salvo");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * GTRaptorStaticValues.ShotgunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Rappid Acceleration");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Quickly accelerte a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BARRIER_NAME", "Light Barrier");
            Language.Add(prefix + "SPECIAL_BARRIER_DESCRIPTION", $"Deploy a Barrier that <style=cIsUtility>Blocks incomingprojectiles</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(GTRaptorMasteryAchievement.identifier), "GTRaptor: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(GTRaptorMasteryAchievement.identifier), "As GTRaptor, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
