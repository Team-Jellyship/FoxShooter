using System;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph
{
    [Serializable]
    public class Menu : GamemodeNode
    {
        public const string SceneName = "Scene";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);

            context.AddInputPort<GameObject>(SceneName).Build();
        }

        public GameObject GetMenuScene()
        {
            return GetInputPortByName(SceneName).TryGetValue<GameObject>(out var scene) ? scene : null;
        }
    }
}