using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class ResurrectedBuff : Buff
    {
        private const ulong kResurrectedBuffGuid = 0xCDEDE943B0C64B1F;
        public static ulong StaticGuid
        {
            get
            {
                return 0xCDEDE943B0C64B1F;

            }
        }
        public ResurrectedBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
