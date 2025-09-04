using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    private Vector2Int _roomPos;

    public void Init(Vector2Int roomPos)
    {
        _roomPos = roomPos;
    }
    
    public Vector2Int GetRoomPos() => _roomPos;
}
