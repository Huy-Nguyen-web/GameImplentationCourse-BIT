using UnityEngine;

[CreateAssetMenu(fileName = "Position Event", menuName = "Events/Position Event")]
public class PositionEventChannelSO : BaseEventChannelSO<PositionEventContext>
{
}

public class PositionEventContext : EventContext
{
    public Vector3 position;
    public PositionEventContext(Vector3 position)
    {
        this.position = position;
    }
}
