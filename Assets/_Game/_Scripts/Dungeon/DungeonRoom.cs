using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Vector2Int roomSize;
    [SerializeField] private GameObject roomPrefab;
    [SerializedDictionary] public SerializedDictionary<DoorType, GameObject> doorsDictionary = new ();
    [SerializeField] private Transform enemyPrefab; // Change this one to Enemy type later
    [SerializeField] private Transform coinPrefab; // Change this one to Coin type later
    [SerializeField] private Transform doorPrefab;

    [Header("Spawn Points")] 
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Transform[] coinSpawnPoints;
    [SerializeField] private Transform doorSpawnPoint;

    [Header("Spawn chances")]
    [SerializeField, Range(0.0f, 1.0f)] private float enemySpawnChance = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] private float coinSpawnChance = 0.5f;


    public enum DoorType
    {
        Top,
        Bottom,
        Left,
        Right,
    }
    
    private readonly List<GameObject> _walls = new ();
    private readonly List<GameObject> _randomTiles = new ();

    private readonly List<Transform> _enemies = new();
    private readonly List<Transform> _coins = new();

    private Vector2Int _roomPos;

    [Button]
    private void GenerateRoom()
    {
        for (int y = 0; y < roomSize.y; y++)
        {
            bool generateHorizontalWall = (y == 0 || y == roomSize.y - 1);

            for (int x = 0; x < roomSize.x; x++)
            {
                if (!generateHorizontalWall && x != 0 && x != roomSize.x - 1) continue;
                GameObject wall = Instantiate(roomPrefab, new Vector2(x, y), Quaternion.identity, transform);
                _walls.Add(wall);
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
            GameObject tile = Instantiate(roomPrefab, new Vector2(Random.Range(1, roomSize.x-1), Random.Range(1, roomSize.y-1)), Quaternion.identity, transform);
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

    /// <summary>
    /// Initialize the room
    /// </summary>
    /// <param name="roomPos">The position of the room</param>
    public void Init(Vector2Int roomPos)
    {
        _roomPos = roomPos;
        Debug.Log($"Enemy Spawn Chance {enemySpawnChance}");
        OpenRandomDoor(3);
        SpawnRandomEnemy();
        SpawnRandomCoin();
    }

    /// <summary>
    /// Open random door from top, bottom, left, right
    /// Default: 4 doors
    /// </summary>
    private void OpenRandomDoor()
    {
        int numberOfDoorsToOpen = Random.Range(0, 4);
        
        for (int i = 0; i < numberOfDoorsToOpen; i++)
        {
            OpenDoor(doorsDictionary.Keys.ToArray()[Random.Range(0, doorsDictionary.Count)]);
        }
    }

    /// <summary>
    /// Open random door from top, bottom, left, right
    /// If the same door is trying to open, it'll skip
    /// So there're always numberOfDoorsToOpen - 1 door to open
    /// </summary>
    /// <param name="numberOfDoorsToOpen">number of random door to open.</param>
    private void OpenRandomDoor(int numberOfDoorsToOpen)
    {
        for (int i = 0; i < numberOfDoorsToOpen; i++)
        {
            OpenDoor(doorsDictionary.Keys.ToArray()[Random.Range(0, doorsDictionary.Count)]);
        }
    }

    /// <summary>
    /// Open door at this position
    /// </summary>
    /// <param name="doorType"></param>
    public void OpenDoor(DoorType doorType)
    {
        doorsDictionary[doorType].SetActive(false);
    }

    /// <summary>
    /// Close door at this position
    /// </summary>
    /// <param name="doorType"></param>
    public void CloseDoor(DoorType doorType)
    {
        doorsDictionary[doorType].SetActive(true);
    }

    /// <summary>
    /// Spawn random enemy in the room
    /// </summary>
    private void SpawnRandomEnemy()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            float randomChance = Random.Range(0f, 1f);
            Debug.Log(randomChance);
            if (randomChance < enemySpawnChance)
            {
                Debug.Log("Spawn enemy");
                var enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity, enemySpawnPoints[i].transform);
                _enemies.Add(enemy);
            }
        }
    }

    /// <summary>
    /// Spawn coins randomly in the room
    /// </summary>
    private void SpawnRandomCoin()
    {
        for (int i = 0; i < coinSpawnPoints.Length; i++)
        {
            float randomChance = Random.Range(0f, 1f);
            if (randomChance < coinSpawnChance)
            {
                var coin = Instantiate(coinPrefab, coinSpawnPoints[i].position, Quaternion.identity, coinSpawnPoints[i].transform);
                _coins.Add(coin);
            }
        }
    }

    /// <summary>
    /// Spawn random coin in the room
    /// </summary>
    private void SpawnRandomCoins()
    {
        for (int i = 0; i < coinSpawnPoints.Length; i++)
        {
            float randomChance = Random.Range(0f, 1f);
            if (randomChance > coinSpawnChance)
            {
                var coin = Instantiate(coinPrefab, transform);
                _coins.Add(coin);
            }
        }
    }

    public void SpawnDoor()
    {
        Vector3 offset = new Vector3(0.25f, -0.125f, 0f);
        var door = Instantiate(doorPrefab, doorSpawnPoint.transform.position + offset, Quaternion.identity);
        door.transform.SetParent(doorSpawnPoint);
    }
    
    public Vector2Int GetRoomPos() => _roomPos;
    public Vector2Int GetRoomSize() => roomSize;

    public Transform GetPlayerSpawnPoint() => playerSpawnPoint;
    public Transform GetDoorSpawnPoint() => doorSpawnPoint;
}
