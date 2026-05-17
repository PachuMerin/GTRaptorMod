using RoR2;
using UnityEngine;

namespace GTRMod.Survivors.GTRaptor
{
    public static class GTRaptorBuffs
    {
        // armor buff gained during Dash
        public static BuffDef armorBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("GTRaptorArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

        }
    }
}
