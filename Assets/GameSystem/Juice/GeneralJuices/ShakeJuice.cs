using DG.Tweening;
using EditorAttributes;
using UnityEngine;

namespace GameSystem.Juice.GeneralJuices
{
    public class ShakeJuice : BaseGameJuice
    {
        private void Awake()
        {
            CurrentTween = Target.DOShakeScale(duration, 1.1f);
            CurrentTween.OnComplete(() => OnComplete?.Invoke());
        }
    }
}
