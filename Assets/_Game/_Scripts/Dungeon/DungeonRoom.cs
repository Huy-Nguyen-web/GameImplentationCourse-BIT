using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonRoom : MonoBehaviour
{
    private Vector2Int _roomPos;
    [SerializeField] private Vector2Int _roomSize;

    public enum DoorType
    {
        Top,
        Bottom,
        Left,
        Right,
    }
    
    private List<GameObject> _walls = new ();
    private List<GameObject> _randomTiles = new ();

    [SerializeField] private GameObject roomPrefab;
    [SerializedDictionary] public SerializedDictionary<DoorType, GameObject> _doorsDictionary = new ();

    [Button]
    private void GenerateRoom()
    {
        for (int y = 0; y < _roomSize.y; y++)
        {
            bool generateHorizontalWall = (y == 0 || y == _roomSize.y - 1);

            for (int x = 0; x < _roomSize.x; x++)
            {
                if (generateHorizontalWall)
                {
                    GameObject wall = Instantiate(roomPrefab, new Vector2(x, y), Quaternion.identity, transform);
                    _walls.Add(wall);
                }
                else
                {
                    if (x == 0 || x == _roomSize.x - 1)
                    {
                        GameObject wall = Instantiate(roomPrefab, new Vector2(x, y), Quaternion.identity, transform);
                        _walls.Add(wall);
                    }
                }
            }
        }
    }

    [Button]
    private void RemoveRooms()
    {
        foreach (var wall in _walls)
        {
            DestroyImmediate(wall);
        }
    }

    [Button]
    private void GenerateRandomTile()
    {
        int randomTileAmount = 3;
        for (int i = 0; i < randomTileAmount; i++)
        {
            GameObject tile = Instantiate(roomPrefab, new Vector2(Random.Range(1, _roomSize.x-1), Random.Range(1, _roomSize.y-1)), Quaternion.identity, transform);
            _randomTiles.Add(tile);
        }
    }

    [Button]
    private void RemoveAllRandomTiles()
    {
        foreach (var tile in _randomTiles)
        {
            DestroyImmediate(tile);
        }
    }

    public void Init(Vector2Int roomPos)
    {
        _roomPos = roomPos;
        OpenRandomDoor(2);
    }

    public void Init(Vector2Int roomPos, DoorType doorType)
    {
        _roomPos = roomPos;
        OpenDoor(doorType);
        // Maximum another 2 different doors to open
        OpenRandomDoor(2);
    }

    private void OpenRandomDoor()
    {
        int numberOfDoorsToOpen = Random.Range(0, 4);
        
        for (int i = 0; i < numberOfDoorsToOpen; i++)
        {
            OpenDoor(_doorsDictionary.Keys.ToArray()[Random.Range(0, _doorsDictionary.Count)]);
        }
    }

    private void OpenRandomDoor(int numberOfDoorsToOpen)
    {
        for (int i = 0; i < numberOfDoorsToOpen; i++)
        {
            OpenDoor(_doorsDictionary.Keys.ToArray()[Random.Range(0, _doorsDictionary.Count)]);
        }
    }

    /// <summary>
    /// Open door at this position
    /// </summary>
    /// <param name="doorType"></param>
    public void OpenDoor(DoorType doorType)
    {
        _doorsDictionary[doorType].SetActive(false);
    }

    /// <summary>
    /// Close door at this position
    /// </summary>
    /// <param name="doorType"></param>
    public void CloseDoor(DoorType doorType)
    {
        _doorsDictionary[doorType].SetActive(true);
    }
    
    public Vector2Int GetRoomPos() => _roomPos;
    public Vector2Int GetRoomSize() => _roomSize;
}
