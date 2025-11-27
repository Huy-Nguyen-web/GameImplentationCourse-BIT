using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.Juice
{
    public class GameJuiceCombiner : MonoBehaviour
    {
        public enum CombinerType
        {
            Sequence,
            Parallel,
        }

        public UnityEvent OnComplete;
        [SerializeField] private List<BaseGameJuice> gameJuices = new ();
        [SerializeField] private CombinerType combinerType = CombinerType.Sequence;
        
        private Sequence _sequence;

        private void Start()
        {
            SetupJuice();
        }

        private void SetupJuice()
        {
            _sequence = DOTween.Sequence();
            if (gameJuices.Count == 0) return;
            
            for (int i = 0; i < gameJuices.Count; i++)
            {
                if (i == 0)
                {
                    _sequence.Append(gameJuices[i].GetTween());
                    continue;
                }

                switch (combinerType)
                {
                    case CombinerType.Sequence:
                        _sequence.Append(gameJuices[i].GetTween());
                        break;
                    case CombinerType.Parallel:
                        _sequence.Join(gameJuices[i].GetTween());
                        break;
                    default:
                        break;
                }
            }
            _sequence.AppendCallback(() =>
            {
                OnComplete?.Invoke();
            });
        }

        #if UNITY_EDITOR
        [Button]
        public void GetAllGameJuices()
        {
            gameJuices = GetComponentsInChildren<BaseGameJuice>().ToList();
        }

        [Button]
        public void RemoveAllGameJuices()
        {
            gameJuices.Clear();
        }
        #endif

        public void Play() => _sequence.Play();
        public bool IsPlaying() => _sequence.IsPlaying();
    }
}
