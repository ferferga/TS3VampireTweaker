using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using System.Collections.Generic;
using VampireTweaker.ModInitiatorAndHandler;
using VampireTweaker.VampireEffects;

namespace VampireTweaker.Interactions
{
    public class EnableBlood : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, EnableBlood>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                if (Instantiator.pBloodActive)
                {
                    return "Blood are ACTIVATED";
                }
                return "Blood are DEACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new EnableBlood.Definition();
        protected override bool Run()
        {
            Instantiator.pBloodActive = !Instantiator.pBloodActive;
            if (Instantiator.pBloodActive)
            {
                VTBlood.OnWorldLoadFinishedHandler();
                SimpleMessageDialog.Show("Vampire Tweaker - Blood Effect", "Blood is enabled succesfully");
            }
            else
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Blood Effect", "Disabling blood, please be patient, it will take a while...");
                VTBlood.RemoveMakeupFromToggle();
            }
            return true;
        }

    }
    public class EnableThirst : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, EnableThirst>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                if (Instantiator.pThirstActive)
                {
                    return "Thirst are ACTIVATED";
                }
                return "Thirst are DEACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new EnableThirst.Definition();
        protected override bool Run()
        {
            Instantiator.pThirstActive = !Instantiator.pThirstActive;
            if (Instantiator.pThirstActive)
            {
                VTThirst.OnWorldLoadFinishedHandler();
                SimpleMessageDialog.Show("Vampire Tweaker - Thirst Effect", "Thirst is enabled succesfully. THIS ACTION IS ALSO APPLIED TO CHILDREN");
            }
            else
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Thirst Effect", "Disabling thirst, please be patient, it will take a while... THIS ACTION IS ALSO APPLIED TO CHILDREN");
                VTThirst.RemoveMakeupFromToggle();
            }
            return true;
        }

    }
    public class EnableConversion : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, EnableConversion>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                if (Instantiator.pConversionActive)
                {
                    return "Conversion blood are ACTIVATED";
                }
                return "Conversion blood are DEACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new EnableConversion.Definition();
        protected override bool Run()
        {
            Instantiator.pConversionActive = !Instantiator.pConversionActive;
            if (Instantiator.pConversionActive)
            {
                VTConversion.OnWorldLoadFinishedHandler();
                SimpleMessageDialog.Show("Vampire Tweaker - Conversion Effect", "Conversion is enabled succesfully.");
            }
            else
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Conversion Effect", "Disabling conversion, please be patient, it will take a while...");
                VTConversion.RemoveMakeupFromToggle();
            }
            return true;
        }

    }
    public class EnableBiteMark : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, EnableBiteMark>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                if (Instantiator.pBiteMarkActive)
                {
                    return "Bite Mark are ACTIVATED";
                }
                return "Bite Mark are DEACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new EnableBiteMark.Definition();
        protected override bool Run()
        {
            Instantiator.pBiteMarkActive = !Instantiator.pBiteMarkActive;
            if (Instantiator.pBiteMarkActive)
            {
                VTBiteMark.OnWorldLoadFinishedHandler();
                SimpleMessageDialog.Show("Vampire Tweaker - Bite Mark Effect", "Bite Mark is enabled succesfully.");
            }
            else
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Bite Mark Effect", "Disabling bite mark, please be patient, it will take a while...");
                VTBiteMark.RemoveMakeupFromToggle();
            }
            return true;
        }

    }
    public class EnableNotification : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, EnableNotification>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                if (Instantiator.pLoadNotificationDeActived)
                {
                    return "Load Notification is DEACTIVATED";
                }
                return "Load Notification is ACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new EnableNotification.Definition();
        protected override bool Run()
        {
            Instantiator.pLoadNotificationDeActived = !Instantiator.pLoadNotificationDeActived;
            if (Instantiator.pLoadNotificationDeActived)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Togglers", "Removed Notification from load");
            }
            else
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Togglers", "Added Notification");
            }
            return true;
        }

    }
    public class EnableSparkle : ImmediateInteraction<Sim, GameObject>
    {
        public static ListenerAction OnGotBuff(Event e)
        {
            Sim actor = e.Actor as Sim;
            if (actor != null)
            {
                Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(EnableSparkle.ProcessBuff), actor));
            }
            return ListenerAction.Keep;
        }
        public static void ProcessBuff(object obj)
        {
            Sim sim = obj as Sim;
            using (IEnumerator<BuffInstance> enumerator = sim.BuffManager.Buffs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BuffInstance current = enumerator.Current;
                    if (current.mBuffGuid == BuffNames.Sparkly)
                    {
                        sim.BuffManager.RemoveElement(BuffNames.Sparkly);
                    }
                }
            }
        }
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, EnableSparkle>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                if (Instantiator.pBuffActive)
                {
                    return "Sun Sparkles are ACTIVATED";
                }
                return "Sun Sparkles are DEACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new EnableSparkle.Definition();
        protected override bool Run()
        {
            Instantiator.pBuffActive = !Instantiator.pBuffActive;
            if (Instantiator.pBuffActive)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Togglers", "Activated Sparkles when a vampire is under the sun (only if have Super Vampire Lifetime Reward)");
            }
            else
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Togglers", "Disabled Sparkles when a vampire is under the sun (only if have Super Vampire Lifetime Reward)");
            }
            return true;
        }

    }
    public class Credits : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, Credits>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                return "Credits";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new Credits.Definition();
        protected override bool Run()
        {
            SimpleMessageDialog.Show("Vampire Tweaker", "Created by:fer456/nTranslations:/nGerman:Armilus/nRussian:Nihilluss/nThank you very much to all contributors and for the downloaders to help me to improve!. You can also help to improve the mod. See how in the main thread in ModTheSims");
            return true;
        }

    }
    public class HowToUninstall : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, HowToUninstall>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                return "Uninstall";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new HowToUninstall.Definition();
        protected override bool Run()
        {
            CommandSystem.ExecuteCommandString("VampireTweaker_Uninstall");
            return true;
        }

    }
    public class Reset : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, HowToUninstall>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                return "Reset and fix errors with the mod";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new Reset.Definition();
        protected override bool Run()
        {
            CommandSystem.ExecuteCommandString("VampireTweaker_Reset");
            return true;
        }

    }
    public class EnableBathroomCleaning : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, EnableBathroomCleaning>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                if (Instantiator.pBloodActive)
                {
                    return "The cleaning of the blood when you get a bath is ACTIVATED";
                }
                return "The cleaning of the blood when you get a bath is DEACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new EnableBathroomCleaning.Definition();
        protected override bool Run()
        {
            Instantiator.pBathCleaning = !Instantiator.pBathCleaning;
            if (Instantiator.pBathCleaning)
            {
                if (AcceptCancelDialog.Show("This action will be applied to the bloody mouth and the body blood. Agree?"))
                {
                    VTBlood.bathingCleans = true;
                    VTConversion.bathingCleans = true;
                    VTConversion.sConversionBathListener = EventTracker.AddListener(EventTypeId.kEventTakeBath, new ProcessEventDelegate(VTConversion.OnClean));
                    VTBlood.sBathListener = EventTracker.AddListener(EventTypeId.kEventTakeBath, new ProcessEventDelegate(VTBlood.OnClean));
                }
                else
                {
                }
            }
            else
            {
                VTBlood.bathingCleans = false;
                VTConversion.bathingCleans = false;
                EventTracker.RemoveListener(VTBlood.sBathListener);
                EventTracker.RemoveListener(VTConversion.sConversionBathListener);
            }
            return true;
        }
    }
    public class EnableShowerCleaning : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, EnableShowerCleaning>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
            {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
            };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                if (Instantiator.pShowerCleaning)
                {
                    return "The cleaning of the blood when you get a shower is ACTIVATED";
                }
                return "The cleaning of the blood when you get a shower is DEACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new EnableShowerCleaning.Definition();
        protected override bool Run()
        {
            Instantiator.pShowerCleaning = !Instantiator.pShowerCleaning;
            if (Instantiator.pShowerCleaning)
            {
                if (AcceptCancelDialog.Show("This action will be applied to the bloody mouth and the body blood. Agree?"))
                {
                    VTBlood.showeringCleans = true;
                    VTConversion.showeringCleans = true;
                    VTBlood.sShowerListener = EventTracker.AddListener(EventTypeId.kEventTakeShower, new ProcessEventDelegate(VTBlood.OnClean));
                    VTConversion.sConversionShowerListener = EventTracker.AddListener(EventTypeId.kEventTakeShower, new ProcessEventDelegate(VTConversion.OnClean));
                }
                else
                {
                }
            }
            else
            {
                VTBlood.showeringCleans = false;
                VTConversion.showeringCleans = false;
                EventTracker.RemoveListener(VTBlood.sShowerListener);
                EventTracker.RemoveListener(VTConversion.sConversionShowerListener);
            }
            return true;
        }
    }
    public class Debug : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, Debug>, IOverridesVisualType
        {
            public InteractionVisualTypes GetVisualType
            {
                get
                {
                    return InteractionVisualTypes.Immediate;
                }
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
                {
                    Localization.LocalizeString(bPath, "Interactions/VampireTweaker/EnableMenus:MenuPath", new object[0])
                };
            }
            protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair iop)
            {
                if (Instantiator.pDebugOn)
                {
                    return "Debug is ACTIVATED";
                }
                return "Debug is DEACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new Debug.Definition();
        protected override bool Run()
        {
            Instantiator.pDebugOn = !Instantiator.pDebugOn;
            if (Instantiator.pDebugOn)
            {
            }
            else
            {
            }
            return true;
        }
    }
}