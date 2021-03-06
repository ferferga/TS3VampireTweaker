using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.RabbitHoles;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using System;
using System.Collections.Generic;
using VampireTweaker.Interactions;
using VampireTweaker.VampireEffects;
using VT.Skills;
using VT.Buffs;

namespace VampireTweaker.ModInitiatorAndHandler
{
    public class Instantiator
    {
        [Tunable]
        internal static bool kInstantiator = false;
        private static EventListener sSimInstantiatedListener = null;
        private static EventListener sSimAgedUpListener = null;
        private static EventListener sBoughtObject = null;
        private static EventListener sOnObjectChanged = null;
        private static EventListener sOnObjectAddedToInventory = null;
        [PersistableStatic(true)]
        public static bool pInteractionsActive;
        [PersistableStatic(true)]
        public static bool pBloodActive;
        [PersistableStatic(true)]
        public static bool pThirstActive;
        [PersistableStatic(true)]
        public static bool pConversionActive;
        [PersistableStatic(true)]
        public static bool pDebugOn;
        [PersistableStatic(true)]
        public static bool pBiteMarkActive;
        [PersistableStatic(true)]
        public static bool pLoadNotificationDeActived;
        [PersistableStatic(true)]
        public static bool pBuffActive;
        [PersistableStatic(true)]
        public static bool pBathCleaning;
        [PersistableStatic(true)]
        public static bool pShowerCleaning;
        public static EventListener sBuffSparkleDisable;
        static Instantiator()
        {
            World.OnWorldLoadFinishedEventHandler += new EventHandler(Instantiator.OnWorldLoadFinishedHandler);
            LoadSaveManager.ObjectGroupsPreLoad += Instantiator.OnPreLoad;
            LoadSaveManager.ObjectGroupsPreLoad += Instantiator.OnPreLoadBuff;
        }
        public static void OnPreLoad()
        {
            (new SkillBooter()).LoadBuffData();
        }
        public static void OnPreLoadBuff()
        {
            (new BuffBooter()).LoadBuffData();
        }
        public static void AddInteractions(Sim obj)
        {
            try
            {
                obj.AddInteraction(ForceConversion.Singleton, true);
                obj.AddInteraction(VampireTweaker.Interactions.Resurrect.Singleton, true);
                obj.AddInteraction(Kill.Singleton, true);
            }
            catch (Exception exception)
            {
                Instantiator.Exception(exception);
                StyledNotification.Show(new StyledNotification.Format("Failed to load Vampire Tweaker Interactions. Error generated by fer456.VampireTweaker.Interactions.dll. See info below and in Documents_Electronic Arts_The Sims 3" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
        }
        protected static ListenerAction HandleNewSim(Event e)
        {
            try
            {
                Sim sim = e.TargetObject as Sim;
                if (sim != null)
                {
                    Instantiator.AddInteractions(sim);
                }
            }
            catch (Exception exception)
            {
                Instantiator.Exception(exception);
                StyledNotification.Show(new StyledNotification.Format("Failed to load Vampire Tweaker Interactions. Error generated by fer456.VampireTweaker.Interactions.dll. See info below and in Documents_Electronic Arts_The Sims 3" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
            return ListenerAction.Keep;
        }
        public static void OnWorldLoadFinishedHandler(object sender, EventArgs e)
        {
            try
            {
                if (GameUtils.IsInstalled(ProductVersion.EP7) || GameUtils.IsInstalled(ProductVersion.EP3))
                {
                    Instantiator.AddInteractionsToCityHall();
                    if (Instantiator.pInteractionsActive)
                    {
                        Instantiator.AddAllInteractionsForSims();
                        Instantiator.AddAllInteractionsForUrnstones();
                        Instantiator.AddAllInteractionsForHospitals();
                    }
                    if (Instantiator.pBloodActive)
                    {
                        VTBlood.OnWorldLoadFinishedHandler();
                    }
                    if (Instantiator.pBiteMarkActive)
                    {
                        VTBiteMark.OnWorldLoadFinishedHandler();
                    }
                    if (Instantiator.pThirstActive)
                    {
                        VTThirst.OnWorldLoadFinishedHandler();
                    }
                    if (Instantiator.pConversionActive)
                    {
                        VTConversion.OnWorldLoadFinishedHandler();
                    }
                    if (!Instantiator.pLoadNotificationDeActived)
                    {
                        StyledNotification.Show(new StyledNotification.Format("Loaded Vampire Tweaker correctly without any errors", StyledNotification.NotificationStyle.kSystemMessage));
                        SimpleMessageDialog.Show("Vampire Tweaker", "Every functionality of the mod is deactivated, please, consider to go to City Hall>Vampire Tweaker... to customize the mod with a easy method and disable this message and the notification to load on start. Anything won�t work if you don�t active in the City Hall the function.");
                        if (!GameUtils.IsInstalled(ProductVersion.EP7))
                        {
                            StyledNotification.Show(new StyledNotification.Format("We have detected you haven�t installed Supernatural EP. Some functionalities of the mod requires this expansion, but it won�t generate any conflicts. The functionalities will be activated the first time you install Supernatural EP", StyledNotification.NotificationStyle.kSystemMessage));
                        }
                    }
                    if (!Instantiator.pBuffActive)
                    {
                        sBuffSparkleDisable = EventTracker.AddListener(EventTypeId.kGotBuff, new ProcessEventDelegate(EnableSparkle.OnGotBuff));
                    }
                }
                if (!GameUtils.IsInstalled(ProductVersion.EP7) || !GameUtils.IsInstalled(ProductVersion.EP3))
                {
                    SimpleMessageDialog.Show("Vampire Tweaker", "It seems you haven�t installed any expansion of Vampires (Late Night or Supernatural). The mod is deactivated and any function won�t work. Upgrade the Expansion Pack or install it. If you won�t install any expansion, you should remove this mod, it isn�t useful for you without the EPs.");
                }
            }
            catch (Exception exception)
            {
                Instantiator.Exception(exception);
                StyledNotification.Show(new StyledNotification.Format("Failed to load Vampire Tweaker things. Error generated by fer456.VampireTweaker.Interactions.dll into VampireTweaker.ModInitiatorAndHandler.Instantiator || See info below and in Documents_Electronic Arts_The Sims 3" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
        }
        public static bool Exception(Exception exception)
        {
            try
            {
                return ((IScriptErrorWindow)AppDomain.CurrentDomain.GetData("ScriptErrorWindow")).DisplayScriptError(null, exception);
            }
            catch
            {
                WriteLog(exception);
                return true;
            }
        }
        public static bool WriteLog(Exception exception)
        {
            try
            {
                new ScriptError(null, exception, 0).WriteMiniScriptError();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void AddAllInteractionsForSims()
        {
            List<Sim> list = new List<Sim>(Sims3.Gameplay.Queries.GetObjects<Sim>());
            foreach (Sim sim in list)
            {
                if (sim != null)
                {
                    AddInteractions(sim);
                }
            }
            sSimInstantiatedListener = EventTracker.AddListener(EventTypeId.kSimInstantiated, new ProcessEventDelegate(Instantiator.HandleNewSim));
            sSimAgedUpListener = EventTracker.AddListener(EventTypeId.kSimAgeTransition, new ProcessEventDelegate(Instantiator.HandleNewSim));
        }
        public static void AddAllInteractionsForUrnstones()
        {
            try
            {
                List<Urnstone> list = new List<Urnstone>(Sims3.Gameplay.Queries.GetObjects<Urnstone>());
                foreach (Urnstone urnstone in list)
                {
                    if (urnstone != null)
                    {
                        Instantiator.AddInteractionsForUrnstones(urnstone);
                    }
                }
                sBoughtObject = EventTracker.AddListener(EventTypeId.kBoughtObject, new ProcessEventDelegate(Instantiator.OnObjectChanged));
                sOnObjectAddedToInventory = EventTracker.AddListener(EventTypeId.kInventoryObjectAdded, new ProcessEventDelegate(Instantiator.OnObjectChanged));
                sOnObjectChanged = EventTracker.AddListener(EventTypeId.kObjectStateChanged, new ProcessEventDelegate(Instantiator.OnObjectChanged));
            }
            catch (Exception exception)
            {
                Instantiator.Exception(exception);
                StyledNotification.Show(new StyledNotification.Format("Failed to load Vampire Tweaker Interactions. Error generated by fer456.VampireTweaker.Interactions.dll. See info below and in Documents_Electronic Arts_The Sims 3" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
        }
        private static ListenerAction OnObjectChanged(Event e)
        {
            try
            {
                Urnstone urnstone = e.TargetObject as Urnstone;
                if (urnstone != null)
                {
                    Instantiator.AddInteractionsForUrnstones(urnstone);
                }
            }
            catch (Exception exception)
            {
                Instantiator.Exception(exception);
                StyledNotification.Show(new StyledNotification.Format("Failed to load Vampire Tweaker Interactions. Error generated by fer456.VampireTweaker.Interactions.dll. See info below and in Documents_Electronic Arts_The Sims 3" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
            return ListenerAction.Keep;
        }
        private static void AddInteractionsForUrnstones(Urnstone obj)
        {
            obj.AddInteraction(ResurrectFromGrave.Singleton, true);
        }
        public static void AddAllInteractionsForHospitals()
        {
            try
            {
                List<Hospital> list = new List<Hospital>(Sims3.Gameplay.Queries.GetObjects<Hospital>());
                foreach (Hospital hospital in list)
                {
                    if (hospital != null)
                    {
                        Instantiator.AddInteractionsToHospitals(hospital);
                    }
                }
                List<ComboHospitalScienceLab> list2 = new List<ComboHospitalScienceLab>(Sims3.Gameplay.Queries.GetObjects<ComboHospitalScienceLab>());
                foreach (ComboHospitalScienceLab hospital in list2)
                {
                    if (hospital != null)
                    {
                        Instantiator.AddInteractionsToHospitals(hospital);
                    }
                }
            }
            catch (Exception exception)
            {
                Instantiator.Exception(exception);
                StyledNotification.Show(new StyledNotification.Format("Failed to load Vampire Tweaker Interactions. Error generated by fer456.VampireTweaker.Interactions.dll. See info below and in Documents_Electronic Arts_The Sims 3" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
        }
        private static void AddInteractionsToHospitals(RabbitHole obj)
        {
            obj.AddInteraction(GetBlood.Singleton, true);
        }
        private static void AddInteractionsToCityHall()
        {
            List<CityHall> list = new List<CityHall>(Sims3.Gameplay.Queries.GetObjects<CityHall>());
            foreach (CityHall obj in list)
            {
                if (obj != null)
                {
                    Instantiator.AddGameObjInteractions(obj);
                }
            }
            List<CityHallEP11> list2 = new List<CityHallEP11>(Sims3.Gameplay.Queries.GetObjects<CityHallEP11>());
            foreach (CityHallEP11 obj in list2)
            {
                if (obj != null)
                {
                    Instantiator.AddGameObjInteractions(obj);
                }
            }
            List<CityHallNoClock> list3 = new List<CityHallNoClock>(Sims3.Gameplay.Queries.GetObjects<CityHallNoClock>());
            foreach (CityHallNoClock obj in list3)
            {
                if (obj != null)
                {
                    Instantiator.AddGameObjInteractions(obj);
                }
            }
            List<ComboCityhallPoliceMilitary> list4 = new List<ComboCityhallPoliceMilitary>(Sims3.Gameplay.Queries.GetObjects<ComboCityhallPoliceMilitary>());
            foreach (ComboCityhallPoliceMilitary obj in list4)
            {
                if (obj != null)
                {
                    Instantiator.AddGameObjInteractions(obj);
                }
            }
        }
        private static void AddGameObjInteractions(RabbitHole obj)
        {
            obj.AddInteraction(EnableMenus.Singleton, true);
            obj.AddInteraction(EnableBiteMark.Singleton, true);
            obj.AddInteraction(EnableBlood.Singleton, true);
            obj.AddInteraction(EnableConversion.Singleton, true);
            obj.AddInteraction(EnableThirst.Singleton, true);
            obj.AddInteraction(EnableNotification.Singleton, true);
            obj.AddInteraction(EnableSparkle.Singleton, true);
            obj.AddInteraction(Credits.Singleton, true);
            obj.AddInteraction(HowToUninstall.Singleton, true);
            obj.AddInteraction(EnableBathroomCleaning.Singleton, true);
            obj.AddInteraction(Reset.Singleton, true);
            obj.AddInteraction(Debug.Singleton, true);
        }
        public static void RemoveAllInteractionsForSims()
        {
            List<Sim> list = new List<Sim>(Sims3.Gameplay.Queries.GetObjects<Sim>());
            foreach (Sim sim in list)
            {
                if (sim != null)
                {
                    Instantiator.RemoveSimInteractions(sim);
                }
            }
        }
        public static void RemoveSimInteractions(Sim obj)
        {
            obj.RemoveInteractionByType(ForceConversion.Singleton);
            obj.RemoveInteractionByType(Kill.Singleton);
            obj.RemoveInteractionByType(Resurrect.Singleton);
        }
        public static void RemoveAllInteractionsForHospitals()
        {
            List<Hospital> list = new List<Hospital>(Sims3.Gameplay.Queries.GetObjects<Hospital>());
            foreach (Hospital rabbithole in list)
            {
                if (rabbithole != null)
                {
                    Instantiator.RemoveHospitalInteractions(rabbithole);
                }
            }
            List<ComboHospitalScienceLab> list2 = new List<ComboHospitalScienceLab>(Sims3.Gameplay.Queries.GetObjects<ComboHospitalScienceLab>());
            foreach (ComboHospitalScienceLab hospital in list2)
            {
                if (hospital != null)
                {
                    Instantiator.RemoveHospitalInteractions(hospital);
                }
            }
        }
        public static void RemoveHospitalInteractions(RabbitHole obj)
        {
            obj.RemoveInteractionByType(GetBlood.Singleton);
        }
        public static void RemoveAllInteractionsForUrnstones()
        {
            List<Urnstone> list = new List<Urnstone>(Sims3.Gameplay.Queries.GetObjects<Urnstone>());
            foreach (Urnstone urnstone in list)
            {
                if (urnstone != null)
                {
                    Instantiator.RemoveUrnstoneInteractions(urnstone);
                }
            }
        }
        public static void RemoveUrnstoneInteractions(Urnstone obj)
        {
            obj.RemoveInteractionByType(ResurrectFromGrave.Singleton);
        }
    }
}