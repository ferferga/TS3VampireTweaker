using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;

namespace VampireTweaker.Interactions.Others
{
    internal class OthersConversion
    {
        public static Sim player1;
        public static Sim player2;        

        public static bool CanDrink(uint vamp, uint vic)
        {
            return vamp > vic;
        }
        public static void AddOccultState(Sim vamp, Sim vic)
        {
            OthersConversion.player1 = vamp;
            OthersConversion.player2 = vic;
            OthersConversion.player2.BuffManager.AddElement(BuffNames.VampireBite, Origin.FromVampire);
            OthersConversion.player2.BuffManager.AddElement(0x7A197492362B6540, (Origin)ResourceUtils.HashString64("ByBeingConvertedByTheVampire"));
            OthersConversion.player1.BuffManager.AddElement(0xB040AC12800F6F7D, (Origin)ResourceUtils.HashString64("ByConvertingASim"));
        }
        public static void VampireDrinkSuccess(Sim actor, InteractionInstance i)
        {           
            OthersConversion.setMaxMotive(actor, CommodityKind.Energy);
            OthersConversion.setThirstMotive(actor, CommodityKind.VampireThirst);
            OthersConversion.setTargetMotive(player2, CommodityKind.Hunger);
            OthersConversion.setTargetMotive(player2, CommodityKind.Energy);
            if (actor.TraitManager.HasElement(TraitNames.Vegetarian))
            {
                actor.BuffManager.AddElement(BuffNames.Nauseous, Origin.FromCarnivorousBehavior);
            }
            else
            {
                actor.BuffManager.AddElement(BuffNames.Sated, Origin.FromReceivingVampireNutrients);
            }
            {
                actor.BuffManager.AddElement(0xB040AC12800F6F7D, (Origin)ResourceUtils.HashString64("ByConvertingASim"));
                actor.BuffManager.AddElement(0xD224DD43B0C28B1F, (Origin)ResourceUtils.HashString64("ForBitingASim"));                
            }
        }
        public static bool childVersion(Sim target)
        {
            return !target.SimDescription.TeenOrBelow && !target.SimDescription.IsPregnant;
        }
        public static void SetDeathMoodlets()
        {
            if (OthersConversion.player1.TraitManager.HasElement(TraitNames.Good) || OthersConversion.player1.TraitManager.HasElement(TraitNames.Friendly))
            {                
                OthersConversion.player1.BuffManager.AddElement(0xB040AC12800F6F7D, (Origin)ResourceUtils.HashString64("ByConvertingASim"));
                OthersConversion.player1.BuffManager.AddElement(0xD224DD43B0C28B1F, (Origin)ResourceUtils.HashString64("ForBitingASim"));
            }
            else
            {
                if (OthersConversion.player2.IsBloodRelated(OthersConversion.player1) && OthersConversion.player1.TraitManager.HasElement(TraitNames.FamilyOriented))
                {
                    OthersConversion.player1.BuffManager.AddElement(0xB040AC12800F6F7D, (Origin)ResourceUtils.HashString64("ByConvertingASim"));
                    OthersConversion.player1.BuffManager.AddElement(0xD224DD43B0C28B1F, (Origin)ResourceUtils.HashString64("ForBitingASim"));
                }
                else
                {
                    OthersConversion.player1.BuffManager.AddElement(0xB040AC12800F6F7D, (Origin)ResourceUtils.HashString64("ByConvertingASim"));
                    OthersConversion.player1.BuffManager.AddElement(0xD224DD43B0C28B1F, (Origin)ResourceUtils.HashString64("ForBitingASim"));
                }
            }
            foreach (Sim current in OthersConversion.player1.LotCurrent.GetSims())
            {
                if (current != OthersConversion.player1 && current.RoomId == OthersConversion.player1.RoomId && current != OthersConversion.player2)
                {
                    Relationship relationship = Relationship.Get(OthersConversion.player1, current, true);
                    float liking = relationship.LTR.Liking;
                    if (current.TraitManager.HasElement(TraitNames.Evil) || current.TraitManager.HasElement(TraitNames.MeanSpirited))
                    {                        
                        relationship.LTR.SetLiking(liking - 20f);
                    }
                    else
                    {
                        if (current.SimDescription.TeenOrAbove)
                        {
                            OthersConversion.player1.SocialComponent.SetHasTreatedKidsPoorly();
                        }                        
                        relationship.LTR.SetLiking(liking + 100f);
                    }
                }
            }
        }
        public static void setMaxMotive(Sim actor, CommodityKind type)
        {
            actor.Motives.SetValue(type, - 40f);
        }
        public static void setTargetMotive(Sim target, CommodityKind type)
        {
            target.Motives.SetValue(type, -15f);
        }
        public static void setThirstMotive(Sim target, CommodityKind type)
        {
            target.Motives.SetValue(type, 100f);
        }
        public static void setSkillPoints(Sim actor)
        {
            if (!actor.SkillManager.HasElement(0x11F10A98))
            {
                actor.SkillManager.AddElement((SkillNames)0x11F10A98);
            }
            actor.SkillManager.AddSkillPoints((SkillNames)0x11F10A98, 35f);
        }
    }
}