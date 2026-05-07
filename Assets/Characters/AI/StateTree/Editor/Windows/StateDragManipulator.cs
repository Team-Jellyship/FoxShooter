using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    public class StateDragManipulator : PointerManipulator
    {
        private static State _selectedState;

        private bool _active;
        
        public StateDragManipulator()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
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

        private void OnPointerDown(PointerDownEvent pointerMoveEvent)
        {
            _active = true;
        }

        private void OnPointerUp(PointerUpEvent pointerUpEvent)
        {
            if (_active)
            {
                
            }
            _active = false;
        }
    }
}