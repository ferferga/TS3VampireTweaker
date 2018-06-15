using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class AltarBedEP3Buff : Buff
    {
        private const ulong kAltarBedEP3BuffGuid = 0xABEDE343B0C48B1A;
        public static ulong StaticGuid
        {
            get
            {
                return 0xABEDE343B0C48B1A;

            }
        }
        public AltarBedEP3Buff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
