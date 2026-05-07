using System.Collections.Generic;
using System.Linq;
using FoxShooter.Characters.AI.StateTree.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    public class StateDragManipulator : PointerManipulator
    {
        private static State _selectedState;

        private bool enabled { get; set; }
        
        private bool _active;

        private StateView _stateView;
        private StateTreeView _root;
        
        public StateDragManipulator(StateView target, StateTreeView root)
        {
            this.target = target.stateContainer;
            _stateView = target;
            _root = root;
            // activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            enabled = true;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private void OnPointerDown(PointerDownEvent pointerDownEvent)
        {
            _active = true;
            
            target.CapturePointer(pointerDownEvent.pointerId);
        }

        private void OnPointerUp(PointerUpEvent pointerUpEvent)
        {
            if (enabled && target.HasPointerCapture(pointerUpEvent.pointerId))
            {
                target.ReleasePointer(pointerUpEvent.pointerId);
            }
            _active = false;
            
            var state = FindState(pointerUpEvent.position);
            if (state == null)
            {
                return;
            }
            
            Debug.Log($"Dropping '{_stateView.state.name}' on '{state.state.name}'");
            _root?.MoveParent(_stateView, state);
        }

        private StateView FindState(Vector3 pointerPosition)
        {
            return _root.Query<StateView>().ToList().FirstOrDefault(view => view.stateContainer.worldBound.Contains(pointerPosition));
        }
    }
}