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
    public class VTThirst
    {
        [Tunable]
        public static CASPart ThirstMask = new CASPart();
        [PersistableStatic]
        public static Dictionary<Sim, bool> thirstySims = new Dictionary<Sim, bool>();
        [Tunable]
        public static bool debugOn = false;
        public static Sim lastTriggered = null;
        [Tunable]
        protected static string kMaskGroupID = "";
        [Tunable]
        protected static string kMaskInstanceID = "";
        public static ResourceKey VTThirstMaskPart = ResourceKey.kInvalidResourceKey;
        [Tunable]
        protected static string kMaskPresetIndex = "";
        [Tunable]
        protected static string kMaskTypeID = "";
        public static EventListener sThirstBuffListener;                
        public static EventListener sSimAgedUp = null;
        public static EventListener sSimInstantiated = null;

        // Methods
        static VTThirst()
        {
            VTThirst.sThirstBuffListener = null;
            World.OnWorldQuitEventHandler += new EventHandler(VTThirst.World_OnWorldQuitEventHandler);
        }

        public static int cleanVampFaces(object obj)
        {
            try
            {
                foreach (KeyValuePair<Sim, bool> pair in thirstySims)
                {
                    if (thirstySims[pair.Key])
                    {
                        Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTThirst.runClean), pair.Key));
                    }
                }
            }
            catch (Exception exception)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "An error is ocurred when uninstalling. Check notification bar");
                StyledNotification.Show(new StyledNotification.Format("Failed the removal of the sim dictionary (thirstySims) \n" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
            return 1;
        }
        public static ListenerAction OnClean(Event e)
        {
            Sim actor = e.Actor as Sim;
            if (actor != null)
            {
                Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTThirst.RemoveMakeup), actor));
            }
            return ListenerAction.Keep;
        }

        public static ListenerAction OnGotBuff(Event e)
        {
            Sim actor = e.Actor as Sim;
            if (actor != null)
            {
                if (actor.SimDescription.IsVampire)
                {
                    if ((lastTriggered == null) || (lastTriggered != actor))
                    {
                        Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTThirst.ProcessBuff), actor));
                    }
                }
                else if (thirstySims.ContainsKey(actor))
                {
                    Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTThirst.RemoveMakeup), actor));
                }
            }
            return ListenerAction.Keep;
        }

        public static void OnWorldLoadFinishedHandler()
        {
            VTThirstMaskPart = ResourceKey.Parse(kMaskTypeID + "-" + kMaskGroupID + "-" + kMaskInstanceID);
            sThirstBuffListener = EventTracker.AddListener(EventTypeId.kGotBuff, new ProcessEventDelegate(VTThirst.OnGotBuff));
            AlarmManager.Global.AddAlarm(40f, TimeUnit.Seconds, new AlarmTimerCallback(VTThirst.AddChildrenEyes), "Add Children Eyes Procces", AlarmType.NeverPersisted, null);
            {
                PartSearch search = new PartSearch();
                foreach (CASPart part in search)
                {
                    if (part.Key == VTThirstMaskPart)
                    {
                        ThirstMask = part;
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
                    if ((current.mBuffGuid == BuffNames.Thirsty) || (current.mBuffGuid == BuffNames.VeryThirsty) || (current.mBuffGuid == BuffNames.MadlyThirsty))
                    {
                        flag = true;
                        if (current.mStartTime > (SimClock.CurrentTicks - 0x2dL))
                        {
                            SetMakeup(sim);
                            return;
                        }
                        if ((current.mBuffGuid == BuffNames.Sated) || (current.mBuffGuid == BuffNames.SanguineSnack) || (current.mBuffGuid == BuffNames.DrankFromAFairy))
                        {
                            flag = true;
                            if (current.mStartTime > (SimClock.CurrentTicks - 0x2dL))
                            {
                                RemoveMakeup(sim);                                
                                return;
                            }
                        }
                    }
                    if ((!flag && thirstySims.ContainsKey(sim)))
                    {
                        if (thirstySims[sim])
                        {
                            RemoveMakeup(sim);
                        }
                    }
                }
            }
        }

        public static void RemoveMakeup(object obj)
        {
            Sim key = obj as Sim;
            if ((key.SimDescription.IsVampire && thirstySims.ContainsKey(key)) && thirstySims[key])
            {
                RemoveMakeup(key);
            }
        }
        public static void RemoveMakeupFromToggle()
        {
            List<Sim> list = new List<Sim>(Sims3.Gameplay.Queries.GetObjects<Sim>());
            foreach (Sim sim in list)
            {
                if ((sim.SimDescription.IsVampire && thirstySims.ContainsKey(sim)) && thirstySims[sim])
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
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Cleaning red eyes of: " + sim.FullName + "...");
                    RemoveMakeup(sim);
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Cleaned red eyes of: " + sim.FullName + "!");
                }
                catch (Exception exception)
                {
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Failed the removal process of the thirst makeup of: " + sim.FullName + "!" + "\nCheck Notification bar");
                    StyledNotification.Show(new StyledNotification.Format("Failed the uninstall process in the removal of the thirst makeup of: " + sim.FullName + "!\nTechinical info to get more information about the error: \n" + exception, StyledNotification.NotificationStyle.kDebugAlert));
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
            if (!thirstySims.ContainsKey(sim))
            {
                thirstySims.Add(sim, true);
            }
            else
            {
                thirstySims[sim] = true;
            }
            lastTriggered = null;
        }
        public static void RemoveMakeup(Sim sim)
        {
            lastTriggered = sim;
            MakeupManagement(sim, false);
            if (!thirstySims.ContainsKey(sim))
            {
                thirstySims.Remove(sim);
            }
            else
            {
                thirstySims[sim] = false;
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
                    sendDebugMsg(sim.FullName + "\nAdding eyes.");
                    string designPreset = CASUtils.PartDataGetPreset(VTThirstMaskPart, uint.Parse(kMaskPresetIndex));
                    builder.AddPart(ThirstMask);
                    if (sim.SimDescription.YoungAdultOrAbove)
                    {
                        sim.BuffManager.AddElement(0xB110CC43B0C44B1C, (Origin)ResourceUtils.HashString64("ByAppearanceOfRedEyes"));
                    }
                    CASUtils.ApplyPresetToPart(builder, ThirstMask, designPreset);
                    builder.SetPartPreset(VTThirstMaskPart, uint.Parse(kMaskPresetIndex), designPreset);
                }
                else
                {
                    builder.RemovePart(ThirstMask);
                    sim.BuffManager.RemoveElement(0xB110CC43B0C44B1C);
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
                                        string str2 = CASUtils.PartDataGetPreset(VTThirstMaskPart, uint.Parse(kMaskPresetIndex));
                                        builder.AddPart(ThirstMask);
                                        sim.BuffManager.AddElement(0xB110CC43B0C44B1C, (Origin)ResourceUtils.HashString64("ByAppearanceOfRedEyes"));
                                        CASUtils.ApplyPresetToPart(builder, ThirstMask, str2);
                                        builder.SetPartPreset(VTThirstMaskPart, uint.Parse(kMaskPresetIndex), str2);
                                    }
                                    else
                                    {
                                        builder.RemovePart(ThirstMask);
                                        sim.BuffManager.RemoveElement(0xB110CC43B0C44B1C);
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
                SimpleMessageDialog.Show("Vampire Tweaker - Thirst Effect", sim.Name + "\nApply makeup failed!\n" + exception);
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
                thirstySims = null;
            }
        }
        public static void AddChildrenEyes()
        {
            List<Sim> list = new List<Sim>(Sims3.Gameplay.Queries.GetObjects<Sim>());
            foreach (Sim vampire in list)
            { 
                if (vampire != null)
                {
                    VTThirst.AddEyesToChildren(vampire);
                }
            }
            sSimAgedUp = EventTracker.AddListener(EventTypeId.kSimAgeTransition, new ProcessEventDelegate(VTThirst.AddEyesWhenAgedUp));
            sSimInstantiated = EventTracker.AddListener(EventTypeId.kSimInstantiated, new ProcessEventDelegate(VTThirst.AddEyesWhenAgedUp));
        }
        private static void AddEyesToChildren(Sim child)
        {
            if (child.SimDescription.IsVampire && child.SimDescription.ChildOrAbove && !child.SimDescription.YoungAdultOrAbove && child.SimDescription.TeenOrBelow)
            {
                VTThirst.SetMakeup(child);
            }
        }
        protected static ListenerAction AddEyesWhenAgedUp(Event e)
        {
            Sim sim = e.TargetObject as Sim;
            if (sim != null)
            {
                AddEyesToChildren(sim);
            }
            return ListenerAction.Keep;
        }
    }
}