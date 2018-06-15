using Sims3.Gameplay.Utilities;
using Sims3.UI;
using System;
using System.Collections.Generic;

namespace VampireTweaker.Helpers
{
    public static class Debugger
    {
        private static AlarmHandle sReporterHandle = AlarmHandle.kInvalidHandle;
        private static List<string> sLog;
        private static DateTime sLastReported = DateTime.MinValue;
        private static bool sWorldLoaded = false;
        private static void AddAlarm()
        {
            if (Debugger.sWorldLoaded && Debugger.sReporterHandle == AlarmHandle.kInvalidHandle)
            {
                Debugger.sReporterHandle = AlarmManager.Global.AddAlarmRepeating(1f, TimeUnit.Minutes, new AlarmTimerCallback(Debugger.Report), 1f, TimeUnit.Minutes, "Debugger Reporter Alarm", AlarmType.NeverPersisted, null);
            }
        }
        public static void Append(string str)
        {
            if (Debugger.sLog == null)
            {
                Debugger.sLog = new List<string>();
            }
            Debugger.sLog.Add(str);
            Debugger.AddAlarm();
        }
        public static void OnWorldLoaded(object sender, EventArgs e)
        {
            Debugger.sWorldLoaded = true;
            if (Debugger.sLog == null)
            {
                return;
            }
            Debugger.AddAlarm();
        }
        public static void Report()
        {
            if (Debugger.sLog != null)
            {
                foreach (string current in Debugger.sLog)
                {
                    StyledNotification.Show(new StyledNotification.Format(current, StyledNotification.NotificationStyle.kDebugAlert));
                }
            }
            AlarmManager.Global.RemoveAlarm(Debugger.sReporterHandle);
            Debugger.sReporterHandle = AlarmHandle.kInvalidHandle;
        }
        public static void WriteExceptionLog(Exception e, object sender, string messageText)
        {
            string text = "\nError: ";
            text += ((!string.IsNullOrEmpty(messageText)) ? messageText : "No error message given.");
            text += "\n\n ----- Sender: ";
            text += ((sender != null) ? sender.GetType().FullName : "null or static");
            text = text + "\n\n ----- Exception: " + e.ToString();
            ScriptError scriptError = new ScriptError(null, new Exception(text), 0);
            scriptError.WriteMiniScriptError();
            if (Debugger.sLastReported == DateTime.MinValue || (DateTime.Now - Debugger.sLastReported).Minutes > 1)
            {
                Debugger.Append("fer456_VampireTweaker_CustomSkills Script Error caught. Check user files for specifics and report to the developer (fer456). Check Documents/Electronic Arts/The Sims 3/ for get the script error files. Thank you!. This script error is caused because there is a problem to load the buff booter when the world is loaded.");
            }
            Debugger.sLastReported = DateTime.Now;
        }
    }
}
