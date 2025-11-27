using UnityEngine;
using DG.Tweening;

namespace GameSystem.Juice.GeneralJuices
{
    public class FadeJuice : BaseGameJuice
    {
        private Renderer _renderer;
        
        private void Awake()
        {
            if (!Target.TryGetComponent(out _renderer))
            {
                Debug.LogError($"{gameObject.name}: No renderer found on {Target.name}");
                return;
            }

            CurrentTween = _renderer.material.DOFade(0f, duration);
            CurrentTween.OnComplete(() => OnComplete?.Invoke());
        }
    }
}
