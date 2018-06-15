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
    public class VTConversion
    {
        public static bool bathingCleans = false;
        public static CASPart conversionMask = new CASPart();
        [PersistableStatic]
        public static Dictionary<Sim, bool> convertedSims = new Dictionary<Sim, bool>();
        public static Sim lastTriggered = null;
        [Tunable]
        protected static string maskGroupID = "";
        [Tunable]
        protected static string maskInstanceID = "";
        public static ResourceKey VTConversionMaskPart = ResourceKey.kInvalidResourceKey;
        [Tunable]
        protected static string maskPresetIndex = "";
        [Tunable]
        protected static string maskTypeID = "";
        public static EventListener sConversionBathListener;
        public static EventListener sConversionBuffListener;
        public static bool showeringCleans = false;
        public static EventListener sConversionShowerListener;
        [Tunable]
        protected static bool timeCleans = false;

        static VTConversion()
        {
            VTConversion.sConversionBuffListener = null;
            VTConversion.sConversionBathListener = null;
            VTConversion.sConversionShowerListener = null;
            World.OnWorldQuitEventHandler += new EventHandler(VTConversion.World_OnWorldQuitEventHandler);
        }

        public static int cleanBodys(object obj)
        {
            try
            {
                foreach (KeyValuePair<Sim, bool> pair in convertedSims)
                {
                    if (convertedSims[pair.Key])
                    {
                        Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTConversion.runClean), pair.Key));
                    }
                }
            }
            catch (Exception exception)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "An error is ocurred when uninstalling. Check notification bar");
                StyledNotification.Show(new StyledNotification.Format("Failed the removal of the sim dictionary (convertedSims) \n." + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
            return 1;
        }
        public static ListenerAction OnClean(Event e)
        {
            Sim actor = e.Actor as Sim;
            if (actor != null)
            {
                Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTConversion.RemoveMakeup), actor));
            }
            return ListenerAction.Keep;
        }

        public static ListenerAction OnGotBuff(Event e)
        {
            Sim actor = e.Actor as Sim;
            if (actor != null)
            {
                if (actor.SimDescription.IsVampire || (actor.SimDescription.IsHuman))
                {
                    if ((lastTriggered == null) || (lastTriggered != actor))
                    {
                        Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTConversion.ProcessBuff), actor));
                    }
                }
                else if (convertedSims.ContainsKey(actor))
                {
                    Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTConversion.RemoveMakeup), actor));
                    convertedSims.Remove(actor);
                }
            }
            return ListenerAction.Keep;
        }

        public static void OnWorldLoadFinishedHandler()
        {
            VTConversionMaskPart = ResourceKey.Parse(maskTypeID + "-" + maskGroupID + "-" + maskInstanceID);
            sConversionBuffListener = EventTracker.AddListener(EventTypeId.kGotBuff, new ProcessEventDelegate(VTConversion.OnGotBuff));
            if (bathingCleans)
            {
                sConversionBathListener = EventTracker.AddListener(EventTypeId.kEventTakeBath, new ProcessEventDelegate(VTConversion.OnClean));
            }
            if (showeringCleans)
            {
                sConversionShowerListener = EventTracker.AddListener(EventTypeId.kEventTakeShower, new ProcessEventDelegate(VTConversion.OnClean));
            }
            PartSearch search = new PartSearch();
            foreach (CASPart part in search)
            {
                if (part.Key == VTConversionMaskPart)
                {
                    conversionMask = part;
                }
            }
            search.Reset();
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
                    if ((current.BuffGuid == (0xF210BB43E0C33B3F)) || (current.BuffGuid == (0xB040AC12800F6F7D)) || (current.BuffGuid == (0x7A197492362B6540) || (current.BuffGuid == (0xBCF83A2A06FD58B0))))
                    {
                        flag = true;
                        if (current.mStartTime > (SimClock.CurrentTicks - 0x2dL))
                        {
                            SetMakeup(sim);
                            return;
                        }
                    }
                }
                if ((!flag && convertedSims.ContainsKey(sim)) && (timeCleans))
                {
                    if (convertedSims[sim])
                    {
                        RemoveMakeup(sim);
                    }
                }
            }
        }

        public static void RemoveMakeup(object obj)
        {
            Sim key = obj as Sim;
            if ((key.SimDescription.IsVampire && key.SimDescription.IsHuman && convertedSims.ContainsKey(key)) && convertedSims[key])
            {
                RemoveMakeup(key);
            }
        }
        public static void RemoveMakeupFromToggle()
        {
            List<Sim> list = new List<Sim>(Sims3.Gameplay.Queries.GetObjects<Sim>());
            foreach (Sim sim in list)
            {
                if ((sim.SimDescription.IsVampire && convertedSims.ContainsKey(sim)) && convertedSims[sim])
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
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Cleaning bloody body of: " + sim.FullName + "...");
                    RemoveMakeup(sim);
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Cleaned bloody body of: " + sim.FullName + "!");
                }
                catch(Exception exception)
                {
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Failed the removal process of the conversion makeup of: " + sim.FullName + "!" + "\nCheck notification bar");
                    StyledNotification.Show(new StyledNotification.Format("Failed the uninstall process in the removal of the conversion makeup of: " + sim.FullName + "!\nTechinical info to get more information about the error: \n" + exception, StyledNotification.NotificationStyle.kDebugAlert));
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
            if (!convertedSims.ContainsKey(sim))
            {
                convertedSims.Add(sim, true);
            }
            else
            {
                convertedSims[sim] = true;
            }
            lastTriggered = null;
        }
        public static void RemoveMakeup(Sim sim)
        {
            lastTriggered = sim;
            MakeupManagement(sim, false);
            if (!convertedSims.ContainsKey(sim))
            {
                convertedSims.Remove(sim);
            }
            else
            {
                convertedSims[sim] = false;
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
                    sendDebugMsg(sim.FullName + "\nAdding blood.");
                    string designPreset = CASUtils.PartDataGetPreset(VTConversionMaskPart, uint.Parse(maskPresetIndex));                   
                    builder.AddPart(conversionMask);
                    CASUtils.ApplyPresetToPart(builder, conversionMask, designPreset);
                    builder.SetPartPreset(VTConversionMaskPart, uint.Parse(maskPresetIndex), designPreset);
                }
                else
                {
                    builder.RemovePart(conversionMask);                    
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
                                        string str2 = CASUtils.PartDataGetPreset(VTConversionMaskPart, uint.Parse(maskPresetIndex));
                                        builder.AddPart(conversionMask);                                        
                                        CASUtils.ApplyPresetToPart(builder, conversionMask, str2);
                                        builder.SetPartPreset(VTConversionMaskPart, uint.Parse(maskPresetIndex), str2);
                                    }
                                    else
                                    {
                                        builder.RemovePart(conversionMask);                                        
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
                SimpleMessageDialog.Show("Vampire Tweaker - Conversion Effect", sim.Name + "\nApply makeup failed!\n" + exception);
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
                convertedSims = null;
            }
        }
    }
}