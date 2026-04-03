using System;
using System.Collections.Generic;
using FoxShooter.Game.GamemodeGraph.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph
{
    [ScriptedImporter(1, GamemodeGraph.AssetExtension)]
    public class GamemodeGraphImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var gamemodeGraph = GraphDatabase.LoadGraphForImporter<GamemodeGraph>(ctx.assetPath);
            var gamemodeTransitionManager = ScriptableObject.CreateInstance<GamemodeTransitionManager>();
            var gamemodeNodeDictionary = new Dictionary<GamemodeNode, Gamemode>();
            foreach (var node in gamemodeGraph.GetNodes())
            {
                if (node is not GamemodeNode gamemodeNode)
                {
                    continue;
                }
                
                gamemodeNodeDictionary.Add(gamemodeNode, new Gamemode(gamemodeNode.GetModeName()));
            }

            foreach (var node in gamemodeGraph.GetNodes())
            {
                switch (node)
                {
                    case StartNode startNode:
                    {
                        gamemodeTransitionManager.startingGameMode = gamemodeNodeDictionary[startNode.GetNextNode()];
                        break;
                    }

                    case GamemodeNode gamemodeNode:
                    {
                        gamemodeTransitionManager.gamemodes.Add(gamemodeNode.GetModeName());
                        foreach (var block in gamemodeNode.blockNodes)
                        {
                            if (block is not GamemodeTransitionNode transition)
                            {
                                continue;
                            }
                            var currentNode = gamemodeNodeDictionary[gamemodeNode];
                            var transitionType = transition.GetTransitionFlag();
                            var nextNode = gamemodeNodeDictionary[transition.GetNextNode()];

                            if (nextNode == null)
                            {
                                continue;
                            }
                            gamemodeTransitionManager.AddTransition(currentNode, transitionType, nextNode);
                        }
                        break;
                    }
                }
            }
            Debug.Log($"[GamemodeGraphImporter] Loaded modes: '{string.Join(", ", gamemodeTransitionManager.gamemodes)}'");
            Debug.Log($"[GamemodeGraphImporter] Starting mode: '{gamemodeTransitionManager.startingGameMode.name}'");
            ctx.AddObjectToAsset("RuntimeData", gamemodeTransitionManager);
            ctx.SetMainObject(gamemodeTransitionManager);
        }
    }
}