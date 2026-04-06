using System;
using Eflatun.SceneReference;

namespace FoxShooter.Game.GamemodeGraph
{
    [Serializable]
    public class LoadScene : Menu
    {
        private const string ScenePinName = "scene";
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);

            context.AddInputPort<SceneReference>(ScenePinName).Build();
        }

        public SceneReference GetScene()
        {
            return GetInputPortByName(ScenePinName).TryGetValue<SceneReference>(out var scene) ? scene : null;
        }
    }
}