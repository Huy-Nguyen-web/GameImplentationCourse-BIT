using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonController : Singleton<DungeonController>
{
    private enum MoveOptions
    {
        None,
        Left,
        Right,
        Bottom,
    }

    [Header("Setup")]
    [SerializeField] private DungeonRoom[] roomPrefabs;
    [SerializeField] private Vector2Int dungeonSize;

    [Header("Events")] 
    [SerializeField] private GenericEventChannelSO dungeonFinishEvent;
    [SerializeField] private PositionEventChannelSO playerSetupEvent;

    private DungeonRoom _startRoom;

    private readonly DungeonRoom[,] _rooms = new DungeonRoom[4, 4];

    private void Start()
    {
        GenerateRoom();
        PopulateRoom();
    }

    public DungeonRoom SpawnRoom(Vector2Int position)
    {
        DungeonRoom room = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], transform);
        room.transform.position = ((Vector2)room.GetRoomSize() - Vector2.one) * position;
        Debug.Log($"Spawn room at {position}");
        return room;
    }

    private void GenerateRoom()
    {
        bool done = false;
        Vector2Int currentPos = new Vector2Int(Random.Range(0, dungeonSize.x), dungeonSize.y - 1);
        MoveOptions currentMove = MoveOptions.None;
        int move = 0;
        while (!done)
        {
            List<MoveOptions> moves = new List<MoveOptions>()
            {
                MoveOptions.Left,
                MoveOptions.Right,
                MoveOptions.Bottom,
            };
            
            if(currentMove == MoveOptions.Left) moves.Remove(MoveOptions.Right);
            if(currentMove == MoveOptions.Right) moves.Remove(MoveOptions.Left);

            if (currentPos.x == 0) moves.Remove(MoveOptions.Left);
            if (currentPos.x == dungeonSize.x - 1) moves.Remove(MoveOptions.Right);
            
            _rooms[currentPos.x, currentPos.y] = SpawnRoom(currentPos);
            if (move == 0)
            {
                _startRoom = _rooms[currentPos.x, currentPos.y];
            }
            move++;
            _rooms[currentPos.x, currentPos.y].Init(currentPos);
            switch (currentMove)
            {
                case MoveOptions.Left:
                    _rooms[currentPos.x, currentPos.y].OpenDoor(DungeonRoom.DoorType.Right);
                    break;
                case MoveOptions.Right:
                    _rooms[currentPos.x, currentPos.y].OpenDoor(DungeonRoom.DoorType.Left);
                    break;
                case MoveOptions.Bottom:
                    _rooms[currentPos.x, currentPos.y].OpenDoor(DungeonRoom.DoorType.Top);
                    break;
            }

            if(currentPos.y == dungeonSize.y - 1) _rooms[currentPos.x, currentPos.y].CloseDoor(DungeonRoom.DoorType.Top);
            if(currentPos.y == 0) _rooms[currentPos.x, currentPos.y].CloseDoor(DungeonRoom.DoorType.Bottom);
            if(currentPos.x == dungeonSize.x - 1) _rooms[currentPos.x, currentPos.y].CloseDoor(DungeonRoom.DoorType.Right);
            if(currentPos.x == 0) _rooms[currentPos.x, currentPos.y].CloseDoor(DungeonRoom.DoorType.Left);
            
            currentMove = moves[Random.Range(0, moves.Count)];
            switch (currentMove)
            {
                case MoveOptions.Left:
                    _rooms[currentPos.x, currentPos.y].OpenDoor(DungeonRoom.DoorType.Left);
                    currentPos = new Vector2Int(currentPos.x - 1, currentPos.y);
                    break;
                case MoveOptions.Right:
                    _rooms[currentPos.x, currentPos.y].OpenDoor(DungeonRoom.DoorType.Right);
                    currentPos = new Vector2Int(currentPos.x + 1, currentPos.y);
                    break;
                case MoveOptions.Bottom:
                    if (currentPos.y == 0)
                    {
                        done = true;
                        _rooms[currentPos.x, currentPos.y].SpawnDoor();
                        playerSetupEvent.RaiseEvent(new PositionEventContext(GetPlayerSpawnPoint().position));
                        return;
                    }
                    _rooms[currentPos.x, currentPos.y].OpenDoor(DungeonRoom.DoorType.Bottom);
                    currentPos = new Vector2Int(currentPos.x, currentPos.y - 1);
                    break;
                default:
                    Debug.LogError("Something goes wrong");
                    done = true;
                    break;
            }
        }
    }

    public void PopulateRoom()
    {
        for (int y = dungeonSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < dungeonSize.x; x++)
            {
                if (_rooms[x, y] == null)
                {
                    _rooms[x, y] = SpawnRoom(new Vector2Int(x, y));
                    _rooms[x, y].Init(new Vector2Int(x, y));
                    if(y == dungeonSize.y - 1) _rooms[x, y].CloseDoor(DungeonRoom.DoorType.Top);
                    if(y == 0) _rooms[x, y].CloseDoor(DungeonRoom.DoorType.Bottom);
                    if(x == dungeonSize.x - 1) _rooms[x, y].CloseDoor(DungeonRoom.DoorType.Right);
                    if(x == 0) _rooms[x, y].CloseDoor(DungeonRoom.DoorType.Left);
                    Debug.Log($"Populate room at {new Vector2Int(x, y)}");
                }
            }
        }
    }

    public Transform GetPlayerSpawnPoint()
    {
        return _startRoom.GetPlayerSpawnPoint();
    }
}
