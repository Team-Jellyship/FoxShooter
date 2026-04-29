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
            var gamemodeTransitionManager = ScriptableObject.CreateInstance<GamemodeTransitionData>();
            gamemodeTransitionManager.gamemodes = new List<Gamemode>();
            gamemodeTransitionManager.transitionEntries = new List<GamemodeTransitionEntry>();
            var gamemodeNodeDictionary = new Dictionary<GamemodeNode, Gamemode>();
            foreach (var node in gamemodeGraph.GetNodes())
            {
                if (node is not GamemodeNode gamemodeNode)
                {
                    continue;
                }

                var gamemode = CreateGamemodeFromNode(node);
                gamemodeNodeDictionary.Add(gamemodeNode, gamemode);
                gamemodeTransitionManager.gamemodes.Add(gamemode);
            }

            foreach (var node in gamemodeGraph.GetNodes())
            {
                switch (node)
                {
                    case StartNode startNode:
                    {
                        gamemodeTransitionManager.startingGameModeId = gamemodeNodeDictionary[startNode.GetNextNode()].id;
                        break;
                    }

                    case GamemodeNode gamemodeNode:
                    {
                        foreach (var block in gamemodeNode.blockNodes)
                        {
                            if (block is not GamemodeTransitionNode transition)
                            {
                                continue;
                            }
                            var currentNode = gamemodeNodeDictionary[gamemodeNode];
                            var transitionType = transition.GetTransitionFlag();
                            var nextNode = transition.GetNextNode();
                            if (transition.GetNextNode() == null)
                            {
                                continue;
                            }
                            
                            var nextGamemode = gamemodeNodeDictionary[transition.GetNextNode()];

                            if (nextNode == null)
                            {
                                continue;
                            }
                            gamemodeTransitionManager.AddTransition(currentNode, transitionType, nextGamemode);
                        }
                        break;
                    }
                }
            }
            ctx.AddObjectToAsset("RuntimeData", gamemodeTransitionManager);
            ctx.SetMainObject(gamemodeTransitionManager);
        }

        private static Gamemode CreateGamemodeFromNode(INode node)
        {
            Gamemode mode;
            switch (node)
            {
                case LoadScene loading:
                {
                    mode = new LoadingMode
                    {
                        name = loading.GetModeName(),
                        time = loading.GetModeTime(),
                        scene = loading.GetMenuScene(),
                        id = Guid.NewGuid().ToString(),
                        level = loading.GetScene()
                    };
                    return mode;
                }
                
                case ReloadScene reload:
                {
                    mode = new ReloadMode
                    {
                        name = reload.GetModeName(),
                        time = reload.GetModeTime(),
                        scene = reload.GetMenuScene(),
                        id = Guid.NewGuid().ToString(),
                        showMouse = reload.GetShowMouse()
                    };
                    return mode;
                }
                
                case Menu menu:
                {
                    mode = new MenuMode
                    {
                        name = menu.GetModeName(),
                        time = menu.GetModeTime(),
                        scene = menu.GetMenuScene(),
                        id = Guid.NewGuid().ToString(),
                        showMouse = menu.GetShowMouse()
                    };
                    return mode;
                }
                
                case GamemodeNode gamemode:
                {
                    mode = new Gamemode
                    {
                        name = gamemode.GetModeName(),
                        time = gamemode.GetModeTime(),
                        id = Guid.NewGuid().ToString()
                    };
                    return mode;
                }
                default:
                    return null;
            }
        }
    }
}