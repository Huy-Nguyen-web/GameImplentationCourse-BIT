using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool _isFacingRight;

    
    void Start()
    {
        _isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer) && !Physics2D.Raycast(wallCheck.position, Vector2.right, 0.1f, groundLayer))
        {
            Debug.Log("keep walking");
            //keep walking
        }
        else
        {
            Debug.Log("turn around");
            _isFacingRight = !_isFacingRight;
            var localScale = transform.localScale;
            localScale.x = (_isFacingRight) ? 1f : -1f;
            transform.localScale = localScale;
        }
    }
    private void FixedUpdate()
    {
        if (_isFacingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + (Vector3)(Vector2.down * 0.5f));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * 0.1f));
    }
}
