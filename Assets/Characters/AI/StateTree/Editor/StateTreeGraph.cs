using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace Characters.AI.StateTree.Editor
{
    [Graph(AssetExtension)]
    [Serializable]
    public class StateTreeGraph : Graph
    {
        public const string AssetExtension = "stg";

        [MenuItem("Assets/Create/StateTreeGraph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<StateTreeGraph>();
        }
    }
}