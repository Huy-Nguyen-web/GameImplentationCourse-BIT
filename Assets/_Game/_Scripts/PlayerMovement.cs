using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isGrounded;
    public bool isFacingRight;
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float coyoteTime;

    [SerializeField] private Animator anim;

    [SerializeField] private Transform fallPointTransform;
    [SerializeField] private float fallDamageDistance;
    [SerializeField] private bool activateOnce;

    private float _speedMultiplier = 1.0f;
    private SpriteRenderer _sprite;

    private PlayerHealth playerHPScript;

    private bool _playerAtDoor = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        playerHPScript = GetComponent<PlayerHealth>();
        activateOnce = true;

        GameManager.Instance.onPlayerAtDoor += OnPlayerAtDoor;
    }



    private void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(moveHorizontal * moveSpeed * _speedMultiplier, rb.linearVelocityY);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask);

        if(moveHorizontal != 0)
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && coyoteTime > 0)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Space) && coyoteTime <= 0)
        {
            jumpBufferTime = 0.1f;
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && _playerAtDoor)
        {
            GameManager.Instance.GoToNextLevel();
        }


        if (jumpBufferTime > 0 && coyoteTime > 0)
        {
            Jump();
        }

        if (isGrounded)
        {
            anim.SetBool("Grounded", true);
            coyoteTime = 0.1f;
        }
        else
        {
            anim.SetBool("Grounded", false);
            coyoteTime -= 1 * Time.deltaTime;
        }

        if(!isGrounded && activateOnce)
        {
            OnGroundLeft();
        }

        if(isGrounded && !activateOnce)
        {
            OnLanded();
        }

        if(coyoteTime < 0)
        {
            coyoteTime = 0;
        }

        if(jumpBufferTime >= 0)
        {
            jumpBufferTime -= 1 * Time.deltaTime;
        }

        
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.x < 0)
        {
            isFacingRight = false;
            _sprite.flipX = true;
        }

        if (rb.linearVelocity.x > 0)
        {
            isFacingRight = true;
            _sprite.flipX = false;
        }
    }


    public void SetPosition(PositionEventContext context)
    {
        transform.position = context.position;
    }

    private void OnGroundLeft()
    {
        if (!fallPointTransform) return;
        fallPointTransform.position = groundCheck.position;
        activateOnce = false;
    }

    private void OnLanded()
    {
        if(!fallPointTransform) return;
        float distance = fallPointTransform.position.y - transform.position.y;
        if (distance > fallDamageDistance)
        {
            Debug.Log("Took fall damage");
            playerHPScript.TakeDamage();
        }
        activateOnce = true;
    }

    private void Jump()
    {
        jumpBufferTime = 0;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
    }
    
    #region Events

    public void OnSpeedPowerUp(Context ctx)
    {
        bool powerUpTrigger = false;
        if (ctx != null)
        {
            powerUpTrigger = (bool)ctx.Data[0];
        }
        _speedMultiplier = (powerUpTrigger) ? 2.0f : 1.0f;
    }

    private void OnPlayerAtDoor(bool atDoor)
    {
        _playerAtDoor = atDoor;
    }
    #endregion
}