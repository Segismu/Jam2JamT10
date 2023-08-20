using Scrips;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CatConfig catConfig;

    float currentJumpForce;

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
            SoundManager.Instance.StopAllSFX();
            SoundManager.Instance.GetPrepareJump().Play();
        }
            

        if (State == CatState.PreparingJump && Input.GetKeyUp(KeyCode.Space))
        {
            State = CatState.Jump;
            Jump();
            SoundManager.Instance.StopAllSFX();
            SoundManager.Instance.GetJump().Play();
        }

        if((State is CatState.ClimbRightWall or CatState.ClimbLeftWall) && Input.GetKeyUp(KeyCode.Space))
        {
            DoWallJump();
            SoundManager.Instance.StopAllSFX();
            SoundManager.Instance.GetJump().Play();
        }
        
        if (State == CatState.PreparingJump && Input.GetKey(KeyCode.Space))
            currentJumpForce = 
                Mathf.Clamp(currentJumpForce + Time.deltaTime * catConfig.jumpChargeRate, 0f, catConfig.maxJumpForce);

    }

    void DoWallJump()
    {
        rb.velocity = State == CatState.ClimbRightWall ?
            catConfig.rightWallJumpForce :
            catConfig.leftWallJumpForce;
        
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
        if (Input.GetKey(KeyCode.Space))
        {
            if (!SoundManager.Instance.GetClimb().isPlaying)
                SoundManager.Instance.GetClimb().Play();
            rb.velocity = new Vector2(rb.velocity.x, catConfig.climbSpeed);
        }
            
    }

    void DoHorizontalMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        Move(horizontalInput);

        if (horizontalInput == 0)
        {
            SoundManager.Instance.StopAllSFX();
        }
        else
        {
            if (!SoundManager.Instance.GetWalk().isPlaying && State == CatState.Walk)
                SoundManager.Instance.GetWalk().Play();
        }
    }
    
    void Move(float direction) => 
        rb.velocity = new Vector2(direction * catConfig.moveSpeed, rb.velocity.y);

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
            
            SoundManager.Instance.StopAllSFX();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") &&
            State == CatState.ClimbLeftWall ||
            State == CatState.ClimbRightWall)
        {
            State = CatState.Jump;
        }
    }
}