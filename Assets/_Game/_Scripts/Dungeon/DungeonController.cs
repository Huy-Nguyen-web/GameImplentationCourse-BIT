using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using UnityEngine;

public class DungeonController : Singleton<DungeonController>
{
    private enum MoveOptions
    {
        Left,
        Right,
        Bottom,
    }

    [Header("Setup")]
    [SerializeField] private DungeonRoom roomPrefab;
    [SerializeField] private Vector2Int dungeonSize;

    private readonly DungeonRoom[,] _rooms = new DungeonRoom[4, 4];

    public DungeonRoom SpawnRoom(Vector2Int position,
        bool left = false,
        bool right = false,
        bool bottom = false,
        bool top = false)
    {
        DungeonRoom room = Instantiate(roomPrefab, transform);
        Debug.Log($"Spawn room at {position}");
        return room;
    }

    private void GenerateRoom()
    {
        bool done = false;
        Vector2Int currentPos = new Vector2Int(Random.Range(0, dungeonSize.x), Random.Range(0, dungeonSize.y));
        while (!done)
        {
            List<MoveOptions> moves = new List<MoveOptions>()
            {
                MoveOptions.Left,
                MoveOptions.Right,
                MoveOptions.Bottom,
            };

            if (currentPos.x == 0) moves.Remove(MoveOptions.Left);
            if (currentPos.x == dungeonSize.x - 1) moves.Remove(MoveOptions.Right);
            
            var currentMove = moves[Random.Range(0, moves.Count)];
            
            _rooms[currentPos.x, currentPos.y] = SpawnRoom(currentPos);
            switch (currentMove)
            {
                case MoveOptions.Left:
                    currentPos = new Vector2Int(currentPos.x - 1, currentPos.y);
                    break;
                case MoveOptions.Right:
                    currentPos = new Vector2Int(currentPos.x + 1, currentPos.y);
                    break;
                case MoveOptions.Bottom:
                    if (currentPos.y == dungeonSize.y - 1)
                    {
                        done = true;
                        // TODO: Generate the final room here
                        // TODO: Generate the door
                        return;
                    }
                    currentPos = new Vector2Int(currentPos.x, currentPos.y + 1);
                    break;
                default:
                    Debug.LogError("Something goes wrong");
                    done = true;
                    break;
            }
        }
    }
}
