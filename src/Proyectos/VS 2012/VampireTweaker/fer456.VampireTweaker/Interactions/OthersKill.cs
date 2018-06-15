using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;

namespace VampireTweaker.Interactions.Others
{
    public class OthersKill
	{
		public static Sim player1;
		public static Sim player2;
		public static int misery = -75;
		public static int delight = 75;
		public static int adreanalinerush = 100;
		public static float miseryTimeout = 4320f;
		public static float delightTimeout = 2880f;
		public static float steamTimeout = 1440f;
		public static float adrenalineTimeOut = 1440f;
        public static void VampVictimDeath(Sim vamp, Sim vic)
		{
			OthersKill.player1 = vamp;
			OthersKill.player2 = vic;
			OthersKill.player2.Kill(SimDescription.DeathType.Starve, null, false);
            OthersKill.player1.BuffManager.AddElement(0xF210BB43E0C33B3F, (Origin)ResourceUtils.HashString64("ByKillingASim"));
            OthersKill.player2.BuffManager.AddElement(0xBCF83A2A06FD58B0, (Origin)ResourceUtils.HashString64("ByBeingKilledByTheVampire"));
		}
        public static void VampireDrinkSuccess(Sim actor, InteractionInstance i)
		{
			if (actor.Motives.IsFullEnoughForStuffedBuff())
			{
				actor.BuffManager.AddElement(BuffNames.Stuffed, Origin.FromCarnivorousBehavior);
                actor.BuffManager.AddElement(0xF210BB43E0C33B3F, (Origin)ResourceUtils.HashString64("ByKillingASim"));
                actor.BuffManager.AddElement(0xD224DD43B0C28B1F, (Origin)ResourceUtils.HashString64("ForBitingASim"));
			}
			OthersKill.setMaxMotive(actor, CommodityKind.VampireThirst);			
			if (actor.TraitManager.HasElement(TraitNames.Vegetarian))
			{
				actor.BuffManager.AddElement(BuffNames.Nauseous, Origin.FromCarnivorousBehavior);                
			}
			else
			{
				actor.BuffManager.AddElement(BuffNames.Sated, Origin.FromReceivingVampireNutrients);
			}            
            {
                actor.BuffManager.AddElement(0xF210BB43E0C33B3F, (Origin)ResourceUtils.HashString64("ByKillingASim"));
                actor.BuffManager.AddElement(0xD224DD43B0C28B1F, (Origin)ResourceUtils.HashString64("ForBitingASim"));
		}
        }
        public static bool childVersion(Sim target)
		{
			return !target.SimDescription.TeenOrBelow && !target.SimDescription.IsPregnant;
		}
        public static void SetDeathMoodlets()
		{
			if (OthersKill.player1.TraitManager.HasElement(TraitNames.Good) || OthersKill.player1.TraitManager.HasElement(TraitNames.Friendly))
			{
				OthersKill.player1.BuffManager.AddElement(BuffNames.CreepedOut, OthersKill.misery, OthersKill.miseryTimeout, Origin.FromWitnessingDeath);
				OthersKill.player1.BuffManager.AddElement(BuffNames.Horrified, OthersKill.misery, OthersKill.miseryTimeout, Origin.FromWatchingSimSuffer);
				OthersKill.player1.BuffManager.AddElement(BuffNames.AdrenalineRush, OthersKill.adreanalinerush, OthersKill.adrenalineTimeOut, Origin.FromGrimReaper);
                OthersKill.player1.BuffManager.AddElement(0xF210BB43E0C33B3F, (Origin)ResourceUtils.HashString64("ByKillingASim"));
                OthersKill.player1.BuffManager.AddElement(0xD224DD43B0C28B1F, (Origin)ResourceUtils.HashString64("ForBitingASim"));
			}
			else
			{
				if (OthersKill.player2.IsBloodRelated(OthersKill.player1) && OthersKill.player1.TraitManager.HasElement(TraitNames.FamilyOriented))
				{
					OthersKill.player1.BuffManager.AddElement(BuffNames.CreepedOut, OthersKill.misery, OthersKill.miseryTimeout, Origin.FromWitnessingDeath);
					OthersKill.player1.BuffManager.AddElement(BuffNames.Horrified, OthersKill.misery, OthersKill.miseryTimeout, Origin.FromWatchingSimSuffer);
					OthersKill.player1.BuffManager.AddElement(BuffNames.AdrenalineRush, OthersKill.adreanalinerush, OthersKill.adrenalineTimeOut, Origin.FromGrimReaper);
                    OthersKill.player1.BuffManager.AddElement(0xF210BB43E0C33B3F, (Origin)ResourceUtils.HashString64("ByKillingASim"));
                    OthersKill.player1.BuffManager.AddElement(0xD224DD43B0C28B1F, (Origin)ResourceUtils.HashString64("ForBitingASim"));
				}
				else
				{
					OthersKill.player1.BuffManager.AddElement(BuffNames.LetOffSteam, OthersKill.delight, OthersKill.steamTimeout, Origin.FromWitnessingDeath);
					OthersKill.player1.BuffManager.AddElement(BuffNames.AdrenalineRush, OthersKill.adreanalinerush, OthersKill.adrenalineTimeOut, Origin.FromGrimReaper);
					OthersKill.player1.BuffManager.AddElement(BuffNames.FiendishlyDelighted, OthersKill.delight, OthersKill.delightTimeout, Origin.FromWatchingSimSuffer);
                    OthersKill.player1.BuffManager.AddElement(0xF210BB43E0C33B3F, (Origin)ResourceUtils.HashString64("ByKillingASim"));
                    OthersKill.player1.BuffManager.AddElement(0xD224DD43B0C28B1F, (Origin)ResourceUtils.HashString64("ForBitingASim"));
				}
			}
			foreach (Sim current in OthersKill.player1.LotCurrent.GetSims())
			{
				if (current != OthersKill.player1 && current.RoomId == OthersKill.player1.RoomId && current != OthersKill.player2)
				{
					Relationship relationship = Relationship.Get(OthersKill.player1, current, true);
					float liking = relationship.LTR.Liking;
					if (current.TraitManager.HasElement(TraitNames.Evil) || current.TraitManager.HasElement(TraitNames.MeanSpirited))
					{
						current.BuffManager.AddElement(BuffNames.FiendishlyDelighted, OthersKill.delight, OthersKill.steamTimeout, Origin.FromWatchingSimSuffer);
						current.BuffManager.AddElement(BuffNames.Intrigued, OthersKill.delight / 4, OthersKill.steamTimeout, Origin.FromWitnessingDeath);                        
						relationship.LTR.SetLiking(liking + 20f);
					}
					else
					{
						if (current.SimDescription.TeenOrAbove)
						{
							OthersKill.player1.SocialComponent.SetHasTreatedKidsPoorly();
						}
						current.BuffManager.AddElement(BuffNames.Scared, OthersKill.misery, OthersKill.miseryTimeout, Origin.FromWitnessingDeath);
						current.BuffManager.AddElement(BuffNames.Horrified, OthersKill.misery, OthersKill.miseryTimeout, Origin.FromWatchingSimSuffer);
						current.BuffManager.AddElement(BuffNames.CreepedOut, OthersKill.misery, OthersKill.miseryTimeout, Origin.FromWitnessingDeath);                        
						relationship.LTR.SetLiking(liking - 100f);
					}
				}
			}
		}
        public static void setMaxMotive(Sim actor, CommodityKind type)
        {
            actor.Motives.SetMax(type);
        }
        public static void setSkillPoints(Sim actor)
        {
            if (!actor.SkillManager.HasElement(0x11F10A98))
            {
                actor.SkillManager.AddElement((SkillNames)0x11F10A98);
            }
            actor.SkillManager.AddSkillPoints((SkillNames)0x11F10A98, 25f);
        }
    }
}
