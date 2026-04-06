using System;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public class LoadingMode : MenuMode
    {
        [SerializeField] public SceneReference sceneToLoad;

        public override void Enter()
        {
            base.Enter();

            var buildIndex = sceneToLoad.BuildIndex;
            if (buildIndex < 0)
            {
                Debug.Log("cant load scene, index is -1");
                return;
            }
            
            var result = SceneManager.LoadSceneAsync(sceneToLoad.BuildIndex);
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

            SceneManager.LoadScene(sceneToLoad.BuildIndex);
            Game.instance.Command(GamemodeTransitionFlag.Loaded);
        }
    }
}