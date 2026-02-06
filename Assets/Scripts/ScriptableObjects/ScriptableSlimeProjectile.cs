using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data", fileName = "ScriptableObject/ProjectileData")]
public class ScriptableSlimeProjectile : ScriptableObject
{
    public float ProjectileVelocity;
    public float ProjectileAliveTime;
}
