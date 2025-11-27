using DG.Tweening;
using EditorAttributes;
using UnityEngine;

namespace GameSystem.Juice.GeneralJuices
{
    public class ShakeJuice : BaseGameJuice
    {
        [SerializeField] private float shakeDuration;
        
        private void Awake()
        {
            CurrentTween = Target.DOShakeScale(shakeDuration, 1.1f).OnComplete(() => OnComplete?.Invoke());
        }
        
        public override void Play()
        {
            Debug.Log("Play Tween");
            CurrentTween.Play();;
        }
        
        #if UNITY_EDITOR
        [Button]
        public void PlayInEditor()
        {
            CurrentTween ??= Target.DOShakeScale(shakeDuration, 1.1f).OnComplete(() => OnComplete?.Invoke());
        }
        #endif

    }
}
