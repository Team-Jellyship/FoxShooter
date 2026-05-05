using System;
using FoxShooter.Characters.AI.StateTree.Graph;
using FoxShooter.Characters.AI.StateTree.UI;
using UnityEditor;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    internal class StateTreeWindow : EditorWindow
    {
        [SerializeField] private Tree asset;

        private void OnEnable()
        {
        }

        static void Open(StateTreeGraph asset)
        {
            
        }

        private static bool IsAssetValid(Tree asset)
        {
            return asset != null;
        }

        [InitializeOnLoadMethod]
        private static void RegisterWindowDelegates()
        {
            StateTreeWindowDelegate.handler = Open;
        }
    }
}