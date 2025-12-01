using System.Numerics;
using GameSystem.Juice;
using GameSystem.Juice.GeneralJuices;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    public bool CanAttake { get; private set; } = true;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioSource deathSound;
    
    [Header("Juice")]
    [SerializeField] private GameJuiceCombiner juice;
    private bool _isFacingRight;
    private Vector2 velocity;

    private bool _isOnGround;
    
    void Start()
    {
        _isFacingRight = true;
        juice.OnComplete.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer) && !Physics2D.Raycast(wallCheck.position, Vector2.right, 0.1f, groundLayer))
        {
            
            //keep walking
        }
        else
        {
            _isFacingRight = !_isFacingRight;
            var localScale = transform.localScale;
            localScale.x = (_isFacingRight) ? 1f : -1f;
            transform.localScale = localScale;
        }

        RaycastHit2D hit = (Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer));
        _isOnGround = hit != null;
        if (_isOnGround)
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.5f, transform.position.z);
        }
    }
    private void FixedUpdate()
    {
        velocity.y = (_isOnGround) ? 0 : Physics2D.gravity.y;
        velocity.x = moveSpeed * (_isFacingRight ? 1f : -1f);
        transform.Translate(velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            // Play the juice
            // When the juice done, destroy object
            deathSound.Play();
            CanAttake = false;
            juice.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + (Vector3)(Vector2.down * 0.5f));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * 0.1f));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Vector2.down * 0.5f));
    }
}
