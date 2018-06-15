using Sims3.Gameplay.ActorSystems;
using System;

namespace VT.Buffs
{
    internal class ConvertedByTheVampireBuff : Buff
    {
        private const ulong kConvertedByTheVampireBuffGuid = 0x7A197492362B6540;
        public static ulong StaticGuid
        {
            get
            {
                return 0x7A197492362B6540;

            }
        }
        public ConvertedByTheVampireBuff(Buff.BuffData data)
            : base(data)
        {
        }
    }
}
