using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class BitedByTheVampireBuff : Buff
    {
        private const ulong kBitedByTheVampireBuffGuid = 0xE0CDC26A9F9D3B74;
        public static ulong StaticGuid
        {
            get
            {
                return 0xE0CDC26A9F9D3B74;

            }
        }
        public BitedByTheVampireBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
