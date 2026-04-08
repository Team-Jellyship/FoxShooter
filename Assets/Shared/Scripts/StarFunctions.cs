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

        public static T GetComponentInRoot<T>(this GameObject obj)
            where T : Component
        {
            return obj.transform.root?.GetComponent<T>();
        }

        public static T GetComponentInRoot<T>(this MonoBehaviour obj)
            where T : Component
        {
            return GetComponentInRoot<T>(obj.gameObject);
        }

        public static T GetComponentInRoot<T>(this Component obj)
            where T : Component
        {
            return GetComponentInRoot<T>(obj.gameObject);
        }
    }
}