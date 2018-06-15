using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class AltarBedEP7Buff : Buff
    {
        private const ulong kAltarBedEP7BuffGuid = 0x7BEDE743C0C42B1F;
        public static ulong StaticGuid
        {
            get
            {
                return 0x7BEDE743C0C42B1F;

            }
        }
        public AltarBedEP7Buff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
