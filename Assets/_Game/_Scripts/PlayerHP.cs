using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private Slider hpBar;

    private int health;
    [SerializeField] private int maxHealth;

    private bool isDead;

    private PlayerMovement movementScript;
    [SerializeField] private Animator anim;

    private Rigidbody2D rb;

    [SerializeField] private UnityEvent gainHpEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        hpBar.maxValue = maxHealth;
        movementScript = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = health;
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

    private void TakeDamage()
    {

    }
}
