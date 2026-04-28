using System.Collections.Generic;
using FoxShooter.Characters;
using UnityEngine;

namespace FoxShooter.Game
{
    public class PerceptionSubsystem : MonoBehaviour
    {
        public static PerceptionSubsystem instance { get; private set; }

        private readonly List<PerceptionSource> _perceptionSources = new();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
        {
            var gameObject = new GameObject("PerceptionSubsystem");
            DontDestroyOnLoad(gameObject);
            instance = gameObject.AddComponent<PerceptionSubsystem>();
        }

        public void RegisterPerceptionSource(PerceptionSource source)
        {
            _perceptionSources.Add(source);
            source.destroyCancellationToken.Register( _ => _perceptionSources.Remove(source), this);
        }

        public IEnumerable<PerceptionSource> GetSources()
        {
            return _perceptionSources;
        }
    }
}