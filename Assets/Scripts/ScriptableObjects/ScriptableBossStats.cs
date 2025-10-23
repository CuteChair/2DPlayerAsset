using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SlimeBossData", order = 2)]

public class ScriptableBossStats : MonoBehaviour
{
    [Header("Health Parameters")]
    [Tooltip("Maximum amout of health")]
    [Range(0f, 2000f)] public float MaxHealth;
    [Tooltip("Health threshold to reach boss's 2nd phase")]
    [Range(0f, 1999f)] public float SecondPhaseThreshold;
    [Tooltip("Health threshold to reach boss's 3rd phase")]
    [Range(0f, 1998f)] public float ThirdPhaseThreshold;
}
