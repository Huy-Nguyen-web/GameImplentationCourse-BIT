using UnityEngine;

public class SmoothTransition : MonoBehaviour
{
    [SerializeField] private Animator anim;


    public void StartFadeEffect()
    {
        anim.SetTrigger("Play");
    }


}
