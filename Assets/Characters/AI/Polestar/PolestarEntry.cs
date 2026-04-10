using System.Collections.Generic;
using Characters.AI.Polestar;
using UnityEngine;

namespace FoxShooter.Characters.AI.Polestar
{
    public abstract class PolestarEntry : ScriptableObject
    {
        public abstract void Evaluate(ref List<PolestarResult> results, CharacterStats self,
            CharacterStats other);
    }
}