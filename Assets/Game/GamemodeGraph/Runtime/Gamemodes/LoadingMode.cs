using System;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public class LoadingMode : MenuMode
    {
        [SerializeField] public LevelDefinition level;

        private SceneReference _nextScene;
        
        public override void Enter()
        {
            base.Enter();

            _nextScene = level ? level.scene : Game.instance.nextLevel.scene;

            var buildIndex = _nextScene.BuildIndex;
            if (buildIndex < 0)
            {
                Debug.Log("cant load scene, index is -1");
                return;
            }
            
            Game.instance.DestroyPlayer();
            var result = SceneManager.LoadSceneAsync(_nextScene.BuildIndex);
            if (result == null)
            {
                return;
            }
            
            result.completed += SceneLoaded;
        }

        private void SceneLoaded(AsyncOperation asyncOperation)
        {
            if (!asyncOperation.isDone)
            {
                Debug.Log("Failed to load scene");
                return;
            }

            SceneManager.LoadScene(_nextScene.BuildIndex);
            Game.instance.currentLevel = level;
            Game.instance.Command(GamemodeTransitionFlag.Loaded);
        }
    }
}