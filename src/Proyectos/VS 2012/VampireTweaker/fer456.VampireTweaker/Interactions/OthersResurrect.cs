using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Objects;
using Sims3.SimIFace;
using Sims3.UI.Hud;
using Sims3.SimIFace.CAS;

namespace VampireTweaker.Interactions.Others
{
    internal class OthersResurrect
    {
        public static Sim player1;
        public static Sim player2;

        public static bool CanDrink(uint vamp, uint vic)
        {
            return vamp > vic;
        }
        public static bool childVersion(Sim target)
        {
            return !target.SimDescription.TeenOrBelow && !target.SimDescription.IsPregnant;
        }
        public static void SetResurrectMoodlets()
        {
            if (OthersResurrect.player1.TraitManager.HasElement(TraitNames.Good) || OthersResurrect.player1.TraitManager.HasElement(TraitNames.Friendly))
            {
                OthersResurrect.player1.BuffManager.AddElement(0xCDEDE943B0C64B1F, (Origin)ResourceUtils.HashString64("ByRevivingASim"));
            }
            else
            {
                OthersResurrect.player1.BuffManager.AddElement(0xCDEDE943B0C64B1F, (Origin)ResourceUtils.HashString64("ByRevivingASim"));
            }
            foreach (Sim current in OthersResurrect.player1.LotCurrent.GetSims())
            {
                if (current != OthersResurrect.player1 && current.RoomId == OthersResurrect.player1.RoomId && current != OthersResurrect.player2)
                {
                    Relationship relationship = Relationship.Get(OthersResurrect.player1, current, true);
                    float liking = relationship.LTR.Liking;
                    if (current.TraitManager.HasElement(TraitNames.Evil) || current.TraitManager.HasElement(TraitNames.MeanSpirited))
                    {
                        relationship.LTR.SetLiking(liking - 20f);
                    }
                    else
                    {
                        if (current.SimDescription.TeenOrAbove)
                        {
                            OthersResurrect.player1.SocialComponent.SetHasTreatedKidsPoorly();
                        }
                        relationship.LTR.SetLiking(liking + 100f);
                    }
                }
            }
        }

        public static void SetResurrectMoodletsToUrnstone()
        {
            OthersResurrect.player1.BuffManager.AddElement(0xCDEDE943B0C64B1F, (Origin)ResourceUtils.HashString64("ByRevivingASim"));
        }
        public static void PlayResurrectAnimation(Sim target, bool bPlayVTResurrectAnim, bool fadeSim)
        {
            string effectName = null;
            if (bPlayVTResurrectAnim)
            {
                effectName = "ep3VampireResurrect_test";
                VisualEffect visualEffect = VisualEffect.Create(effectName);
                visualEffect.SetPosAndOrient(target.Position, target.ForwardVector, target.UpVector);
                visualEffect.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
                if (target.SimDescription.AdultOrAbove)
                {
                    target.PlaySoloAnimation("a_floatingSimInResurrect_x", false);
                }
                if (fadeSim)
                {
                    target.FadeOut(true, false);
                }
            }
        }
        public static void AppearSimInResurrect(Sim target, bool fadeSim)
        {
            if (fadeSim)
            {
                target.PlaySoloAnimation("a_floatingSimInResurrect_x", false);
                target.FadeIn(true);
            }
        }
        public static void setSkillPoints(Sim actor)
        {
            if (!actor.SkillManager.HasElement(0x11F10A98))
            {
                actor.SkillManager.AddElement((SkillNames)0x11F10A98);
            }
            actor.SkillManager.AddSkillPoints((SkillNames)0x11F10A98, 45f);
        }
        public static void GhostToSim(Sim vamp, Sim vic)
        {
            OthersResurrect.player1 = vamp;
            OthersResurrect.player2 = vic;
            Sim actor = player1;
            Sim target = player2;
            Urnstone urnstone = Urnstone.FindGhostsGrave(target);

            if (urnstone != null)
            {
                OthersResurrect.PlayResurrectAnimation(target, true, true);
                urnstone.GhostToSim(target, true, true);
                target.OccultManager.AddOccultType(OccultTypes.Vampire, true, false, false);
                target.ChooseRandomOutfitCategoryOtherThanCurrent(OutfitCategories.Everyday);
                OthersResurrect.AppearSimInResurrect(target, true);
            }
        }
        protected bool OnPerform(Sim actor)
        {
            Urnstone urnstone = Urnstone.FindGhostsGrave(actor);
            SimDescription deadSimsDescription = urnstone.DeadSimsDescription;
            deadSimsDescription.IsGhost = false;
            deadSimsDescription.IsNeverSelectable = false;
            deadSimsDescription.ShowSocialsOnSim = true;
            Vector3 vector2 = actor.Position;
            vector2.x++;
            if (deadSimsDescription.CreatedSim != null)
            {
                urnstone.GhostToSim(deadSimsDescription.CreatedSim, false, true);
            }
            else
            {
                urnstone.OriginalHousehold.Add(deadSimsDescription);
                deadSimsDescription.Instantiate(vector2);
                deadSimsDescription.AgingEnabled = false;
            }
            if (urnstone.DeadSimsDescription.CreatedSim != null)
            {
                urnstone.Destroy();
            }

            return true;
        }
    }
}