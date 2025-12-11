using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private DungeonController dungeonController;
    
    [Header("UI")]
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private InGameUI inGameUI;

    public bool IsTransition { get; set; } = false;
    public UnityAction<bool> onPlayerAtDoor;
    public int level;

    public void GoToNextLevel()
    {
        StartCoroutine(Transition());
    }


    private IEnumerator Transition()
    {
        Debug.Log("Did it go here?");
        IsTransition = true;
        transitionAnimator.Play("Fade");
        yield return new WaitForSeconds(0.5f);
        IsTransition = false;
        level++;
        inGameUI.UpdateRoomNumberUI();
        dungeonController.ResetDungeon();
        onPlayerAtDoor?.Invoke(false);
    }
}
