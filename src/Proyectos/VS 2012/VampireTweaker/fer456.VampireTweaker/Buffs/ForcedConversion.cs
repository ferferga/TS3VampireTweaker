using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class ForcedConversionBuff : Buff
    {
        private const ulong kForcedConversionBuffGuid = 0xB040AC12800F6F7D;
        public static ulong StaticGuid
        {
            get
            {
                return 0xB040AC12800F6F7D;

            }
        }
        public ForcedConversionBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}