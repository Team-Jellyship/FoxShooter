using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using Unity.VisualScripting.TextureAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    public struct TypeSearchEntry
    {
        public TypeSearchEntry(Type type, string name)
        {
            this.type = type;
            this.name = name;
        }
        
        public Type type;
        public string name;
    }
    
    public class TypeSearch : PopupWindowContent
    {
        private const string TypeSearchFilename = "type-search";

        public Action<Type> selected;
            
        private SelectionEntry<Type> _currentlySelectedElement;

        public override VisualElement CreateGUI()
        {
            var visualAsset = Resources.Load<VisualTreeAsset>(TypeSearchFilename);
            var tree = visualAsset.CloneTree();
            tree.AddToClassList("popup");
            
            var stringConverter = new SelectionEntry<TypeSearchEntry>.StringConverter(type => type.name);
            
            foreach (var task in GetTypes())
            {
                var taskSelector = new SelectionEntry<TypeSearchEntry>(task, stringConverter);
                taskSelector.RegisterCallback<MouseDownEvent>(_ => Selected(task));
                tree.Add(taskSelector);
            }
            
            return tree;
        }

        public static List<TypeSearchEntry> GetTypes()
        {
            var types = GetCoreTypes();
            // types.AddRange(GetEnumTypes());
            return types;
        }

        private void Selected(TypeSearchEntry taskType)
        {
            selected?.Invoke(taskType.type);
            editorWindow.Close();
        }

        public static List<TypeSearchEntry> GetCoreTypes()
        {
            return new List<TypeSearchEntry>
            {
                new (typeof(GameObject), "Unity.GameObject"),
                new (typeof(Transform), "Unity.Transform"),
                new (typeof(string), "string"),
                new (typeof(float), "float"),
                new (typeof(int), "int"),
                new (typeof(double), "double"),
                new (typeof(bool), "bool"),
                new (typeof(Vector2), "Vector2"),
                new (typeof(Vector3), "Vector3"),
                new (typeof(Vector4), "Vector4"),
                new (typeof(Vector2Int), "Vector2Int"),
                new (typeof(Vector3Int), "Vector3Int"),
                new (typeof(Color), "Color"),
                
                new (typeof(ScriptableObject), "Unity.ScriptableObject"),
                new (typeof(Texture2D), "Texture2D"),
                new (typeof(Sprite), "Sprite"),
                new (typeof(Material), "Material"),
                new (typeof(AudioClip), "AudioClip"),
                new (typeof(AudioResource), "AudioResource"),
                new (typeof(AnimationClip), "AnimationClip"),
                new (typeof(AudioMixer), "AudioMixer"),
                new (typeof(TextAsset), "TextAsset"),
                new (typeof(ParticleSystem), "ParticleSystem"),
                new (typeof(List<GameObject>), "List<Unity.GameObject"),
                new (typeof(List<string>), "List<string>"),
                new (typeof(List<float>), "List<float>"),
                new (typeof(List<int>), "List<int>"),
                new (typeof(List<double>), "List<double>"),
                new (typeof(List<bool>), "List<bool>"),
                new (typeof(List<Vector2>), "List<Vector2>"),
                new (typeof(List<Vector3>), "List<Vector3>"),
                new (typeof(List<Vector4>), "List<Vector4>"),
                new (typeof(List<Vector2Int>), "List<Vector2Int>"),
                new (typeof(List<Vector3Int>), "List<Vector3Int>"),
                new (typeof(List<Color>), "List<Color>")
            };
        }
    }
}