using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class RedEyesBuff : Buff
    {
        private const ulong kRedEyesBuffGuid = 0xB110CC43B0C44B1C;
        public static ulong StaticGuid
        {
            get
            {
                return 0xB110CC43B0C44B1C;

            }
        }
        public RedEyesBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
