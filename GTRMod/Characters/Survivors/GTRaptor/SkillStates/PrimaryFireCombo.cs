using GTRMod.Modules.BaseStates;
using RoR2;
using UnityEngine;

namespace GTRMod.Survivors.GTRaptor.SkillStates
{
    public class PrimaryFireCombo : PrimaryOneTwoCombo
    {
        public override void OnEnter()
        {
            hitboxGroupName = "VulcanGroup";

            damageType = DamageTypeCombo.GenericPrimary;
            damageCoefficient = GTRaptorStaticValues.VulcanDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.4f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.6f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = "GTRaptorShot";
            hitSoundString = "";
            muzzleString = vulcanIndex % 2 == 0 ? "ShootLeft" : "ShootRight";
            playbackRateParam = "GTR-Kun_ShootingR.playbackRate";
            //swingEffectPrefab = GTRaptorAssets.swordSwingEffect;
            //hitEffectPrefab = GTRaptorAssets.swordHitImpactEffect;

            //impactSound = GTRaptorAssets.swordHitSoundEvent.index;

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            PlayCrossfade("Gesture, Override, Shoot", "GTR-Kun_Shooting" + (1 + vulcanIndex), playbackRateParam, duration, 0.1f * duration);
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}