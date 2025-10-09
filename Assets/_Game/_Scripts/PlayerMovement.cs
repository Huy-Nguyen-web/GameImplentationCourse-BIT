using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isGrounded;
    public bool isFacingRight;
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    private SpriteRenderer sprait;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpForce;

    [SerializeField] private Animator anim;

    [SerializeField] private Transform fallPointTransform;
    [SerializeField] private float fallDamageDistance;
    [SerializeField] private bool activateOnce;

    private float _speedMultiplier = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprait = GetComponentInChildren<SpriteRenderer>();
        activateOnce = true;
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }

        if (isGrounded)
        {
            anim.SetBool("Grounded", true);
        }
        else
        {
            anim.SetBool("Grounded", false);
        }

        if(!isGrounded && activateOnce)
        {
            OnGroundLeft();
        }

        if(isGrounded && !activateOnce)
        {
            OnLanded();
        }


    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.x < 0)
        {
            isFacingRight = false;
            sprait.flipX = true;
        }

        if (rb.linearVelocity.x > 0)
        {
            isFacingRight = true;
            sprait.flipX = false;
        }
    }


    public void SetPosition(PositionEventContext context)
    {
        transform.position = context.position;
    }

    private void OnGroundLeft()
    {
        fallPointTransform.position = groundCheck.position;
        activateOnce = false;
    }

    private void OnLanded()
    {
        float distance = Vector2.Distance(transform.position, fallPointTransform.position);
        if (distance > fallDamageDistance)
        {
            Debug.Log("Took fall damage");
        }
        activateOnce = true;
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
    #endregion
}