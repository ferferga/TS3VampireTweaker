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
    public class VTBlood
    {
        public static bool bathingCleans = false;
        private static CASPart VTBloodMask = new CASPart();
        [PersistableStatic]
        public static Dictionary<Sim, bool> bloodySims = new Dictionary<Sim, bool>();
        [Tunable]
        protected static bool brushingCleans = false;        
        private static Sim lastTriggered = null;
        [Tunable]
        protected static string maskGroupID = "";
        [Tunable]
        protected static string maskInstanceID = "";
        private static ResourceKey maskPart = ResourceKey.kInvalidResourceKey;
        [Tunable]
        protected static string maskPresetIndex = "";
        [Tunable]
        protected static string maskTypeID = "";
        public static EventListener sBathListener;
        public static EventListener sBrushListener;
        public static EventListener sBloodBuffListener;
        public static bool showeringCleans = false;
        public static EventListener sShowerListener;
        [Tunable]
        protected static bool timeCleans = false;

        static VTBlood()
        {
            VTBlood.sBloodBuffListener = null;
            VTBlood.sBathListener = null;
            VTBlood.sBrushListener = null;
            VTBlood.sShowerListener = null;            
            World.OnWorldQuitEventHandler += new EventHandler(VTBlood.World_OnWorldQuitEventHandler);
        }

        public static int cleanVampFaces(object obj)
        {
            try
            {                
                foreach (KeyValuePair<Sim, bool> pair in bloodySims)
                {
                    if (bloodySims[pair.Key])
                    {
                        Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTBlood.runClean), pair.Key));
                    }
                }                
            }
            catch(Exception exception)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "An error is ocurred when uninstalling. Check notification bar");
                StyledNotification.Show(new StyledNotification.Format("Failed the removal of the sim dictionary (bloodysims) \n" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
            return 1;
        }
        public static ListenerAction OnClean(Event e)
        {
            Sim actor = e.Actor as Sim;
            if (actor != null)
            {
                Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTBlood.RemoveMakeup), actor));
            }
            return ListenerAction.Keep;
        }

        private static ListenerAction OnGotBuff(Event e)
        {
            Sim actor = e.Actor as Sim;
            if (actor != null)
            {
                if (actor.SimDescription.IsVampire)
                {
                    if ((lastTriggered == null) || (lastTriggered != actor))
                    {
                        Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTBlood.ProcessBuff), actor));
                    }
                }
                else if (bloodySims.ContainsKey(actor))
                {
                    Simulator.AddObject(new OneShotFunctionWithParams(new FunctionWithParam(VTBlood.RemoveMakeup), actor));
                }
            }
            return ListenerAction.Keep;
        }

        public static void OnWorldLoadFinishedHandler()
        {
            maskPart = ResourceKey.Parse(maskTypeID + "-" + maskGroupID + "-" + maskInstanceID);
            sBloodBuffListener = EventTracker.AddListener(EventTypeId.kGotBuff, new ProcessEventDelegate(VTBlood.OnGotBuff));
            if (bathingCleans)
            {
                sBathListener = EventTracker.AddListener(EventTypeId.kEventTakeBath, new ProcessEventDelegate(VTBlood.OnClean));
            }
            if (brushingCleans)
            {
                sBrushListener = EventTracker.AddListener(EventTypeId.kBrushedTeeth, new ProcessEventDelegate(VTBlood.OnClean));
            }
            if (showeringCleans)
            {
                sShowerListener = EventTracker.AddListener(EventTypeId.kEventTakeShower, new ProcessEventDelegate(VTBlood.OnClean));
            }
            PartSearch search = new PartSearch();
            foreach (CASPart part in search)
            {
                if (part.Key == maskPart)
                {
                    VTBloodMask = part;
                }
            }
            search.Reset();
        }
        private static void ProcessBuff(object obj)
        {
            Sim sim = obj as Sim;
            bool flag = false;
            using (IEnumerator<BuffInstance> enumerator = sim.BuffManager.Buffs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BuffInstance current = enumerator.Current;
                    if ((current.mBuffGuid == BuffNames.Sated) || (current.mBuffGuid == BuffNames.SanguineSnack) || (current.mBuffGuid == BuffNames.DrankFromAFairy) || (current.BuffGuid == 0xB8C52F5C23B7F50C || (current.BuffGuid == 0xDDA3184CBEAFDF22)))
                    {
                        flag = true;
                        if (current.mStartTime > (SimClock.CurrentTicks - 0x2dL))
                        {
                            SetMakeup(sim);
                            return;
                        }
                    }
                }
                if ((!flag && bloodySims.ContainsKey(sim)) && (timeCleans || !sim.IsInActiveHousehold))
                {
                    if (bloodySims[sim])
                    {
                        RemoveMakeup(sim);
                    }
                }
            }
        }

        public static void RemoveMakeup(object obj)
        {
            Sim key = obj as Sim;
            if ((key.SimDescription.IsVampire && bloodySims.ContainsKey(key)) && bloodySims[key])
            {
                RemoveMakeup(key);
            }
        }
        public static void RemoveMakeupFromToggle()
        {
            List<Sim> list = new List<Sim>(Sims3.Gameplay.Queries.GetObjects<Sim>());
            foreach (Sim sim in list)
            {
                if ((sim.SimDescription.IsVampire && bloodySims.ContainsKey(sim)) && bloodySims[sim])
                {
                    RemoveMakeup(sim);
            }
        }
        }

        private static void runClean(object obj)
        {
            Sim sim = obj as Sim;
            if (sim != null)
                try
                {
                    {
                        SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Cleaning bloody face of: " + sim.FullName + "...");
                        RemoveMakeup(sim);
                        SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Cleaned bloody face of: " + sim.FullName + "!");
                    }
                }
                catch(Exception exception)
                {
                    SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Failed the removal process of the blood makeup of: " + sim.FullName + "!" + "\nCheck notification bar.");
                    StyledNotification.Show(new StyledNotification.Format("Failed the uninstall process in the removal of the blood makeup of: " + sim.FullName + "!\nTechinical info to get more information about the error: \n" + exception, StyledNotification.NotificationStyle.kDebugAlert));
                }
        }

        private static void sendDebugMsg(string msg)
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
            if (!bloodySims.ContainsKey(sim))
            {
                bloodySims.Add(sim, true);
            }
            else
            {
                bloodySims[sim] = true;
            }
            lastTriggered = null;
        }
        public static void RemoveMakeup(Sim sim)
        {
            lastTriggered = sim;
            MakeupManagement(sim, false);
            if (!bloodySims.ContainsKey(sim))
            {
                bloodySims.Remove(sim);
            }
            else
            {
                bloodySims[sim] = false;
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
                    string designPreset = CASUtils.PartDataGetPreset(maskPart, uint.Parse(maskPresetIndex));
                    builder.AddPart(VTBloodMask);
                    sim.BuffManager.AddElement(0x1A46435F0559B988, (Origin)ResourceUtils.HashString64("ByBloodyFace"));
                    CASUtils.ApplyPresetToPart(builder, VTBloodMask, designPreset);
                    builder.SetPartPreset(maskPart, uint.Parse(maskPresetIndex), designPreset);
                }
                else
                {
                    builder.RemovePart(VTBloodMask);
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
                                        string str2 = CASUtils.PartDataGetPreset(maskPart, uint.Parse(maskPresetIndex));
                                        builder.AddPart(VTBloodMask);
                                        sim.BuffManager.AddElement(0x1A46435F0559B988, (Origin)ResourceUtils.HashString64("ByBloodyFace"));
                                        CASUtils.ApplyPresetToPart(builder, VTBloodMask, str2);
                                        builder.SetPartPreset(maskPart, uint.Parse(maskPresetIndex), str2);
                                    }
                                    else
                                    {
                                        builder.RemovePart(VTBloodMask);
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
                SimpleMessageDialog.Show("Vampire Tweaker - Blood Effect", sim.Name + "\nApply makeup failed!\n" + exception);
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
                bloodySims = null;
            }
        }
    }
}