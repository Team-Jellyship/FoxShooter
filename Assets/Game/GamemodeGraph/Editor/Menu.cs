using System;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph
{
    [Serializable]
    public class Menu : GamemodeNode
    {
        public const string SceneName = "Scene";
        public const string ShowMouseName = "Show Mouse";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);

            context.AddInputPort<GameObject>(SceneName).Build();
            context.AddInputPort<bool>(ShowMouseName).Build();
        }

        public GameObject GetMenuScene()
        {
            return GetInputPortByName(SceneName).TryGetValue<GameObject>(out var scene) ? scene : null;
        }

        public bool GetShowMouse()
        {
            return !GetInputPortByName(ShowMouseName).TryGetValue<bool>(out var result) || result;
        }
    }
}