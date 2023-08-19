using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float minJumpForce = 5f;
    [SerializeField] float maxJumpForce = 10f;
    [SerializeField] float currentJumpForce = 5f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] float moveSpeed = 3f;
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

    public Canvas canvas;
    public GameObject jumpBar;

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

                var image = jumpBar.GetComponent<Image>();
                image.fillAmount = (currentJumpForce - minJumpForce) / (maxJumpForce - minJumpForce);
                if (image.fillAmount == 1) jumpBar.GetComponent<Image>().color = Color.red;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            noMove = true;

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 playerScreenPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) + 15));

            jumpBar.GetComponent<RectTransform>().anchoredPosition = playerScreenPosition;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            noMove = false;

            var image = jumpBar.GetComponent<Image>();
            image.fillAmount = 0;
            image.color = Color.green;
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