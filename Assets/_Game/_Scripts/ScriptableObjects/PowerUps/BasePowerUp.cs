using UnityEngine;


public abstract class BasePowerUp : ScriptableObject
{
    public GenericEventChannelSO GenericEventChannelSO;
    
    public float duration;

    public virtual void Perform()
    {
        GenericEventChannelSO?.RaiseEvent(new Context(true));
    }

    public virtual void Stop()
    {
        GenericEventChannelSO?.RaiseEvent(new Context(false));
    }
}
