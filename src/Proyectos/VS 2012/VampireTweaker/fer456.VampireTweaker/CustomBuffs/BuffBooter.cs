using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using System;
using VampireTweaker.Helpers;

namespace VT.Buffs
{
    public class BuffBooter
    {
        protected static bool kInstantiator = false;
        public void LoadBuffData()
        {
            this.AddBuffs(null);
            UIManager.NewHotInstallStoreBuffData += new UIManager.NewHotInstallStoreBuffCallback(this.AddBuffs);
        }
        public void AddBuffs(ResourceKey[] resourceKeys)
        {
            try
            {
                ResourceKey key = new ResourceKey(ResourceUtils.HashString64("VampireTweaker_CustomBuffs"), 0x0333406C, 0x0);
                XmlDbData xmlDbData = XmlDbData.ReadData(key, false);
                if (xmlDbData != null)
                {
                    BuffManager.ParseBuffData(xmlDbData, true);
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