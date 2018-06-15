using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class BitedAnHumanBuff : Buff
    {
        private const ulong kBitedAnHumanBuffGuid = 0xD224DD43B0C28B1F;
        public static ulong StaticGuid
        {
            get
            {
                return 0xD224DD43B0C28B1F;

            }
        }
        public BitedAnHumanBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
