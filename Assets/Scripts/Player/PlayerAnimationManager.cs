using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator p_animator;
    private void Awake()
    {
        p_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlayerMovements.OnIdleEvent += TriggerIdleAnimation;
        PlayerMovements.OnRunEvent  += TriggerRunAnimation;
        PlayerMovements.OnJumpEvent += TriggerJumpAnimation;
        PlayerMovements.OnFallEvent += TriggerFallAnimation;
    }

    private void OnDisable()
    {
        PlayerMovements.OnIdleEvent -= TriggerIdleAnimation;
        PlayerMovements.OnRunEvent  -= TriggerRunAnimation;
        PlayerMovements.OnJumpEvent -= TriggerJumpAnimation;
        PlayerMovements.OnFallEvent -= TriggerFallAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TriggerIdleAnimation()
    {
        p_animator.SetBool("isRunning", false);
        p_animator.SetBool("isFalling", false);
    }
    private void TriggerRunAnimation()
    {
        p_animator.SetBool("isFalling", false);
        p_animator.SetBool("isRunning", true);
    }
    private void TriggerJumpAnimation()
    {
        p_animator.SetTrigger("Jump");
        p_animator.SetBool("isFalling", false);
        p_animator.SetBool("isRunning", false);
    }
    private void TriggerFallAnimation()
    {
        p_animator.SetBool("isFalling", true);
        p_animator.SetBool("isRunning", false);
    }
}
