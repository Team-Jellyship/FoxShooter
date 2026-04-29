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

            context.AddInputPort<LevelDefinition>(ScenePinName).Build();
        }

        public LevelDefinition GetScene()
        {
            return GetInputPortByName(ScenePinName).TryGetValue<LevelDefinition>(out var scene) ? scene : null;
        }
    }
}