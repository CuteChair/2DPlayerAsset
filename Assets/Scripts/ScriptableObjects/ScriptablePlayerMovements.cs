using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object", fileName = "Data/Player", order = 1)]
public class ScriptablePlayerMovements : ScriptableObject
{
    [Header("Running parameters")]
    [Range(0f, 10f)] public float RunSpeed;

    [Header("Jump Parameters")]
    [Range(0f, 100f)] public float JumpForce;
    [Range(0f, 1f)] public float JumpBufferTimer;

    [Header("GroundCheck parameters")]
    public Vector3 GcOffset;
    public Vector2 GcSize;


    
   
    
}
