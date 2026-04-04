using UnityEngine;

namespace FoxShooter.Scripts
{
    public static class StarFunctions
    {
        public static void RemoveAllChildren(this GameObject obj)
        {
            foreach (Transform child in obj.transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}