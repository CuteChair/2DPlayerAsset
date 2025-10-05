using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScriptablePlayerMovements p_statsSO;
    [SerializeField] private Rigidbody2D p_rb2d;
    [SerializeField] private SpriteRenderer p_sr;

    //Debug
    [SerializeField] private GameObject debugPoint;
    private int debugUsed;

    //Jump Variables
    private float jumpBufferCounter;
    [SerializeField]private bool isJumping;

    //ApexTime Variables
    private float apexCounter;
    //private bool isApexEnded;

    //GroundCheck variables
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    //Gravity Variables
    private float defaultGravity;

    private void Awake()
    {

        if (p_rb2d == null)
           p_rb2d = GetComponent<Rigidbody2D>();

        if (p_sr == null)
            p_sr     = p_sr ?? GetComponent<SpriteRenderer>();

           defaultGravity = p_rb2d.gravityScale;
    }

    private void Update()
    {
        jumpBufferCounter   -= Time.deltaTime;
        apexCounter         -= Time.deltaTime;

        GetInputs();
        //ApplyApexMod();
        ApplyApexMod();
        if (!isGrounded && IsFalling() && !isJumping)
            AccelerateFall();
    }

    private void FixedUpdate()
    {
        Move();
        GroundCheck();
        JumpAction();
    }

    #region Inputs
    bool jumpHeld   => Input.GetKey(KeyCode.Space);
    float moveX     => Input.GetAxisRaw("Horizontal");
    private void GetInputs()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
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
    }

    private void JumpAction()
    {
        if (jumpBufferCounter > 0 && isGrounded)
        {
            jumpBufferCounter = 0;
            p_rb2d.AddForce(Vector2.up * p_statsSO.JumpForce, ForceMode2D.Impulse);
            isJumping = true;
            //print("Jumped");
        }

        if (!jumpHeld && !IsFalling()) 
           AccelerateFall();
        if (jumpHeld && IsFalling())
            AccelerateFall();

        if(isGrounded && jumpBufferCounter < 0 && !IsFalling())
        {
            ResetGravity();
            isJumping = false;
            debugUsed = 0;
        }
    }
    #endregion

    #region Gravity

    private void ApplyApexMod()
    {
        if (isJumping)
        {
                if (Mathf.Abs(p_rb2d.velocity.y) < 2f && debugUsed == 0)
                {
                    Instantiate(debugPoint, transform.position, transform.rotation);
                    debugUsed = 1;
                }
             
               //StartCoroutine(ApexModifCoroutine(1f));
        }
    }
    private bool IsFalling()
    {
        return p_rb2d.velocity.y < 0;
    }

    private void AccelerateFall()
    {
            p_rb2d.gravityScale = defaultGravity * p_statsSO.GravityMultiplier;
    }
    private float ResetGravity() => p_rb2d.gravityScale = defaultGravity;
    #endregion
    private void GroundCheck()
    {
        Vector2 boxSize = new Vector2(transform.localScale.x * p_statsSO.GcSize.x,
                                      transform.localScale.y * p_statsSO.GcSize.y);

        isGrounded = Physics2D.OverlapBox(transform.position + p_statsSO.GcOffset, boxSize, 0f, groundLayer);
    }


    //private IEnumerator ApexModifCoroutine(float wait)
    //{
    //    p_rb2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

    //    yield return new WaitForSeconds(wait);

    //    p_rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    //}
    private void OnDrawGizmos()
    {
        Vector2 boxSize = new Vector2(transform.localScale.x * p_statsSO.GcSize.x,
                                      transform.localScale.y * p_statsSO.GcSize.y);

        Gizmos.color = isGrounded ? Color.green : Color.red;

        Gizmos.DrawWireCube(transform.position + p_statsSO.GcOffset, boxSize);
    }
}
