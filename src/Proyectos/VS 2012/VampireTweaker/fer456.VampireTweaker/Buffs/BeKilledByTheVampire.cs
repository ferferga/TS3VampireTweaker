using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class KilledByTheVampireBuff : Buff
    {
        private const ulong kKilledByTheVampireBuffGuid = 0xBCF83A2A06FD58B0;
        public static ulong StaticGuid
        {
            get
            {
                return 0xBCF83A2A06FD58B0;

            }
        }
        public KilledByTheVampireBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}