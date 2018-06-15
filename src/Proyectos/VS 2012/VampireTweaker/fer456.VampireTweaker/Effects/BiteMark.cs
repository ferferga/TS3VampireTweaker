using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using VampireTweaker.ModInitiatorAndHandler;
namespace VampireTweaker.VampireEffects
{
    public class VTBiteMark
    {
        [Tunable]
        public static CASPart biteMask = new CASPart();
        [PersistableStatic]
        public static Dictionary<Sim, bool> bitedSims = new Dictionary<Sim, bool>();
        public static Sim lastTriggered = null;
        [Tunable]
        protected static string makeupGroupID = "";
        [Tunable]
        protected static string makeupInstanceID = "";
        public static ResourceKey VTBiteMaskPart = ResourceKey.kInvalidResourceKey;
        [Tunable]
        protected static string makeupPresetIndex = "";
        [Tunable]
        protected static string makeupTypeID = "";
        public static EventListener sBiteMarkBuffListener;
        [Tunable]
        protected static bool AutoTimeClean = false;

        static VTBiteMark()
        {
            VTBiteMark.sBiteMarkBuffListener = null;
            World.OnWorldQuitEventHandler += new EventHandler(VTBiteMark.World_OnWorldQuitEventHandler);
        }

        public static int cleanHumanNecks(object obj)
        {
            try
            {
                foreach (KeyValuePair<Sim, bool> pair in bitedSims)
                {
                    if (bitedSims[pair.Key])
                    {
                        Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTBiteMark.runClean), pair.Key));
                    }
                }
            }
            catch (Exception exception)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "An error is ocurred when uninstalling. Check notification bar");
                StyledNotification.Show(new StyledNotification.Format("Failed the removal of the sim dictionary (bitedSims) \n." + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
            return 1;
        }
        public static ListenerAction OnClean(Event e)
        {
            Sim target = e.Actor as Sim;
            if (target != null)
            {
                Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTBiteMark.RemoveMakeup), target));
            }
            return ListenerAction.Keep;
        }

        public static ListenerAction OnGotBuff(Event e)
        {
            Sim actor = e.Actor as Sim;
            if (actor != null)
            {
                if (actor.SimDescription.IsHuman || (actor.SimDescription.IsSupernaturalForm))
                {
                    if ((lastTriggered == null) || (lastTriggered != actor))
                    {
                        Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTBiteMark.ProcessBuff), actor));
                    }
                }
                else if (bitedSims.ContainsKey(actor))
                {
                    Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTBiteMark.RemoveMakeup), actor));
                }
            }
            return ListenerAction.Keep;
        }

        public static void OnWorldLoadFinishedHandler()
        {
            VTBiteMaskPart = ResourceKey.Parse(makeupTypeID + "-" + makeupGroupID + "-" + makeupInstanceID);
            sBiteMarkBuffListener = EventTracker.AddListener(EventTypeId.kGotBuff, new ProcessEventDelegate(VTBiteMark.OnGotBuff));
            {
                PartSearch search = new PartSearch();
                foreach (CASPart part in search)
                {
                    if (part.Key == VTBiteMaskPart)
                    {
                        biteMask = part;
                    }
                }
                search.Reset();
            }
        }
        public static void ProcessBuff(object obj)
        {
            Sim sim = obj as Sim;
            bool flag = false;
            using (IEnumerator<BuffInstance> enumerator = sim.BuffManager.Buffs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BuffInstance current = enumerator.Current;
                    if ((current.mBuffGuid == BuffNames.Weakened) || (current.mBuffGuid == BuffNames.VampireBite))
                    {
                        flag = true;
                        if (current.mStartTime > (SimClock.CurrentTicks - 0x2dL))
                        {
                            SetMakeup(sim);
                            return;
                        }
                    }
                }
                if ((!flag && bitedSims.ContainsKey(sim)) && (AutoTimeClean))
                {
                    if (bitedSims[sim])
                    {
                        RemoveMakeup(sim);
                    }
                }
            }
        }

        public static void RemoveMakeup(object obj)
        {
            Sim key = obj as Sim;
            if ((key.SimDescription.IsHuman && key.SimDescription.IsSupernaturalForm && bitedSims.ContainsKey(key)) && bitedSims[key])
            {
                RemoveMakeup(key);
            }
        }
        public static void RemoveMakeupFromToggle()
        {
            List<Sim> list = new List<Sim>(Sims3.Gameplay.Queries.GetObjects<Sim>());
            foreach (Sim sim in list)
            {
                if ((sim.SimDescription.IsVampire && bitedSims.ContainsKey(sim)) && bitedSims[sim])
                {
                    RemoveMakeup(sim);
                }
            }
        }

        public static void runClean(object obj)
        {
            Sim sim = obj as Sim;
            if (sim != null)
                try
                {
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Cleaning bite mark of: " + sim.FullName + "...");
                    RemoveMakeup(sim);
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Cleaned bite mark of: " + sim.FullName + "!");
                }
                catch (Exception exception)
                {
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Failed the removal process of the bite mark makeup of: " + sim.FullName + "!" + "\nCheck Notification bar");
                    StyledNotification.Show(new StyledNotification.Format("Failed the uninstall process in the removal of the bite mark makeup of: " + sim.FullName + "!\nTechinical info to get more information about the error: \n" + exception, StyledNotification.NotificationStyle.kDebugAlert));
                }
        }

        public static void sendDebugMsg(string msg)
        {
            if (Instantiator.pDebugOn)
            {
                StyledNotification.Show(new StyledNotification.Format("Vampire Tweaker - Debug Message" + msg, StyledNotification.NotificationStyle.kDebugAlert));
            }
        }
        public static void SetMakeup(Sim sim)
        {
            lastTriggered = sim;
            MakeupManagement(sim, true);
            if (!bitedSims.ContainsKey(sim))
            {
                bitedSims.Add(sim, true);
            }
            else
            {
                bitedSims[sim] = true;
            }
            lastTriggered = null;
        }
        public static void RemoveMakeup(Sim sim)
        {
            lastTriggered = sim;
            MakeupManagement(sim, false);
            if (!bitedSims.ContainsKey(sim))
            {
                bitedSims.Remove(sim);
            }
            else
            {
                bitedSims[sim] = false;
            }
            lastTriggered = null;
        }
        public static void MakeupManagement(Sim sim, bool addRemove)
        {
            try
            {
                SimBuilder builder = new SimBuilder();
                SimDescription simDescription = sim.SimDescription;
                OutfitCategories currentOutfitCategory = sim.CurrentOutfitCategory;
                int currentOutfitIndex = sim.CurrentOutfitIndex;
                builder.Clear(false);
                SimOutfit currentOutfit = sim.CurrentOutfit;
                OutfitUtils.SetOutfit(builder, currentOutfit, simDescription);
                if (addRemove)
                {
                    sendDebugMsg(sim.FullName + "\nAdding bite mark.");
                    string designPreset = CASUtils.PartDataGetPreset(VTBiteMaskPart, uint.Parse(makeupPresetIndex));
                    builder.AddPart(biteMask);
                    sim.BuffManager.AddElement(0xE0CDC26A9F9D3B74, (Origin)ResourceUtils.HashString64("ByGiveBloodToAVampire"));
                    CASUtils.ApplyPresetToPart(builder, biteMask, designPreset);
                    builder.SetPartPreset(VTBiteMaskPart, uint.Parse(makeupPresetIndex), designPreset);
                }
                else
                {
                    builder.RemovePart(biteMask);
                }
                SimOutfit outfit = new SimOutfit(builder.CacheOutfit(simDescription.FullName + currentOutfitCategory.ToString() + currentOutfitIndex.ToString()));
                if (simDescription.GetOutfitCount(currentOutfitCategory) > currentOutfitIndex)
                {
                    simDescription.RemoveOutfit(currentOutfitCategory, currentOutfitIndex, true);
                }
                simDescription.AddOutfit(outfit, currentOutfitCategory, currentOutfitIndex);
                if (simDescription.CreatedSim != null)
                {
                    sendDebugMsg("Updated: " + currentOutfitCategory.ToString() + "-" + currentOutfitIndex.ToString());
                    simDescription.CreatedSim.RefreshCurrentOutfit(false);
                }
                foreach (OutfitCategories categories2 in Enum.GetValues(typeof(OutfitCategories)))
                {
                    if (categories2 != OutfitCategories.Special)
                    {
                        ArrayList list = simDescription.GetCurrentOutfits()[categories2] as ArrayList;
                        if (list != null)
                        {
                            int count = list.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if ((categories2 != currentOutfitCategory) || (i != currentOutfitIndex))
                                {
                                    builder.Clear(false);
                                    SimOutfit outfit3 = list[i] as SimOutfit;
                                    OutfitUtils.SetOutfit(builder, outfit3, simDescription);
                                    if (addRemove)
                                    {
                                        string str2 = CASUtils.PartDataGetPreset(VTBiteMaskPart, uint.Parse(makeupPresetIndex));
                                        builder.AddPart(biteMask);
                                        sim.BuffManager.AddElement(0xE0CDC26A9F9D3B74, (Origin)ResourceUtils.HashString64("ByGiveBloodToAVampire"));
                                        CASUtils.ApplyPresetToPart(builder, biteMask, str2);
                                        builder.SetPartPreset(VTBiteMaskPart, uint.Parse(makeupPresetIndex), str2);
                                    }
                                    else
                                    {
                                        builder.RemovePart(biteMask);
                                    }
                                    SimOutfit outfit4 = new SimOutfit(builder.CacheOutfit(simDescription.FullName + categories2.ToString() + i.ToString()));
                                    if (simDescription.GetOutfitCount(categories2) > i)
                                    {
                                        simDescription.RemoveOutfit(categories2, i, true);
                                    }
                                    simDescription.AddOutfit(outfit4, categories2, i);
                                    sendDebugMsg("Updated: " + categories2.ToString() + "-" + i.ToString());
                                    Sleep(0);
                                }
                            }
                        }
                    }
                }
                SimOutfit outfit5 = simDescription.GetOutfit(OutfitCategories.Everyday, 0);
                if (outfit5 != null)
                {
                    ThumbnailManager.GenerateHouseholdSimThumbnail(outfit5.Key, outfit5.Key.InstanceId, 0, ThumbnailSizeMask.Large | ThumbnailSizeMask.ExtraLarge | ThumbnailSizeMask.Medium | ThumbnailSizeMask.Small, ThumbnailTechnique.Default, true, false, simDescription.AgeGenderSpecies);
                }
            }
            catch (Exception exception)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Bite Mark", sim.Name + "\nApply makeup failed!\n" + exception);
            }
        }

        public static bool Sleep(uint value)
        {
            try
            {
                Simulator.Sleep(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void World_OnWorldQuitEventHandler(object sender, EventArgs e)
        {
            if (PersistStatic.MainMenuLoading)
            {
                bitedSims = null;
            }
        }
    }
}