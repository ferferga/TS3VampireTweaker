using System;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using VampireTweaker.Helpers;

namespace VT.Skills
{
    internal class SkillBooter
    {
        static bool HasBeenLoaded = false;
        public void LoadBuffData()
        {
            this.AddSkills(null);
            UIManager.NewHotInstallStoreSkillsData += new UIManager.NewHotInstallStoreSkillsCallback(this.AddSkills);
        }
        public void AddSkills(ResourceKey[] resourceKeys)
        {
            try
            {
                if (HasBeenLoaded) return;
                HasBeenLoaded = true;
                ResourceKey key = new ResourceKey(ResourceUtils.HashString64("VampireTweaker_CustomSkills"), 0xA8D58BE5, 0x0);
                XmlDbData xmlDbData = XmlDbData.ReadData(key, false);
                if (xmlDbData != null)
                {
                    SkillManager.ParseSkillData(xmlDbData, true);
                }
            }
            catch (Exception e)
            {
                Debugger.WriteExceptionLog(e, this, null);
            }
            {
            }
        }
    }
}