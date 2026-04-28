using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FoxShooter.Game.Combo
{
    [CreateAssetMenu(fileName = "Combo_New", menuName = "Combo/Combo Table")]
    public class ComboTable : ScriptableObject
    {
        public List<ComboDefinition> combos;
        public List<ComboRank> ranks;

        public bool TryGetValue(string tag, out ComboDefinition result)
        {
            foreach (var combo in combos.Where(combo => combo.tag == tag))
            {
                result = combo;
                return true;
            }
            result = default;
            return false;
        }
    }
}