using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] private float recoveryTime;
    [SerializeField] private Transform attackZoneRight;
    [SerializeField] private Transform attackZoneLeft;
    [SerializeField] private GameObject attackHitbox;

    private PlayerMovement movementScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && recoveryTime <= 0)
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
        }

        if (recoveryTime > 0)
        {
            recoveryTime -= 1f * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        
    }
}
