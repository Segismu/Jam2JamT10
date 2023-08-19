using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float currentJumpForce = 2f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float maxJumpForce = 10f;
    [SerializeField] float jumpChargeRate = 2f;

    public int maxJumps = 1;

    private bool isOnGround = false;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private int remainingJumps;
    private bool isOnWall = false;
    private Vector2 wallNormal;
    private bool isClimbing = false;
    private bool noMove = false;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        remainingJumps = maxJumps;
    }

    void Start()
    {
        IncreaseJumps(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (remainingJumps > 0 || (isOnWall && rb.velocity.y <= 0)))
        {
            currentJumpForce = 5f;
        }

        if (remainingJumps > 1)
        {

            if (Input.GetKey(KeyCode.Space))
            {
                currentJumpForce = Mathf.Clamp(currentJumpForce + Time.deltaTime * jumpChargeRate, 0f, maxJumpForce);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            noMove = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            noMove = false;
            Jump();
        }

        if (isOnWall && Input.GetKey(KeyCode.Space))
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        if (isOnGround)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0 && !isClimbing)
        {
            Move(horizontalInput);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
        }

        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, climbSpeed);
        }
    }
    private void Jump()
    {
        if (remainingJumps > 0 && coyoteTimeCounter > 0)
        {
            if (isOnWall)
            {
                rb.velocity = new Vector2(-wallNormal.x * moveSpeed, currentJumpForce);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
                remainingJumps--;
                coyoteTimeCounter = 0;
            }

            currentJumpForce = 2f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            remainingJumps = maxJumps;
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            isOnWall = true;
            wallNormal = collision.contacts[0].normal.normalized;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Para que no se tranque en las esquinas?
            isOnWall = true;
            wallNormal = collision.contacts[0].normal.normalized;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isOnWall = false;
        }
    }

    private void Move(float direction)
    {
        if (noMove == false)
        {
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        }
    }

    public void IncreaseJumps(int amount)
    {
        maxJumps += amount;
        remainingJumps += amount;
    }
}