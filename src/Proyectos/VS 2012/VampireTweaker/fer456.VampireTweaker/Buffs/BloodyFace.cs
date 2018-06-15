using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class BloodyFaceBuff : Buff
    {
        private const ulong kBloodyFaceBuffGuid = 0x1A46435F0559B988;
        public static ulong StaticGuid
        {
            get
            {
                return 0x1A46435F0559B988;

            }
        }
        public BloodyFaceBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}