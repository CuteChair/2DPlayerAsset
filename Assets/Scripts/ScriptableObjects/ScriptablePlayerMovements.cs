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
    [Range(0f, 1f)] public float JumpBufferTimer;

    [Header("Apex Parameters")]
    [Range(0f, 1f)] public float ApexTimer;

    [Header("Gravity Parameters")]
    [Range(1f, 4f)] public float GravityMultiplier;

    [Header("GroundCheck parameters")]
    public Vector3 GcOffset;
    public Vector2 GcSize;


    
   
    
}
