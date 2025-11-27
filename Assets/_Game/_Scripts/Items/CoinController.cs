using System;
using _Game._Scripts.System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

[RequireComponent(typeof(BoxCollider2D))]
public class CoinController : MonoBehaviour
{
    [SerializeField] private BasePowerUp powerUp;
    [SerializeField] private SpriteRenderer coinSprite;
    [SerializeField, Range(0.5f, 3f)] private float animationTime;
    
    private Sequence _sequence;

    private void Start()
    {
        SetupSequence();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (powerUp == null) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        
        PowerUpSystem.Instance.SetPowerUp(powerUp);
        PowerUpSystem.Instance.PerformPowerUp();

        _sequence.Play();
    }

    private void SetupSequence()
    {
        _sequence = DOTween.Sequence();
        _sequence.SetEase(Ease.InOutSine);
        
        _sequence.Append(transform.DOLocalMoveY(0.5f, animationTime));
        _sequence.Join(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), animationTime));
        _sequence.Join(coinSprite.DOFade(0f, animationTime));
        
        _sequence.OnComplete(() => Destroy(gameObject));
    }
}
