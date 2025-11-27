using DG.Tweening;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.Juice
{
    [ExecuteInEditMode]
    public abstract class BaseGameJuice : MonoBehaviour
    {
        public Transform Target;
        public UnityEvent OnComplete;
        
        protected Tween CurrentTween;

        public abstract void Play();
        
        public Tween GetTween() => CurrentTween;
        
    }
}
