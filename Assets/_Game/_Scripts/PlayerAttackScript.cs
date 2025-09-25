using System;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] private float recoveryTime;
    [SerializeField] private Transform attackZoneRight;
    [SerializeField] private Transform attackZoneLeft;
    [SerializeField] private GameObject attackHitbox;
    private PlayerHP playerHP;

    public Action playerAction;

    private PlayerMovement movementScript;

    [SerializeField] private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        playerHP = GetComponent<PlayerHP>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && recoveryTime <= 0)
        {
            //playerAction = (playerHP.CheckHealth() > 5) ? Attack : Heal;
            //playerHP.UpdateHealth();
            playerAction?.Invoke();
            playerHP.UpdateHealth(OnPlayerUpdateHealth);
        }

        if (recoveryTime > 0)
        {
            recoveryTime -= 1f * Time.deltaTime;
        }
    }

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

            recoveryTime = 1f;
            anim.SetTrigger("Attack");
        
    }

    private void Heal()
    {
        playerHP.GainHeath();
        playerHP.UpdateHealth();
    }

    private void OnPlayerUpdateHealth(int health)
    {
        if (health < 5) Heal();
        else Attack();
    }
}
