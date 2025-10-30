using System;
using _Game._Scripts.System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CoinController : MonoBehaviour
{
    [SerializeField] private BasePowerUp powerUp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (powerUp == null) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        
        PowerUpSystem.Instance.SetPowerUp(powerUp);
        PowerUpSystem.Instance.PerformPowerUp();
        
        Destroy(gameObject);
    }
}
