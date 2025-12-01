using EditorAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour
{
    private int health;
    [SerializeField] private int maxHealth;

    private bool isDead;

    private PlayerMovement movementScript;
    [SerializeField] private Animator anim;

    private Rigidbody2D rb;

    [SerializeField] private UnityEvent gainHpEvent;

    [SerializeField] private GenericEventChannelSO healthChangeEventChannel;

    [SerializeField] private AudioSource damageSound;

    [SerializeField] private GameObject gameOverPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        movementScript = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (health <= 0 && !isDead)
        {
            Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isDead)
        {
            if (!collision.TryGetComponent<Enemy>(out var _enemy)) return;
            if (!_enemy.CanAttake) return;
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        health--;
        UpdateHealth(health);
        damageSound.Play();
    }

    private void Death()
    {
        isDead = true;
        movementScript.enabled = false;
        rb.linearVelocity = new Vector2(0, 0);
        anim.SetTrigger("Dead");
        StartCoroutine(DeathDelay());
    }

    [Button]
    public void GainHeath()
    {
        health += (Random.Range(-1, 2) * 2);
        health = Math.Clamp(health, 0, maxHealth);
        UpdateHealth(health);
        gainHpEvent?.Invoke();
    }

    public void UpdateHealth(int health)
    {
        healthChangeEventChannel?.RaiseEvent(new Context(health));
    }

    public void UpdateHealth()
    {
        healthChangeEventChannel?.RaiseEvent(new Context(health));
    }

    public void UpdateHealth(Action<int> action)
    {
        action?.Invoke(health);
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2f);
        gameOverPanel.SetActive(true);
    }

    public int CheckHealth() => health;
}
