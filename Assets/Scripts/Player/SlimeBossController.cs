using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScriptableSlimeLogicData), typeof(Rigidbody2D))]
public class SlimeBossController : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private ScriptableSlimeLogicData slimeLogicData;
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private SpriteRenderer spriteR;
    [SerializeField] private LayerMask interactableLayers;
    private Vector2 targetedPlayerPosition => targetPlayer.transform.position;

    private bool isLookingRight;

    private bool isGrounded;

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

    }

    private void Update()
    {
        SlimeHitBox();
        if (Input.GetKeyDown(KeyCode.F) && isGrounded)
        {
            rb2D.velocity = new Vector2(CalculateHorizontalMotion(), CalculateVerticalMotion());
        }

        CorrectSlimeSprite();
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
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
            }
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                print("Player");
            }
        }
        
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector2 boxSize = new Vector2(transform.localScale.x * slimeLogicData.BoxCastSize.x,
                                      transform.localScale.y * slimeLogicData.BoxCastSize.y);
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}