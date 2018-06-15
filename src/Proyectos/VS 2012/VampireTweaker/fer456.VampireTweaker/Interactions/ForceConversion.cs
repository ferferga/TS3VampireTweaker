using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.Gameplay.Abstracts;
using VampireTweaker.Interactions.Others;
using VampireTweaker.VampireEffects;

namespace VampireTweaker.Interactions
{
    public sealed class ForceConversion : Interaction<Sim, Sim>
    {
        public static readonly InteractionDefinition Singleton = new Definition();
        protected override bool Run()
        {
            ForceConversion.Definition definition = base.InteractionDefinition as ForceConversion.Definition;
            Sim actor = this.Actor;
            Sim target = this.Target;
            this.Target.InteractionQueue.CancelAllInteractions();
            {
                this.Actor.RouteTurnToFace(this.Target.Position);
                this.Target.RouteTurnToFace(this.Actor.Position);
                base.StandardEntry();
                base.AcquireStateMachine("social_fight");
                base.EnterStateMachine("social_fight", "Enter", "x", "y");
                base.SetActor("x", this.Actor);
                base.SetActor("y", this.Target);
                base.AnimateJoinSims("init");
                base.AnimateSim("Steamed");
                VTConversion.SetMakeup(this.Actor);
                VTConversion.SetMakeup(this.Target);
                VTBlood.SetMakeup(this.Actor);
                base.StandardExit();
                {
                    OthersConversion.AddOccultState(this.Actor, this.Target);
                    OthersConversion.SetDeathMoodlets();
                    Simulator.Sleep(50u);
                    OthersConversion.VampireDrinkSuccess(this.Actor, this);
                    OthersConversion.setSkillPoints(this.Actor);
                }
            }
            return true;
        }
        [DoesntRequireTuning]
        private sealed class Definition : InteractionDefinition<Sim, Sim, ForceConversion>, IHasTraitIcon, IHasMenuPathIcon
        {
            protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair iop)
            {
                return Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/ForceConversion:InteractionName", new object[0]);
            }
            protected override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return !target.SimDescription.IsVampire && !target.SimDescription.IsGhost && !target.SimDescription.IsEP11Bot && !target.SimDescription.TeenOrBelow && !target.SimDescription.IsPet && !target.SimDescription.IsSupernaturalForm && target.SimDescription.YoungAdultOrAbove && target.SimDescription.IsHuman && a.SimDescription.IsVampire && !a.SimDescription.TeenOrBelow;
            }
            public ResourceKey GetTraitIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey("trait_vampirenocturnal_supernatural_ep7", 0u);
            }
            public ResourceKey GetPathIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey("trait_vampirenocturnal_supernatural_ep7", 0u);
            }
            public override string[] GetPath(bool bPath)
            {
                return new string[]
					{
						Localization.LocalizeString(bPath, "Interactions/LocalizedMod/VampireTweaker/MenuPath:MenuName", new object[0])
					};
            }
        }
    }
}