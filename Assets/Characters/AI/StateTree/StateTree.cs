using System.Collections.Generic;

namespace FoxShooter.Characters.AI.StateTree
{
    public class StateTree
    {
        private State _root;

        private List<State> _activeStates = new();

        private void Update()
        {
            var treeContext = new TreeContext();
            
            for (var i = _activeStates.Count - 1; i >= 0; --i)
            {
                var state = _activeStates[i];
                var result = state.Update(treeContext);
                if (result != null)
                {
                    CompleteTransition(result);
                }
            }
        }

        private void CompleteTransition(State state)
        {
            while (state != null)
            {
                state = TransitionTo(state);
            }
        }

        private State TransitionTo(State state)
        {
            var newActiveStates = GetHierarchy(state);
            var context = new TreeContext();
            
            var i = _activeStates.Count - 1;
            for (; i >= 0; --i)
            {
                if (i >= newActiveStates.Count || _activeStates[i] == newActiveStates[i])
                {
                    break;
                }
                _activeStates[i].Exit();
                _activeStates.RemoveAt(i);
            }

            ++i;
            for (; i > newActiveStates.Count; ++i)
            {
                var result = newActiveStates[i].Enter(context);
                _activeStates[i].Enter(context);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        private List<State> GetHierarchy(State state)
        {
            var result = new List<State>();
            for (var parent = state; parent != null; parent = parent.parent)
            {
                result.Add(parent);
            }
            return result;
        }
    }
}