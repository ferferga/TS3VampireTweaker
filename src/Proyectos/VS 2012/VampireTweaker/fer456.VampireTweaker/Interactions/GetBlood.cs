using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.Gameplay.Abstracts;
using Sims3.UI;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.ActorSystems;

namespace VampireTweaker.Interactions
{
    public class GetBlood : RabbitHole.RabbitHoleInteraction<Sim, RabbitHole>
    {        
        private static int kDefaultPrice;
        static GetBlood()
        {
            kDefaultPrice = 175;
        }
        protected sealed class Definition : InteractionDefinition<Sim, RabbitHole, GetBlood>, IHasTraitIcon
        {
            public ResourceKey GetTraitIcon(Sim actor, GameObject target)
            {
                return ResourceKey.CreatePNGKey("trait_vampirenocturnal_supernatural_ep7", 0u);
            }
            protected override string GetInteractionName(Sim actor, RabbitHole target, InteractionObjectPair iop)
            {
                return Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/GetBlood:InteractionName", new object[0]);
            }
            protected override bool Test(Sim actor, RabbitHole target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
            {
                return actor.SimDescription.IsVampire && actor.SimDescription.ChildOrAbove;
            }
        }
        public static readonly InteractionDefinition Singleton = new GetBlood.Definition();
        protected override bool InRabbitHole()
        {
            base.BeginCommodityUpdates();
            float duration;
            {
                {
                    duration = 20f;
                }
                bool flag = base.DoTimedLoop(duration);
                base.EndCommodityUpdates(flag);
                if (flag)
                {                   
                    GetBlood.Pay(Actor);
                    GetBlood.SetSkillPoints(Actor);
                    GetBlood.SetMoodlets(Actor);                    
                    if (Actor.SimDescription.YoungAdultOrAbove)
                    {                        
                        StyledNotification.Show(new StyledNotification.Format(Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/Notifications/GetBlood:FinalNotification", new object[0]), StyledNotification.NotificationStyle.kSimTalking));
                        GetBlood.setMaxMotive(Actor, CommodityKind.VampireThirst);
                    }
                    if (Actor.SimDescription.ChildOrAbove && Actor.SimDescription.TeenOrBelow)
                    {
                        StyledNotification.Show(new StyledNotification.Format(Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/Notifications/GetBlood:ChildOrAboveFinalNotification", new object[0]), StyledNotification.NotificationStyle.kSimTalking));
                        GetBlood.setMaxMotive(Actor, CommodityKind.Hunger);
                        GetBlood.setMaxMotive(Actor, CommodityKind.Energy);
                    }
                }
                return flag;
            }
        }
        public bool IsAvailableFor(RabbitHoleType rabbitHoleType)
        {
            return rabbitHoleType == RabbitHoleType.Hospital;
        }
        public static void setMaxMotive(Sim actor, CommodityKind type)
        {
            actor.Motives.SetValue(type, 100f);
        }
        public static void Pay(Sim sim)
        {
            int num = kDefaultPrice;
            if (!sim.IsNPC && sim.Household.FamilyFunds >= num)
            {
                sim.Household.ModifyFamilyFunds(-num);
                StyledNotification.Show(new StyledNotification.Format(sim.FullName + Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/Notifications/GetBlood:PaidText", new object[0]) + UIUtils.FormatMoney(num) + Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/Notifications/GetBlood:ForBlood", new object[0]), sim.ObjectId, StyledNotification.NotificationStyle.kGameMessagePositive));
            }
            if (!sim.IsNPC && sim.Household.FamilyFunds < num)
            {
                Household household = sim.Household;
                household.UnpaidBills += num;
                StyledNotification.Show(new StyledNotification.Format(sim.FullName + Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/Notifications/GetBlood:CantAffordNotification", new object[0]) + "\n\n" + UIUtils.FormatMoney(num) + Localization.LocalizeString("Interactions/LocalizedMod/VampireTweaker/Notification/GetBlood:SimoleansAddedToUnpaidBills", new object[0]), sim.ObjectId, StyledNotification.NotificationStyle.kGameMessageNegative));
            }
        }
        public static void SetMoodlets(Sim sim)
        {
            if (sim.SimDescription.YoungAdultOrAbove)
            {
                sim.BuffManager.AddElement(0xDDA3184CBEAFDF22, (Origin)ResourceUtils.HashString64("ByGettingBlood"));
            }
            if (sim.SimDescription.ChildOrAbove && sim.SimDescription.TeenOrBelow)
            {
                sim.BuffManager.AddElement(0xB8C52F5C23B7F50C, (Origin)ResourceUtils.HashString64("ByGettingBloodChild"));                
            }
        }
        public static void SetSkillPoints(Sim sim)
        {
            if (!sim.SkillManager.HasElement((SkillNames)0x11F10A98))
            {
                sim.SkillManager.AddElement((SkillNames)0x11F10A98);
            }
            if (sim.SimDescription.YoungAdultOrAbove)
            {
                sim.SkillManager.AddSkillPoints((SkillNames)0x11F10A98, -25f);
            }
            if (sim.SimDescription.ChildOrAbove)
            {
                sim.SkillManager.AddSkillPoints((SkillNames)0x11F10A98, 25f);
            }
        }
    }
}