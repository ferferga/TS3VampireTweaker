using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Skills;
using System;
namespace VT.Skills
{
    public class VampireSkill : Skill
    {
        public VampireSkill(SkillNames guid)
            : base(guid)
        {
        }
        private VampireSkill()
        {
        }
    }
    public class UsesClass : GameObject
    {
        private const SkillNames VampireSkillGuid = (SkillNames)0x11F10A98;

        public UsesClass()
        {
        }
    }
}

