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
    public sealed class Kill : Interaction<Sim, Sim>
    {
        public static readonly InteractionDefinition Singleton = new Definition();
        protected override bool Run()
        {
            Kill.Definition definition = base.InteractionDefinition as Kill.Definition;
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
                VTBlood.SetMakeup(this.Actor);
                VTConversion.SetMakeup(this.Target);
                VTConversion.SetMakeup(this.Actor);
                base.StandardExit();
                {
                    OthersKill.VampVictimDeath(this.Actor, this.Target);
                    OthersKill.SetDeathMoodlets();
                    Simulator.Sleep(50u);
                    OthersKill.VampireDrinkSuccess(this.Actor, this);
                    OthersKill.setSkillPoints(this.Actor);

                }
            }
            return true;
        }
        [DoesntRequireTuning]
        private sealed class Definition : InteractionDefinition<Sim, Sim, Kill>, IHasTraitIcon, IHasMenuPathIcon
        {
            protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair iop)
            {
                return Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/Kill:InteractionName", new object[0]);
            }
            protected override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return !target.SimDescription.IsVampire && !target.SimDescription.IsGhost && !target.SimDescription.IsEP11Bot && !target.SimDescription.TeenOrBelow && !target.SimDescription.IsPet && target.SimDescription.YoungAdultOrAbove && a.SimDescription.IsVampire && !a.SimDescription.TeenOrBelow;
            }
            public override string[] GetPath(bool isFemale)
            {
                return new string[]
                {
                    Localization.LocalizeString(isFemale, "Interactions/LocalizedMod/VampireTweaker/MenuPath:MenuName", new object[0])
                };
            }
            public ResourceKey GetPathIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey("trait_vampirenocturnal_supernatural_ep7", 0u);
            }
            public ResourceKey GetTraitIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey("trait_vampirenocturnal_supernatural_ep7", 0u);
            }
        }
    }
}