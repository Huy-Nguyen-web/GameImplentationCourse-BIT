using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Serialized stuff")]
    [Tooltip("How much delay when you can attack again")]
    [SerializeField] private float recoveryTime;
    [SerializeField] private Transform attackZoneRight;
    [SerializeField] private Transform attackZoneLeft;
    [SerializeField] private GameObject attackHitbox;
    private PlayerHealth playerHealth;

    public Action playerAction;

    private PlayerMovement movementScript;

    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource attackSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && recoveryTime <= 0)
        {
            playerAction?.Invoke();
            playerHealth.UpdateHealth(OnPlayerUpdateHealth);
        }

        if (recoveryTime > 0)
        {
            recoveryTime -= 1f * Time.deltaTime;
        }
    }

    [ContextMenu("PlayerAttack")]
    private void Attack()
    {
            if (movementScript.isFacingRight)
            {
                Instantiate(attackHitbox, attackZoneRight.position, Quaternion.identity);
            }
            else
            {
                Instantiate(attackHitbox, attackZoneLeft.position, Quaternion.identity);
            }

            recoveryTime = 0.5f;
            anim.SetTrigger("Attack");
        attackSound.Play();
    }

    private void Heal()
    {
        playerHealth.GainHeath();
        playerHealth.UpdateHealth();
    }

    private void OnPlayerUpdateHealth(int health)
    {
        if (health < 5) Heal();
        else Attack();
    }
}
