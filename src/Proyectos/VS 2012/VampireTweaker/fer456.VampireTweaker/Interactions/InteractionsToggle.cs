using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using VampireTweaker.ModInitiatorAndHandler;

namespace VampireTweaker.Interactions
{
    public class EnableMenus : ImmediateInteraction<Sim, GameObject>
    {
        [DoesntRequireTuning]
        private sealed class Definition : ImmediateInteractionDefinition<Sim, GameObject, EnableMenus>, IOverridesVisualType
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
                if (Instantiator.pInteractionsActive)
                {
                    return "Interactions are ACTIVATED";
                }
                return "Interactions are DEACTIVATED";
            }
            protected override bool Test(Sim a, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return true;
            }
        }
        public static readonly InteractionDefinition Singleton = new EnableMenus.Definition();
        protected override bool Run()
        {            
            Instantiator.pInteractionsActive = !Instantiator.pInteractionsActive;
            if (Instantiator.pInteractionsActive)
            {
                Instantiator.AddAllInteractionsForSims();
                Instantiator.AddAllInteractionsForHospitals();
                Instantiator.AddAllInteractionsForUrnstones();
            }
            else
            {
                Instantiator.RemoveAllInteractionsForSims();
                Instantiator.RemoveAllInteractionsForHospitals();
                Instantiator.RemoveAllInteractionsForUrnstones();
            }
            return true;
        }
    }
}