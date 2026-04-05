using System;
using System.ComponentModel;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace FoxShooter.Game.GamemodeGraph
{
[Graph(AssetExtension)]
[Serializable]
public class GamemodeGraph : Graph
{
    public const string AssetExtension = "gmg";

    [MenuItem("Assets/Create/GamemodeGraph", false)]
    private static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<GamemodeGraph>();
    }
}
}