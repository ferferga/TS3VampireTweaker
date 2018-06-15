using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class GettedBloodChild : Buff
    {
        private const ulong kGettedBloodChildBuffGuid = 0xB8C52F5C23B7F50C;
        public static ulong StaticGuid
        {
            get
            {
                return 0xB8C52F5C23B7F50C;

            }
        }
        public GettedBloodChild(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
