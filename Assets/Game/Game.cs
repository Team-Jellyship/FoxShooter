using UnityEngine;

namespace FoxShooter.Game
{
    public class Game 
    {
        public static Game instance { get; private set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
        {
            instance = new Game();
        }
    }
}