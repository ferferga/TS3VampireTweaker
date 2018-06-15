using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class KilledBuff : Buff
    {
        private const ulong kKilledBuffGuid = 0xF210BB43E0C33B3F;
        public static ulong StaticGuid
        {
            get
            {
                return 0xF210BB43E0C33B3F;

            }
        }
        public KilledBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
