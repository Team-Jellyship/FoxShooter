using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using BoundsField = UnityEngine.UIElements.BoundsField;
using BoundsIntField = UnityEngine.UIElements.BoundsIntField;
using ColorField = Unity.AppUI.UI.ColorField;
using DoubleField = UnityEngine.UIElements.DoubleField;
using FloatField = UnityEngine.UIElements.FloatField;
using LongField = UnityEngine.UIElements.LongField;
using RectField = UnityEngine.UIElements.RectField;
using RectIntField = UnityEngine.UIElements.RectIntField;
using Vector2Field = UnityEngine.UIElements.Vector2Field;
using Vector2IntField = UnityEngine.UIElements.Vector2IntField;
using Vector3Field = UnityEngine.UIElements.Vector3Field;
using Vector3IntField = UnityEngine.UIElements.Vector3IntField;
using Vector4Field = UnityEngine.UIElements.Vector4Field;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class GenericField : VisualElement
    {
        public Action<object> dataChanged;
        
        private object _data;
        private Type _dataType;
        private VisualElement _fieldEditor;


        public GenericField()
        {
            _fieldEditor = new TextField
            {
                name = "generic-field-input",
                value = "default"
            };
            Add(_fieldEditor);
        }

        public GenericField(Type type, object data, string label = "")
        {
            if (type == typeof(int))
            {
                _fieldEditor = new IntegerField { value = (int)data, label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<int>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(float))
            {
                _fieldEditor = new FloatField { value = (float)(data ?? 0.0f), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<float>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(long))
            {
                _fieldEditor = new LongField { value = (long)(data ?? 0.0), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<long>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(double))
            {
                _fieldEditor = new DoubleField { value = (double)(data ?? 0.0), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<double>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(string))
            {
                _fieldEditor = new TextField { value = (string)(data ?? ""), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<string>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Hash128))
            {
                _fieldEditor = new Hash128Field { value = (Hash128)data, label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Hash128>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Vector2))
            {
                _fieldEditor = new Vector2Field { value = (Vector2)(data ?? Vector2.zero), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Vector2>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Vector3))
            {
                _fieldEditor = new Vector3Field { value = (Vector3)(data ?? Vector3.zero), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Vector3>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Vector4))
            {
                _fieldEditor = new Vector4Field { value = (Vector4)(data ?? Vector4.zero), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Vector4>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Rect))
            {
                _fieldEditor = new RectField { value = (Rect)(data ?? Rect.zero), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Rect>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Bounds))
            {
                _fieldEditor = new BoundsField { value = (Bounds)(data ?? new Bounds()), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Bounds>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(uint))
            {
                _fieldEditor = new UnsignedIntegerField { value = (uint)(data ?? 0U), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<uint>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(ulong))
            {
                _fieldEditor = new UnsignedLongField { value = (ulong)(data ?? 0), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<ulong>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Vector2Int))
            {
                _fieldEditor = new Vector2IntField { value = (Vector2Int)(data ?? Vector2Int.zero), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Vector2Int>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Vector3Int))
            {
                _fieldEditor = new Vector3IntField { value = (Vector3Int)(data ?? Vector3Int.zero), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Vector3Int>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(RectInt))
            {
                _fieldEditor = new RectIntField { value = (RectInt)(data ?? RectInt.zero), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<RectInt>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(BoundsInt))
            {
                _fieldEditor = new BoundsIntField { value = (BoundsInt)(data ?? new BoundsInt()), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<BoundsInt>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Color))
            {
                _fieldEditor = new ColorField();
                _fieldEditor.RegisterCallback<ChangeEvent<Color>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(AnimationCurve))
            {
                _fieldEditor = new CurveField { value = (AnimationCurve)(data ?? new AnimationCurve()), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<AnimationCurve>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type == typeof(Gradient))
            {
                _fieldEditor = new GradientField { value = (Gradient)(data ?? new Gradient()), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Gradient>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type.IsSubclassOf(typeof(Enum)))
            {
                _fieldEditor = new EnumField { value = (Enum)(data ?? 0), label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<Enum>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            else if (type.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                _fieldEditor = new ObjectField { objectType = type, value = (UnityEngine.Object)data, label = label };
                _fieldEditor.RegisterCallback<ChangeEvent<UnityEngine.Object>>(value =>
                {
                    dataChanged?.Invoke(value.newValue);
                });
            }
            if (_fieldEditor == null)
            {
                return;
            }
            _fieldEditor.name = "generic-field-input";
            Add(_fieldEditor);
        }
    }
}