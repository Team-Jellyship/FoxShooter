using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoxShooter.Game
{
public class TimerHandle
{
    private readonly WeakReference<TimerEntry> _timer;

    public TimerHandle(TimerEntry timer)
    {
        _timer = new WeakReference<TimerEntry>(timer);
    }

    ~TimerHandle()
    {
        if (_timer.TryGetTarget(out var timer))
        {
            timer.expired = true;
        }
    }

    public bool IsActive()
    {
        if (_timer.TryGetTarget(out var timer))
        {
            return timer.duration >= timer.currentTime;
        }

        return false;
    }

    public void Pause()
    {
        if (_timer.TryGetTarget(out var timer))
        {
            timer.paused = true;
        }
    }

    public void Unpause()
    {
        if (_timer.TryGetTarget(out var timer))
        {
            timer.paused = false;
        }
    }

    public void Start(float duration)
    {
        if (!_timer.TryGetTarget(out var timer))
        {
            return;
        }
        timer.currentTime = 0.0f;
        timer.duration = duration;
        timer.paused = false;
    }

    public float GetRemainingTime()
    {
        if (_timer.TryGetTarget(out var timer))
        {
            return timer.duration - timer.currentTime;
        }

        return -1.0f;
    }
}

public class TimerEntry
{
    public readonly WeakReference<MonoBehaviour> monoOwner;
    public float duration;
    public float currentTime;
    public readonly Action callback;
    public bool paused;
    public bool expired;

    public TimerEntry(WeakReference<MonoBehaviour> monoOwner, Action callback)
    {
        this.monoOwner = monoOwner;
        this.callback = callback;

        currentTime = 0.0f;
        paused = true;
    }
}

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance { get; private set; }
    private readonly List<TimerEntry> _timers = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void OnAfterAssembliesLoaded()
    {
        var container = new GameObject("TimerManager");
        instance = container.AddComponent<TimerManager>();
        DontDestroyOnLoad(container);
    }
    
    public void Update()
    {
        for(var i = _timers.Count - 1; i >= 0; --i)
        {
            var timer = _timers[i];

            if (timer.expired)
            {
                _timers.RemoveAt(i);
            }
            
            if (timer.paused)
            {
                continue;
            }
            
            timer.currentTime += Time.deltaTime;

            if (!(timer.currentTime >= timer.duration))
            {
                continue;
            }
            
            if (timer.monoOwner.TryGetTarget(out var owner) && owner)
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                // This invocation is only once per timer
                timer.callback.Invoke();
            }
        }
    }

    /**
     * <summary>Create a new timer</summary>
     * <param name="owner">Owning script of this timer. The timer's lifetime is tied to this object</param>
     * <param name="callback">Function to be called when timer is over</param>
     */
    public TimerHandle CreateTimer(MonoBehaviour owner, Action callback)
    {
        var newTimer = new TimerEntry(new WeakReference<MonoBehaviour>(owner), callback);
        _timers.Add(newTimer);
        return new TimerHandle(newTimer);
    }
}
}