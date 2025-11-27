using DG.Tweening;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.Juice
{
    public abstract class BaseGameJuice : MonoBehaviour
    {
        public Transform Target;
        public UnityEvent OnComplete;
        
        [SerializeField, Range(0f, 5f)] protected float duration;
        
        protected Tween CurrentTween;

        public virtual void Play()
        {
            CurrentTween?.Play();
        }
        
        public Tween GetTween() => CurrentTween;
        
    }
}
