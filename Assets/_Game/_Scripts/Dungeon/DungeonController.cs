using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using UnityEngine;

public class DungeonController : Singleton<DungeonController>
{
    public enum DungeonRoomType
    {
        None,
        T, // Top
        B, // Bottom
        L, // Left
        R, // Right
        TL, // Top Left
        TR, // Top Right
        BL, // Bottom Left
        BR, // Bottom Right
        TBL, // Top Bottom Left
        TBR, // Top Bottom Right
        TLR, // Top Left Right
        BLR, // Bottom Left Right
        TBLR, // Top Bottom Left Right
    }

    private enum MoveOptions
    {
        Left,
        Right,
        Bottom,
    }
    
    [Header("Setup")]
    [SerializedDictionary] public SerializedDictionary<DungeonRoomType, DungeonRoom> roomsPrefab;
    [SerializeField] private Vector2Int dungeonSize;

    private readonly DungeonRoom[,] _rooms = new DungeonRoom[4, 4];

    public DungeonRoom SpawnRoom(DungeonRoomType dungeonRoomType)
    {
        DungeonRoom room = Instantiate(roomsPrefab[dungeonRoomType], transform);
        return room;
    }

    private void GenerateRoom(Vector2Int currentPos, MoveOptions previousMove, bool isMakingPath, bool isDone)
    {
        if (isMakingPath)
        {
            List<MoveOptions> moves = new List<MoveOptions>()
            {
                MoveOptions.Left,
                MoveOptions.Right,
                MoveOptions.Bottom,
            };

            if (currentPos.x == 0 || previousMove == MoveOptions.Right) moves.Remove(MoveOptions.Left);
            if (currentPos.x == dungeonSize.x - 1 || previousMove == MoveOptions.Left) moves.Remove(MoveOptions.Right);
            
            var currentMove = moves[Random.Range(0, moves.Count)];
            Vector2Int nextPos;
            
            _rooms[currentPos.x, currentPos.y] = SpawnRoom(DungeonRoomType.B);
            switch (currentMove)
            {
                case MoveOptions.Left:
                    nextPos = new Vector2Int(currentPos.x - 1, currentPos.y);
                    break;
                case MoveOptions.Right:
                    nextPos = new Vector2Int(currentPos.x + 1, currentPos.y);
                    break;
                case MoveOptions.Bottom:
                    break;
            }
        }
        
        else
        {
            
        }
    }
}
