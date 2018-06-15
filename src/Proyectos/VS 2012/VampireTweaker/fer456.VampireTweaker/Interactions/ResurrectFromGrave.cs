using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Actors;
using Sims3.SimIFace;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Abstracts;
using VampireTweaker.Interactions.Others;

namespace VampireTweaker.Interactions
{
    public sealed class ResurrectFromGrave : Interaction<Sim, Urnstone>
    {
        public static readonly InteractionDefinition Singleton = new Definition();
        protected override bool Run()
        {
            ResurrectFromGrave.Definition definition = base.InteractionDefinition as ResurrectFromGrave.Definition;
            Sim actor = this.Actor;
            GameObject urnstone = this.Target;
            {
                this.Actor.RouteToPoint(this.Target.Position);
                base.StandardEntry();
                base.AcquireStateMachine("VampireHunt");
                base.EnterStateMachine("VampireHunt", "Enter", "x");
                base.SetActor("x", this.Actor);
                base.AnimateSim("Hunt Loop");
                base.StandardExit();
                {
                    OthersResurrect.SetResurrectMoodletsToUrnstone();
                    OthersResurrect.setSkillPoints(this.Actor);
                }
            }
            return true;
        }
        [DoesntRequireTuning]
        private sealed class Definition : InteractionDefinition<Sim, Urnstone, ResurrectFromGrave>, IHasTraitIcon, IHasMenuPathIcon
        {
            protected override string GetInteractionName(Sim actor, Urnstone target, InteractionObjectPair iop)
            {
                return Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/ResurrectFromGrave:InteractionName", new object[0]);
            }
            protected override bool Test(Sim a, Urnstone target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return a.SimDescription.IsVampire && !a.SimDescription.TeenOrBelow && target.DeadSimsDescription.IsHuman && target.DeadSimsDescription.IsValid;
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