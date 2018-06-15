using Sims3.Gameplay.ActorSystems;
using System;
using Sims3.Gameplay.Actors;

namespace VT.Buffs
{
    internal class ReadedAMindBuff : Buff
    {
        private const ulong kReadedAMindBuffGuid = 0xB210DB43E0C33B3B;
        public static ulong StaticGuid
        {
            get
            {
                return 0xB210DB43E0C33B3B;

            }
        }
        public ReadedAMindBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
