using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class GettedBlood : Buff
    {
        private const ulong kGettedBloodBuffGuid = 0xDDA3184CBEAFDF22;
        public static ulong StaticGuid
        {
            get
            {
                return 0xDDA3184CBEAFDF22;

            }
        }
        public GettedBlood(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
