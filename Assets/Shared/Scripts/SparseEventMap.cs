using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace FoxShooter.Scripts
{
    public class SparseEventMap<TKey, TCallback>
    {
        private readonly Dictionary<TKey, UnityEvent<TCallback>> _map = new ();

        public void RegisterCallback(TKey key, UnityAction<TCallback> callback)
        {
            if (_map.TryGetValue(key, out var callbacks))
            {
                callbacks.AddListener(callback);
            }
            else
            {
                var newEvent = new UnityEvent<TCallback>();
                newEvent.AddListener(callback);
                _map.Add(key, newEvent);
            }
        }

        public void TriggerEvent(TKey key, TCallback argument)
        {
            if (!_map.TryGetValue(key, out var craterEvent))
            {
                return;
            }
            
            craterEvent.Invoke(argument);
        }

        public bool RemoveCallback(TKey key, UnityAction<TCallback> callback)
        {
            if (!_map.TryGetValue(key, out var callables))
            {
                return false;
            }

            callables.RemoveListener(callback);
            if (callables.GetPersistentEventCount() == 0)
            {
                _map.Remove(key);
            }

            return true;
        }

        public IEnumerable<TKey> GetMappedEvents()
        {
            return _map.Keys;
        }
    }
    
    public class SparseEventMap<TKey>
    {
        private readonly Dictionary<TKey, UnityEvent> _map = new ();

        public void RegisterCallback(TKey key, UnityAction callback)
        {
            if (_map.TryGetValue(key, out var callbacks))
            {
                callbacks.AddListener(callback);
            }
            else
            {
                var newEvent = new UnityEvent();
                newEvent.AddListener(callback);
                _map.Add(key, newEvent);
            }
        }

        public void TriggerEvent(TKey key)
        {
            if (!_map.TryGetValue(key, out var craterEvent))
            {
                return;
            }
            
            craterEvent.Invoke();
        }

        public bool RemoveCallback(TKey key, UnityAction callback)
        {
            if (!_map.TryGetValue(key, out var callables))
            {
                return false;
                
            }
            callables.RemoveListener(callback);
            
            if (callables.GetPersistentEventCount() == 0)
            {
                _map.Remove(key);
            }

            return true;
        }

        public IEnumerable<TKey> GetMappedEvents()
        {
            return _map.Keys;
        }
    }
    
    public class SparseEventMap<TKey, T0, T1>
    {
        private readonly Dictionary<TKey, UnityEvent<T0, T1>> _map = new ();

        public void RegisterCallback(TKey key, UnityAction<T0, T1> callback)
        {
            if (_map.TryGetValue(key, out var callbacks))
            {
                callbacks.AddListener(callback);
            }
            else
            {
                var newEvent = new UnityEvent<T0, T1>();
                newEvent.AddListener(callback);
                _map.Add(key, newEvent);
            }
        }

        public void TriggerEvent(TKey key, T0 arg0, T1 arg1)
        {
            if (!_map.TryGetValue(key, out var craterEvent))
            {
                return;
            }
            
            craterEvent.Invoke(arg0, arg1);
        }

        public bool RemoveCallback(TKey key, UnityAction<T0, T1> callback)
        {
            if (!_map.TryGetValue(key, out var callables))
            {
                return false;
                
            }

            callables.RemoveListener(callback);
            if (callables.GetPersistentEventCount() == 0)
            {
                _map.Remove(key);
            }

            return true;
        }

        public IEnumerable<TKey> GetMappedEvents()
        {
            return _map.Keys;
        }
    }
}