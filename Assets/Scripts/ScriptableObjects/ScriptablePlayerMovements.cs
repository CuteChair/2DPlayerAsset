using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object", fileName = "Data/Player", order = 1)]
public class ScriptablePlayerMovements : ScriptableObject
{
    [Header("Running parameters")]
    [Range(0f, 10f)] public float RunAcceleration;
    [Range(0f, 100f)] public float TargetedSpeed;

    [Header("Jump Parameters")]
    [Range(0f, 100f)] public float JumpForce;
    [Range(10f, 15f)] public float LowJumpMultiplier;
    [Range(0f, 1f)] public float JumpBufferTimer;

    [Header("Pogo Jump Parameters")]
    [Range(0f, 10f)] public float PogoRaySize;

    [Header("Apex Parameters")]
    [Range(0f, 1f)] public float ApexAirTime;

    [Header("Coyote Jump Parameters")]
    [Range(0f, 1f)] public float CoyoteTimer;

    [Header("Gravity Parameters")]
    [Range(1f, 4f)] public float GravityMultiplier;

    [Header("GroundCheck parameters")]
    public Vector3 GcOffset;
    public Vector2 GcSize;


    
   
    
}
