using System;
using System.Collections.Generic;
using Sims3.SimIFace;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.EventSystem;
using Sims3.UI;
using Sims3.Gameplay.Actors;
using VampireTweaker.ModInitiatorAndHandler;
using VampireTweaker.VampireEffects;

namespace VampireTweaker.Helpers
{
    public class Uninstall
    {
        [Tunable]
        internal static bool kInstantiator = false;
        public static CommandHandler UninstallVampireTweaker = new CommandHandler(Uninstall.cleanVampireTweakerThings);
        public static CommandHandler HelpInUninstallVampireTweaker = new CommandHandler(Uninstall.ShowUninstallHelp);
        public static CommandHandler ResetVampireTweaker = new CommandHandler(Uninstall.ResetVampireTweakerProc);
        static Uninstall()
        {
            CommandSystem.RegisterCommand("VampireTweaker_Uninstall", "Run before uninstalling Vampire Tweaker. Removes all added things by the mod to complete the uninstalling (OBJECTS AND BUFFS AREN´T REMOVED.IT DOESN´T HAVE SPECIAL REQUIRIMENTS IN UNINSTALLING).To complete the uninstall, simply remove the .package file from the Mods folder. Also is highly recommended to clean all cache files and reset the city with MasterController. If you need help, run this command:'VampireTweaker_UninstallHelp'. -fer456", Uninstall.UninstallVampireTweaker);
            CommandSystem.RegisterCommand("VampireTweaker_UninstallHelp", "Run it if you don´t know how to uninstall the mod. It will show up a little tutorial with explanations.", Uninstall.HelpInUninstallVampireTweaker);
            CommandSystem.RegisterCommand("VampireTweaker_Reset", "Run it after installing or if you have problems with the mod. It deletes some data of the mod, especially the effects. Provides also a good guide with the reasons of why the mod isn´t work", Uninstall.ResetVampireTweaker);
        }
        public static int cleanVampireTweakerThings(object[] args)
        {
            AlarmManager.Global.AddAlarm(15f, TimeUnit.Minutes, new AlarmTimerCallback(Uninstall.ReceiveFinalMessage), "", AlarmType.NeverPersisted, null);
            SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Started the Uninstall process...\nDuring the process, the mod will generate messages like this format and notifications for make more easy to track the uninstall errors. If you get notifications, the process is failed in a point, but if you don´t get notifications, the uninstall process was completed succesfully without problems. Also, when occurs an error, messages like this will be created. Example of text:\n'An error is ocurred when uninstalling. Check notification bar'.\nMORE HELP:RUN THE COMMAND 'VampireTweaker_UninstallHelp");
            SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Cleaning the mod...");
            CleanTrackers();
            CleanMakeup();
            RemoveInteractions();
            SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Finished the uninstall process.\nPlease wait to a newer message before saving.");
            SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "DO NOT FORGET TO CHECK THE NOTIFICATIONS ZONE TO CHECK THE UNINSTALL PROBLEMS. IN NORMAL CASES, YOU MUSTN´T HAVE NOTIFICATIONS CREATED BY THE MOD.\nIf there isn´t messages in the notification bar, you make the uninstall correctly. That´s good thing :)");
            return 0;
        }
        public static int ResetVampireTweakerProc(object[] args)
        {
            if (!GameUtils.IsInstalled(ProductVersion.EP3) && !GameUtils.IsInstalled(ProductVersion.EP7))
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Diagnosis", "The mod doesn´t work because you haven´t installed correctly Late Night and Supernatural. The procces will finish.");
            }
            else
            {
                if (!GameUtils.IsInstalled(ProductVersion.EP3) && GameUtils.IsInstalled(ProductVersion.EP7))
                {
                    if (AcceptCancelDialog.Show("Probably the mod doesn´t works well because you only have Supernatural EP and you want to use something from Late Night EP. Accept this dialog to continue diagnose and cleaning"))
                    {
                        Uninstall.ResetVampireTweakerStep2();
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (GameUtils.IsInstalled(ProductVersion.EP3) && !GameUtils.IsInstalled(ProductVersion.EP7))
                    {
                        if (AcceptCancelDialog.Show("Probably the mod doesn´t works well because you only have Late Night and you probably has a error with something that requires Supernatural. Accept the dialog if you want to continue cleaning and diagnosing"))
                        {
                            Uninstall.ResetVampireTweakerStep2();
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        if (GameUtils.IsInstalled(ProductVersion.EP3) && GameUtils.IsInstalled(ProductVersion.EP7))
                        {
                            Uninstall.ResetVampireTweakerStep2();
                        }
                    }
                }
            }
            return 0;
        }
        public static int ResetVampireTweakerStep2()
        {
            SimpleMessageDialog.Show("Vampire Tweaker - Helpers", "Started the Reset process...\nDuring the process, the mod will generate messages like this format and notifications for make more easy to track the reset errors. If you get notifications, the process is failed in a point, but if you don´t get notifications, the reset process was completed succesfully without problems. Also, when occurs an error, messages like this will be created. Example of text:\n'An error is ocurred when resetting. Check notification bar'.");
            CleanTrackers();
            CleanMakeup();
            SimpleMessageDialog.Show("Vampire Tweaker - Helpers", "Cleaned makeup. Starting to reset modified objects by the mod...");
            SimpleMessageDialog.Show("Vampire Tweaker - Helpers", "Recreating everything...");
            Regen();
            SimpleMessageDialog.Show("Vampire Tweaker - Helpers", "Finished the reset process.\nPlease wait to a newer message before saving to make permanent that changes.");
            SimpleMessageDialog.Show("Vampire Tweaker - Helpers", "DO NOT FORGET TO CHECK THE NOTIFICATIONS ZONE TO CHECK THE RESET PROBLEMS. IN NORMAL CASES, YOU MUSTN´T HAVE NOTIFICATIONS CREATED BY THE MOD.\nIf there isn´t messages in the notification bar, you make the reset correctly. That´s good thing :)");
            SimpleMessageDialog.Show("Vampire Tweaker - Helpers", "Save now your game");
            return 0;
        }
        public static int Regen()
        {
            EventTracker.AddListener(VTThirst.sThirstBuffListener);
            EventTracker.AddListener(VTBlood.sBathListener);
            EventTracker.AddListener(VTBlood.sBloodBuffListener);
            EventTracker.AddListener(VTBlood.sBrushListener);
            EventTracker.AddListener(VTBlood.sShowerListener);
            EventTracker.AddListener(VTBiteMark.sBiteMarkBuffListener);
            EventTracker.AddListener(VTConversion.sConversionBathListener);
            EventTracker.AddListener(VTConversion.sConversionBuffListener);
            EventTracker.AddListener(VTConversion.sConversionShowerListener);
            EventTracker.AddListener(VTThirst.sSimAgedUp);
            EventTracker.AddListener(VTThirst.sSimInstantiated);
            VTThirst.thirstySims.Clear();
            VTBlood.bloodySims.Clear();
            VTConversion.convertedSims.Clear();
            VTBiteMark.bitedSims.Clear();
            return 0;
        }
        private static void ReceiveFinalMessage()
        {
            SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "Finished the uninstall! Save your game and remove the package.");
        }
        public static int CleanTrackers()
        {
            try
            {
                EventTracker.RemoveListener(VTThirst.sThirstBuffListener);
                EventTracker.RemoveListener(VTBlood.sBathListener);
                EventTracker.RemoveListener(VTBlood.sBloodBuffListener);
                EventTracker.RemoveListener(VTBlood.sBrushListener);
                EventTracker.RemoveListener(VTBlood.sShowerListener);
                EventTracker.RemoveListener(VTBiteMark.sBiteMarkBuffListener);
                EventTracker.RemoveListener(VTConversion.sConversionBathListener);
                EventTracker.RemoveListener(VTConversion.sConversionBuffListener);
                EventTracker.RemoveListener(VTConversion.sConversionShowerListener);
                EventTracker.RemoveListener(VTThirst.sSimAgedUp);
                EventTracker.RemoveListener(VTThirst.sSimInstantiated);
            }
            catch (Exception exception)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "An error is ocurred when uninstalling. Check notification bar");
                StyledNotification.Show(new StyledNotification.Format("Failed the uninstall in the removal of trackers. Retry the uninstall. If you get the same error, run the command VampireTweaker_UninstallHelp to get more information \n" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
            return 1;
        }
        public static int CleanMakeup()
        {
            try
            {
                VTBlood.cleanVampFaces(true);
                VTThirst.cleanVampFaces(true);
                VTBiteMark.cleanHumanNecks(true);
                VTConversion.cleanBodys(true);
            }
            catch (Exception exception)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "An error is ocurred when uninstalling. Check notification bar");
                StyledNotification.Show(new StyledNotification.Format("Failed the uninstall process in the removal of the makeup. Retry the uninstall. If you get the same error, run the command VampireTweaker_UninstallHelp to get more information \n" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
            return 1;
        }
        public static void RemoveInteractions()
        {
            try
            {
                Instantiator.RemoveAllInteractionsForHospitals();
                Instantiator.RemoveAllInteractionsForSims();
                Instantiator.RemoveAllInteractionsForUrnstones();
            }
            catch (Exception exception)
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Uninstalling Wizard", "An error is ocurred when uninstalling. Check notification bar");
                StyledNotification.Show(new StyledNotification.Format("Failed the uninstall process in the removal of the interactions. YOU CAN CONTINUE UNINSTALLING WITHOUT ANY PROBLEMS, SIMPLY SAVE AND REMOVE THE PACKAGE WHEN YOU GET A MESSAGE WITH THIS INSTRUCTIONS \n" + exception, StyledNotification.NotificationStyle.kDebugAlert));
            }
        }
        public static int ShowUninstallHelp(object[] args)
        {
            try
            {
                SimpleMessageDialog.Show("Vampire Tweaker - Help Step 2", "When finish the uninstall, you will receive a confirmation message");
                SimpleMessageDialog.Show("Vampire Tweaker - Help Step 1", "When you run the 'VampireTweaker_Uninstall' command, the script will start the process of the uninstall of the mod. Windows like this will show up information in real time during the uninstall process.\nThis type of Window also make some advices and show up a message each time that something happens.\nIt´s a easy method to know the state of the removal.");
                StyledNotification.Show(new StyledNotification.Format("Notification like this is shown when an error occurs. It is strange that happens an error, but it can always occur. It will show a small message with information and a summary of code to help understand the error. Don't worry if you don't see a notification, that means that everything went well and you can finally uninstall the mod", StyledNotification.NotificationStyle.kDebugAlert));
                StyledNotification.Show(new StyledNotification.Format("If you run this command because you have problems with the uninstall, retry the process. If it still with errors, reset the city with MasterController and do again the process. If still with problems, copy the error code and post a comment or send a message (to fer456) in the Mod thread's. Make sure you copy without errors the entire error code.", StyledNotification.NotificationStyle.kSystemMessage));
            }
            catch
            {
            }
            return 1;
        }
    }
}