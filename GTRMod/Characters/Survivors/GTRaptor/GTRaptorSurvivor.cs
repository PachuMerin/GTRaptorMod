using BepInEx.Configuration;
using GTRMod.Modules;
using GTRMod.Modules.Characters;
using GTRMod.Survivors.GTRaptor.Components;
using GTRMod.Survivors.GTRaptor.SkillStates;
using RoR2;
using RoR2.Skills;
using RoR2BepInExPack.GameAssetPathsBetter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GTRMod.Survivors.GTRaptor
{
    public class GTRaptorSurvivor : SurvivorBase<GTRaptorSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "gtrassets"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "GTRaptorBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string mastername => "GTRaptorMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlGTRaptor";
        public override string displayPrefabName => "GTRaptorDisplay";

        public const string GTRaptor_PREFIX = GTRaptorPlugin.DEVELOPER_PREFIX + "_GTRAPTOR_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => GTRaptor_PREFIX;
        
        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = GTRaptor_PREFIX + "NAME",
            subtitleNameToken = GTRaptor_PREFIX + "SUBTITLE",

            characterPortrait = gtrassets.LoadAsset<Texture>("texGTRIcon"),
            bodyColor = Color.white,
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 340f,
            healthRegen = 2.5f,
            armor = 25f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "GTK Head",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTK Upper Torso",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorThigh.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorThigh.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorUpperArm.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorUpperArm.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorBlaster.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorBlaster.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTKEyes.001",
                    material = gtrassets.LoadMaterial("matBlueEyes"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorGuage",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorTail",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorTorso",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorWheelMount.L",
                    material = gtrassets.LoadMaterial("matGT1"),
                },
                new CustomRendererInfo
                {
                    childName = "GTRaptorWheelMount.R",
                    material = gtrassets.LoadMaterial("matGT1"),
                }
        };

        public override UnlockableDef characterUnlockableDef => GTRaptorUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => new GTRaptorItemDisplays();

        //set in base classes
        public override AssetBundle gtrassets { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //uncomment if you have multiple characters
           // ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "GTRaptor");

           // if (!characterEnabled.Value)
                //return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            GTRaptorUnlockables.Init();

            base.InitializeCharacter();

            GTRaptorConfig.Init();
            GTRaptorStates.Init();
            GTRaptorTokens.Init();

            GTRaptorAssets.Init(gtrassets);
            GTRaptorBuffs.Init(gtrassets);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            bodyPrefab.AddComponent<GTRaptorWeaponComponent>();
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            Prefabs.SetupHitBoxGroup(characterModelObject, "VulcanGroup", "MeleeHitbox");
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your GTRaptorStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            //AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = GTRaptor_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = GTRaptor_PREFIX + "PASSIVE_DESCRIPTION",
                keywordToken = "KEYWORD_STUNNING",
                icon = gtrassets.LoadAsset<Sprite>("texPassiveIcon"),
            };

           //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
           //GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            //SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            //{
                //skillName = "GTRaptorPassive",
                //skillNameToken = GTRaptor_PREFIX + "PASSIVE_NAME",
                //skillDescriptionToken = GTRaptor_PREFIX + "PASSIVE_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                //skillIcon = gtrassets.LoadAsset<Sprite>("texPassiveIcon"),

                //unless you're somehow activating your passive like a skill, none of the following is needed.
                //but that's just me saying things. the tools are here at your disposal to do whatever you like with

                //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                //activationStateMachineName = "Weapon1",
                //interruptPriority = EntityStates.InterruptPriority.Skill,

                //baseRechargeInterval = 1f,
                //baseMaxStock = 1,

                //rechargeStock = 1,
                //requiredStock = 1,
                //stockToConsume = 1,

                //resetCooldownTimerOnUse = false,
                //fullRestockOnAssign = true,
                //dontAllowPastMaxStocks = false,
                //mustKeyPress = false,
                //beginSkillCooldownOnSkillEnd = false,

                //isCombatSkill = true,
                //canceledFromSprinting = false,
                //cancelSprintingOnActivation = false,
                //forceSprintDuringState = false,

            //});
            //Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "Dual Vulcans",
                    GTRaptor_PREFIX + "PRIMARY_SHOT_NAME",
                    GTRaptor_PREFIX + "PRIMARY_SHOT_DESCRIPTION",
                    gtrassets.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.PrimaryFireCombo)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            primarySkillDef1.stepCount = 2;
            primarySkillDef1.stepGraceDuration = 0.5f;

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Repeling Salvo",
                skillNameToken = GTRaptor_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = GTRaptor_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = gtrassets.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Blasting)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 3f,
                baseMaxStock = 2,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
        }

        private void AddUtiitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GTRaptorDash",
                skillNameToken = GTRaptor_PREFIX + "UTILITY_Dash_NAME",
                skillDescriptionToken = GTRaptor_PREFIX + "UTILITY_Dash_DESCRIPTION",
                skillIcon = gtrassets.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Dash)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Light Barrier",
                skillNameToken = GTRaptor_PREFIX + "SPECIAL_BARRIER_NAME",
                skillDescriptionToken = GTRaptor_PREFIX + "SPECIAL_BARRIER_DESCRIPTION",
                skillIcon = gtrassets.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.DeployShield)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2", interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 0f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }
        #endregion skills
        
        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                gtrassets.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshGTRaptorSword",
            //    "meshGTRaptorGun",
            //    "meshGTRaptor");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(GTRaptor_PREFIX + "MASTERY_SKIN_NAME",
                gtrassets.LoadAsset<Sprite>("texGTRAchiveIcon"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                GTRaptorUnlockables.masterySkinUnlockableDef);
                masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(gtrassets, defaultRendererinfos, null);
            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(gtrassets, defaultRendererinfos,
            // "meshGTRaptorSwordAlt",
            //null,//no gun mesh replacement. use same gun mesh
            //"meshGTRaptorAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matGTRaptorAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matGTRaptorAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matGTRaptorAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);

            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            GTRaptorAI.Init(bodyPrefab, mastername);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (sender.HasBuff(GTRaptorBuffs.armorBuff))
            {
                args.armorAdd += 300;
            }
        }
    }
}