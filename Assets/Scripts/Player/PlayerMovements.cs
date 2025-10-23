using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovements : MonoBehaviour
{
    public static event Action OnJumpEvent;
    public static event Action OnRunEvent;
    public static event Action OnFallEvent;
    public static event Action OnIdleEvent;

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

    //Event variable
    private int currentState;
    private int previousState;
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
        CheckForIdle();
        CheckForRun();
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

    private void ChangeState(int newState)
    {
        currentState = newState;
        if (previousState != currentState)
        {
            switch (newState)
            {
                case 0:
                    OnIdleEvent?.Invoke();
                    //print("Idle event called");
                    previousState = currentState;
                    break;
                case 1:
                    OnRunEvent?.Invoke();
                    //print("Run event called");
                    previousState = currentState;
                    break;
                case 2:
                    OnJumpEvent?.Invoke();
                    //print("Jump event called");
                    previousState = currentState;
                    break;
                case 3:
                    OnFallEvent?.Invoke();
                    //print("Fall event called");
                    previousState = currentState;
                    break;
            }
                
        }
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
        ChangeState(2); //2 is for jump state
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

    private void CheckForIdle()
    {
        if (isGrounded && !isJumping) { 
            if(moveX > -0.1f && moveX < 0.1f)
            {
                ChangeState(0); // 0 is for idle state
            }
        }
    }

    private void CheckForRun()
    {
        if (isGrounded) 
        {
            if (moveX > 0f || moveX < 0)
                ChangeState(1); // 1 is for run state
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
            ChangeState(3); //3 is for fall state
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
