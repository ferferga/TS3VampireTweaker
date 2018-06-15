using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.Gameplay.Abstracts;
using VampireTweaker.Interactions.Others;

namespace VampireTweaker.Interactions
{
    public sealed class Resurrect : Interaction<Sim, Sim>
    {
        public static readonly InteractionDefinition Singleton = new Definition();
        protected override bool Run()
        {
            Resurrect.Definition definition = base.InteractionDefinition as Resurrect.Definition;
            Sim actor = this.Actor;
            Sim target = this.Target;
            this.Target.InteractionQueue.CancelAllInteractions();
            {
                this.Actor.RouteTurnToFace(this.Target.Position);
                this.Target.RouteTurnToFace(this.Actor.Position);
                base.StandardEntry();
                base.AcquireStateMachine("VampireHunt");
                base.EnterStateMachine("VampireHunt", "Enter", "x");
                base.SetActor("x", this.Actor);
                base.AnimateSim("Hunt Loop");
                OthersResurrect.GhostToSim(this.Actor, this.Target);
                base.AnimateSim("Exit");
                base.StandardExit();
                {
                    OthersResurrect.SetResurrectMoodlets();
                    OthersResurrect.setSkillPoints(this.Actor);
                    Simulator.Sleep(50u); 
                }
            }
            return true;
        }
        [DoesntRequireTuning]
        private sealed class Definition :InteractionDefinition<Sim, Sim, Resurrect>, IHasTraitIcon, IHasMenuPathIcon
        {
            protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
            {
                return Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/Resurrect:InteractionName", new object[0]);
            }
            protected override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return target.SimDescription.IsGhost && !target.SimDescription.TeenOrBelow && !target.SimDescription.IsPet && a.SimDescription.IsVampire && a.SimDescription.YoungAdultOrAbove && !a.SimDescription.TeenOrBelow;
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