using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
public class SlimeBossController : MonoBehaviour
{
    public static event Action OnSlimeSmash;

    [Header("Required Components")]
    [SerializeField] private ScriptableSlimeLogicData slimeLogicData;
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private SpriteRenderer spriteR;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Animator slimeAnimator;
    private Vector2 targetedPlayerPosition => targetPlayer.transform.position;

    public float stompTimer;

    [SerializeField]
    private float disableGroundCheckTimer;

    private bool isLookingRight;

    private bool isGrounded;

    private bool isCharging;

    private bool isFalling => rb2D.velocity.y < 0.01f;

    private Coroutine atkRoutine;

    private void Awake()
    {
        if (targetPlayer == null)
            targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (rb2D == null)
        {
            rb2D = GetComponent<Rigidbody2D>();
        }
        if (spriteR == null)
        {
            spriteR = GetComponent<SpriteRenderer>();          
        }
        if(slimeAnimator == null)
        {
            slimeAnimator = GetComponent<Animator>();
        }

    }

    private void Update()
    {
        stompTimer -= Time.deltaTime;
        disableGroundCheckTimer -= Time.deltaTime;

        if (stompTimer <= 0 && RandomAttack() == 5)
        {
            isCharging = true;
            stompTimer = slimeLogicData.AtkCooldown;
        }

        slimeAnimator.SetBool("isCharging", isCharging);

        if (isCharging && atkRoutine == null)
        {
            atkRoutine = StartCoroutine(AtkCycle(2f));
        }

        CorrectSlimeSprite();
    }

    private void FixedUpdate()
    {
        SlimeHitBox();
    }

    #region Maths velocity
    private float CalculateHorizontalMotion()
    {
        float horizontalVelocity = ((targetedPlayerPosition.x - transform.position.x) / slimeLogicData.FirstAirTime);

        return horizontalVelocity;
    }

    private float CalculateVerticalMotion()
    {
        float gravity = -Physics2D.gravity.y * rb2D.gravityScale;
        float verticalVelocity = ((targetedPlayerPosition.y - transform.position.y
                                    + 0.5f * gravity * MathF.Pow(slimeLogicData.FirstAirTime, 2))/slimeLogicData.FirstAirTime);
        return verticalVelocity;
    }
    #endregion

    #region Visuals
    private void CorrectSlimeSprite()
    {
        if (targetedPlayerPosition.x > transform.position.x && !isLookingRight)
        {
            spriteR.flipX = true;
            isLookingRight = true;
        }
        if(targetedPlayerPosition.x < transform.position.x && isLookingRight)
        {
            spriteR.flipX = false;
            isLookingRight = false;
        }
    }
    #endregion

    #region HitBox
    private void SlimeHitBox()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, slimeLogicData.BoxCastSize, 0f, Vector2.down, 0f, interactableLayers);
        if(hit.collider != null)
        {
            if (disableGroundCheckTimer <= 0)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") && isFalling && !isGrounded)
                {
                    print("Slime hit the floor");
                    isGrounded = true;

                    OnSlimeSmash?.Invoke();
                }
            }
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                print("Player");
            }
        }
        
    }

    private void ResetGroundedState()
    {
        disableGroundCheckTimer = slimeLogicData.DisablingGroundCheckTimer;
        print(disableGroundCheckTimer);
        isGrounded = false;
    }
    #endregion
    private int RandomAttack()
    {
        int randomInt = Random.Range(1, 1000);

        return randomInt;
    }
   private IEnumerator AtkCycle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isCharging = false;
        ResetGroundedState();
        rb2D.velocity = new Vector2(CalculateHorizontalMotion(), CalculateVerticalMotion());

        atkRoutine = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector2 boxSize = new Vector2(transform.localScale.x * slimeLogicData.BoxCastSize.x,
                                      transform.localScale.y * slimeLogicData.BoxCastSize.y);
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}