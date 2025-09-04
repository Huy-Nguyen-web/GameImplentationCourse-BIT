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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        //float moveVertical = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(moveHorizontal * moveSpeed, rb.linearVelocityY);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            //rb.gravityScale = 0;
        }
    }

    private void FixedUpdate()
    {
        //if (isGrounded)
        //{
        //    Debug.Log("isgroundede");
        //    rb.gravityScale = 0f;
        //}
        //else
        //{
        //    rb.gravityScale = 3f;
        //}
    }


}