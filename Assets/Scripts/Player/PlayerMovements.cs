using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovements : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScriptablePlayerMovements p_statsSO;
    [SerializeField] private Rigidbody2D p_rb2d;
    [SerializeField] private SpriteRenderer p_sr;

    //Jump Variables
    private float jumpBufferCounter;
    [SerializeField]private bool isJumping;

    //ApexTime Variables
    private bool  isApexApplied;

    //PogoJump Variables
    private bool canPogoJump;
    private bool isPogoJumpQueued;

    //Coyote Jump Variables
    private float coyoteCounter;

    //GroundCheck variables
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private bool wasGrounded;

    //Gravity Variables
    private float defaultGravity;

    private void Awake()
    {

        if (p_rb2d == null)
           p_rb2d = GetComponent<Rigidbody2D>();

        if (p_sr == null)
            p_sr = GetComponent<SpriteRenderer>();

           defaultGravity = p_rb2d.gravityScale;
    }

    private void Update()
    {
        jumpBufferCounter   -= Time.deltaTime;
        coyoteCounter       -= Time.deltaTime;

        GetInputs();
        ApplyApexMod();
        Handlegravity();
    }

    private void FixedUpdate()
    {
        Move();
        GroundCheck();
        PogoCheck();
        JumpAction();
    }

    #region Inputs
    bool jumpHeld   => Input.GetKey(KeyCode.Space);
    float moveX     => Input.GetAxisRaw("Horizontal");
    private void GetInputs()
    {
         if(Input.GetKeyDown(KeyCode.Space))
           {
            if (!isGrounded && IsFalling() && canPogoJump)
            {
                isPogoJumpQueued = true;
            }
            jumpBufferCounter = p_statsSO.JumpBufferTimer;
                
           }
    }

    #endregion

    #region Movements
    private void Move()
    {
        float targetSpeed = moveX * p_statsSO.TargetedSpeed;
        float newX = Mathf.MoveTowards(p_rb2d.velocity.x, targetSpeed, p_statsSO.RunAcceleration);

        p_rb2d.velocity = new Vector2(newX, p_rb2d.velocity.y);
        FlipSprite();
    }

    private void JumpAction()
    {
        bool regularJump    = jumpBufferCounter > 0 && isGrounded;
        bool pogoJump       = isPogoJumpQueued && isGrounded;
        bool coyoteJump     = IsFalling() && coyoteCounter > 0 && !isJumping && jumpBufferCounter > 0;

        if(regularJump || pogoJump || coyoteJump)
        {
            PerformJump();
        }

        if (!jumpHeld && p_rb2d.velocity.y > 0)
            p_rb2d.velocity += Vector2.up * Physics2D.gravity.y * (p_statsSO.LowJumpMultiplier - 1) * Time.deltaTime;

        if (isGrounded && jumpBufferCounter < 0 && !IsFalling())
        {
            isJumping = false;
            isApexApplied = false;
        }
    }

    private void PerformJump()
    {
        p_rb2d.velocity = new Vector2(p_rb2d.velocity.x, 0f);
        jumpBufferCounter = 0;
        p_rb2d.AddForce(Vector2.up * p_statsSO.JumpForce, ForceMode2D.Impulse);
        isJumping = true;
        isPogoJumpQueued = false;
    }

    private void FlipSprite()
    {
        bool isLookingRight = moveX > 0;
        bool isMoving       = moveX < 0 || moveX > 0;

        if (isLookingRight && isMoving)
        {
            p_sr.flipX = false;
        }
        else if (!isLookingRight && isMoving)
        {
            p_sr.flipX = true;
        }
    }
    #endregion

    #region Gravity

    private void ApplyApexMod()
    {
        if (isJumping)
        {
                if (Mathf.Abs(p_rb2d.velocity.y) < 2f && !isApexApplied)
                {
                    StartCoroutine(ApexModifCoroutine(p_statsSO.ApexAirTime));
                    isApexApplied = true;
                }
        }
    }

    private bool IsFalling()
    {
        return p_rb2d.velocity.y < 0;
    }

    private void CoyoteTimer()
    {
        coyoteCounter = p_statsSO.CoyoteTimer;
    }

    private void Handlegravity()
    {
        if (IsFalling() && !isGrounded)
        {
            p_rb2d.gravityScale = defaultGravity * p_statsSO.GravityMultiplier;
        }
        else
            p_rb2d.gravityScale = defaultGravity;
    }

    #endregion
    private void GroundCheck()
    {

        Vector2 boxSize = new Vector2(transform.localScale.x * p_statsSO.GcSize.x,
                                      transform.localScale.y * p_statsSO.GcSize.y);

        isGrounded = Physics2D.OverlapBox(transform.position + p_statsSO.GcOffset, boxSize, 0f, groundLayer);

        if (!isGrounded && wasGrounded)
            CoyoteTimer();

        wasGrounded = isGrounded;

    }

    private void PogoCheck()
    {
        canPogoJump = Physics2D.Raycast(transform.position, Vector2.down, p_statsSO.PogoRaySize, groundLayer);
    }


    private IEnumerator ApexModifCoroutine(float wait)
    {
        p_rb2d.gravityScale = 0f;
        p_rb2d.velocity = new Vector2(p_rb2d.velocity.x, 0f);

        yield return new WaitForSeconds(wait);

        p_rb2d.gravityScale = defaultGravity;
    }
    private void OnDrawGizmos()
    {
        Vector2 boxSize = new Vector2(transform.localScale.x * p_statsSO.GcSize.x,
                                      transform.localScale.y * p_statsSO.GcSize.y);

        Gizmos.color = isGrounded ? Color.green : Color.red;

        Gizmos.DrawWireCube(transform.position + p_statsSO.GcOffset, boxSize);

        Gizmos.color = canPogoJump ? Color.blue : Color.yellow;

        Gizmos.DrawRay(transform.position, Vector2.down * p_statsSO.PogoRaySize);
            
    }
}
