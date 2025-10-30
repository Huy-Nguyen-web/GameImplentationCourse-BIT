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
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (powerUp == null) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        
        PowerUpSystem.Instance.SetPowerUp(powerUp);
        PowerUpSystem.Instance.PerformPowerUp();

        Sequence sequence = DOTween.Sequence();
        sequence.SetEase(Ease.InOutSine);
        
        sequence.Append(transform.DOLocalMoveY(0.5f, animationTime));
        sequence.Join(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), animationTime));
        sequence.Join(coinSprite.DOFade(0f, animationTime));
        
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
