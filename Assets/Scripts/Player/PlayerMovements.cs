using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScriptablePlayerMovements p_statsSO;
    [SerializeField] private Rigidbody2D p_rb2d;
    [SerializeField] private SpriteRenderer p_sr;

    //Inputs variables
    //private float moveX;

    //Jump Variables
    private float jumpBufferCounter;

    //GroundCheck variables
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;



    private void Awake()
    {
        if(p_rb2d == null)
        {
            p_rb2d = GetComponent<Rigidbody2D>();
        }
        
        if(p_sr == null)
        {
            p_sr = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        jumpBufferCounter -= Time.deltaTime;
        GetInputs();
    }

    private void FixedUpdate()
    {
        Move();
        GroundCheck();
        JumpAction();
    }
    bool jumpHeld => Input.GetKey(KeyCode.Space);

    float moveX => Input.GetAxisRaw("Horizontal");
    private void GetInputs()
    {
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = p_statsSO.JumpBufferTimer;
        }
    }
   
    private void Move()
    {
        float targetSpeed = moveX * p_statsSO.TargetedSpeed;
        float newX = Mathf.MoveTowards(p_rb2d.velocity.x, targetSpeed, p_statsSO.RunAcceleration);

        p_rb2d.velocity = new Vector2(newX, p_rb2d.velocity.y);
    }

    private void JumpAction()
    {
        if (jumpBufferCounter > 0 && isGrounded)
        {
            p_rb2d.AddForce(Vector2.up * p_statsSO.JumpForce * 1.5f, ForceMode2D.Impulse);
        }
        if (!jumpHeld && !IsFalling() && jumpBufferCounter > 0) 
        {
            GravityIncrease();
        }

        if(isGrounded && jumpBufferCounter < 0 && !IsFalling())
        {
            ResetGravity();
        }
    }
    private void GroundCheck()
    {
        Vector2 boxSize = new Vector2(transform.localScale.x * p_statsSO.GcSize.x,
                                      transform.localScale.y * p_statsSO.GcSize.y);

        isGrounded = Physics2D.OverlapBox(transform.position + p_statsSO.GcOffset, boxSize, 0f, groundLayer);
    }

    private bool IsFalling()
    {
        return p_rb2d.velocity.y < 0;
    }

    private float GravityIncrease() => p_rb2d.gravityScale *= p_statsSO.GravityIncrease;
    private float ResetGravity() => p_rb2d.gravityScale = 9.5f;
    private void OnDrawGizmos()
    {
        Vector2 boxSize = new Vector2(transform.localScale.x * p_statsSO.GcSize.x,
                                      transform.localScale.y * p_statsSO.GcSize.y);

        Gizmos.color = isGrounded ? Color.green : Color.red;

        Gizmos.DrawWireCube(transform.position + p_statsSO.GcOffset, boxSize);
    }
}
