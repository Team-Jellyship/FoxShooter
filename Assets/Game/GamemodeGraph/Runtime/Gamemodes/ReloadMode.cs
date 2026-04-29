using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    public class ReloadMode : MenuMode
    {
        private string _nextSceneName;
        
        public override void Enter()
        {
            base.Enter();
            Game.instance.DestroyPlayer();
            _nextSceneName = SceneManager.GetActiveScene().name;
            var result = SceneManager.LoadSceneAsync(_nextSceneName);
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

            SceneManager.LoadScene(_nextSceneName);
            Game.instance.Command(GamemodeTransitionFlag.Loaded);
        }
    }
}