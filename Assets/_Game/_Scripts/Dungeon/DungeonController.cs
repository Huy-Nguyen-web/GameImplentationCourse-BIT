using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Button _someButton;

    private void Start()
    {
        GenerateRoom();
        PopulateRoom();
    }

    public DungeonRoom SpawnRoom(Vector2Int position)
    {
        DungeonRoom room = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], transform);
        room.transform.position = ((Vector2)room.GetRoomSize() - Vector2.one) * position;
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
            // Prepare move options
            List<MoveOptions> moves = new List<MoveOptions>()
            {
                MoveOptions.Left,
                MoveOptions.Right,
                MoveOptions.Bottom,
            };
            
            // Remove move options in case it move back to previous room, or it hit the max left/right
            if(currentMove == MoveOptions.Left) moves.Remove(MoveOptions.Right);
            if(currentMove == MoveOptions.Right) moves.Remove(MoveOptions.Left);

            if (currentPos.x == 0) moves.Remove(MoveOptions.Left);
            if (currentPos.x == dungeonSize.x - 1) moves.Remove(MoveOptions.Right);
            
            // Spawn the room in current position, if just start spawn the room, then set it to the start room
            _rooms[currentPos.x, currentPos.y] = SpawnRoom(currentPos);
            if (move == 0)
            {
                _startRoom = _rooms[currentPos.x, currentPos.y];
            }
            move++;
            _rooms[currentPos.x, currentPos.y].Init(currentPos);

            // Open the wall tile base on the route that the rooms has spawned
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

            // Close the wall if it hit max left, right, top or bottom
            if(currentPos.y == dungeonSize.y - 1) _rooms[currentPos.x, currentPos.y].CloseDoor(DungeonRoom.DoorType.Top);
            if(currentPos.y == 0) _rooms[currentPos.x, currentPos.y].CloseDoor(DungeonRoom.DoorType.Bottom);
            if(currentPos.x == dungeonSize.x - 1) _rooms[currentPos.x, currentPos.y].CloseDoor(DungeonRoom.DoorType.Right);
            if(currentPos.x == 0) _rooms[currentPos.x, currentPos.y].CloseDoor(DungeonRoom.DoorType.Left);
            
            // Make next room in randomly left, right or down
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
                        // If try to go down when it hit the very bottom, this should be a final room
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

    private void PopulateRoom()
    {
        // Fill up all the room other than the main route
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
                }
            }
        }
    }

    private void DestroyAllRoom()
    {
        for(int i = 0; i < dungeonSize.x; i++)
        {
            for (int j = 0; j < dungeonSize.y; j++)
            {
                if (_rooms[i, j] != null)
                {
                    Destroy(_rooms[i, j].gameObject);
                }
                _rooms[i, j] = null;
            }
        }
    }

    public void ResetDungeon()
    {
        DestroyAllRoom();
        GenerateRoom();
        PopulateRoom();
    }

    private Transform GetPlayerSpawnPoint()
    {
        return _startRoom.GetPlayerSpawnPoint();
    }
}
