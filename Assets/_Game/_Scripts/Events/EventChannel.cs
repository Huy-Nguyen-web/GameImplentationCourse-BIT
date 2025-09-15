#nullable enable
using EditorAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Scriptable object for event channel. Use RaiseEvent to Invoke the context
/// </summary>
/// <typeparam name="T">EventContext: Custom context to invoke</typeparam>
public abstract class BaseEventChannelSO<T> : ScriptableObject where T : EventContext
{
    public UnityAction<T> OnEventRaised;
    public UnityAction<BaseEventChannelSO<T>> OnFindEventRaised;
    
    public void RaiseEvent(T? context)
    {
        OnEventRaised?.Invoke(context);
    }

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke(null);
    }

    [Button]
    public void FindEventChannelListener()
    {
        OnFindEventRaised?.Invoke(this);
    }
}

/// <summary>
/// Listener to listen to a certain event
/// </summary>
/// <typeparam name="TEventChannel">Event Channel SO</typeparam>
/// <typeparam name="TEventContext">Event Context</typeparam>
[ExecuteAlways]
public abstract class BaseEventChannelListener<TEventChannel, TEventContext> : MonoBehaviour
    where TEventChannel : BaseEventChannelSO<TEventContext> where TEventContext : EventContext
{
    [SerializeField] protected TEventChannel eventChannel;
    [SerializeField] protected UnityEvent<TEventContext> onResponse;

    protected void Awake()
    {
        if (eventChannel == null) return;
        eventChannel.OnFindEventRaised += OnFindEventRaised;
    }

    protected void Destroy()
    {
        if (eventChannel == null) return;
        eventChannel.OnFindEventRaised -= OnFindEventRaised;
    }

    protected virtual void OnEnable()
    {
        if(eventChannel == null) return;
        eventChannel.OnEventRaised += OnEventRaised;
    }

    protected virtual void OnDisable()
    {
        if(eventChannel == null) return;
        eventChannel.OnEventRaised -= OnEventRaised;
    }

    protected virtual void OnEventRaised(TEventContext ctx)
    {
        onResponse?.Invoke(ctx);
    }
    
#if UNITY_EDITOR
    private void OnFindEventRaised(BaseEventChannelSO<TEventContext> arg0)
    {
        Debug.Log($"{nameof(OnFindEventRaised)}: {arg0}", transform);
        EditorGUIUtility.PingObject(transform);
    }
#endif
}

/// <summary>
/// Custom event context.
/// Inherit from this class to create a custom event for event channel
/// </summary>
public abstract class EventContext { }
