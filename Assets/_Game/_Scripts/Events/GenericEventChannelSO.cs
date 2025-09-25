using UnityEngine;

[CreateAssetMenu(menuName = "Events/GenericEvent", fileName = "Generic Event")]
public class GenericEventChannelSO : BaseEventChannelSO<Context>
{
}

public class Context : EventContext
{
    public object[] Data { get; }

    /// <summary>
    /// Give the context a purpose
    /// </summary>
    /// <param name="data">the data that you want to send</param>
    public Context(params object[] data)
    {
        Data = data;
    }
}
