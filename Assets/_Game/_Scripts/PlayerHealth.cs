using EditorAttributes;
using System;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        movementScript = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
            health--;
            UpdateHealth(health);
            Debug.Log("tookDamage");
        }
    }

    private void Death()
    {
        isDead = true;
        movementScript.enabled = false;
        rb.linearVelocity = new Vector2(0, 0);
        anim.SetTrigger("Dead");
    }

    [Button]
    public void GainHeath()
    {
        health = maxHealth;
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

    public int CheckHealth() => health;
}
