using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private DungeonController dungeonController;

    public UnityAction<bool> onPlayerAtDoor;

    public int level;

    public void GoToNextLevel()
    {
        level++;
        dungeonController.ResetDungeon();
        onPlayerAtDoor?.Invoke(false);
    }
}
