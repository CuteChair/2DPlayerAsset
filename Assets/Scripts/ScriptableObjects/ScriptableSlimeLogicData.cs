using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimebossLogicData", menuName = "ScriptableObjects/SlimeBossLogicData")]
public class ScriptableSlimeLogicData : ScriptableObject
{
    [Header("Movements parameter")]
    [Tooltip("How long the slimes jump last (1st Phase): High value = low speed | Low value = high speed")]
    [Range(0f, 3f)] public float FirstAirTime;
    [Range(0f, 10f)] public float AtkCooldown;

    [Header("Collision Parameter")]
    [Tooltip("Size of the SlimeBoss BoxCast")]
    public Vector2 BoxCastSize;

    public float DisablingGroundCheckTimer;

}
