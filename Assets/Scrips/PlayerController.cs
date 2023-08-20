using Scrips;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float currentJumpForce = 2f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float maxJumpForce = 10f;
    [SerializeField] float jumpChargeRate = 2f;
    [SerializeField] Vector2 leftWallJumpForce = new(5, 4f);
    [SerializeField] Vector2 rightWallJumpForce = new(-5, 4f);

    public static CatState State { get; private set; }

    Rigidbody2D rb;

    void Awake()
    {
        State = CatState.Walk;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (State == CatState.Walk && Input.GetKeyDown(KeyCode.Space))
        {
            currentJumpForce = 5f;
            State = CatState.PreparingJump;
        }
            

        if (State == CatState.PreparingJump && Input.GetKeyUp(KeyCode.Space))
        {
            State = CatState.Jump;
            Jump();
        }

        if((State is CatState.ClimbRightWall or CatState.ClimbLeftWall) && Input.GetKeyUp(KeyCode.Space))
        {
            DoWallJump();
        }
        
        if (State == CatState.PreparingJump && Input.GetKey(KeyCode.Space))
            currentJumpForce = 
                Mathf.Clamp(currentJumpForce + Time.deltaTime * jumpChargeRate, 0f, maxJumpForce);

    }

    void DoWallJump()
    {
        rb.velocity = State == CatState.ClimbRightWall ? 
            rightWallJumpForce : 
            leftWallJumpForce;
        
        State = CatState.WallJump;
    }

    void FixedUpdate()
    {
        if (State == CatState.Walk) 
            DoHorizontalMovement();
        
        if (State is CatState.ClimbRightWall or CatState.ClimbLeftWall) 
            DoClimb();
        
        if(State == CatState.Jump)
            DoHorizontalMovement();
    }

    void DoClimb()
    {
        if(Input.GetKey(KeyCode.Space))
            rb.velocity = new Vector2(rb.velocity.x, climbSpeed);
    }

    void DoHorizontalMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        Move(horizontalInput);
    }
    
    void Move(float direction) => 
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
        currentJumpForce = 2f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            State = CatState.Walk;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            var contactPoint = collision.GetContact(0).point;
            if (contactPoint.x > transform.position.x)
                State = CatState.ClimbRightWall;
            else
                State = CatState.ClimbLeftWall;
        }
    }
}