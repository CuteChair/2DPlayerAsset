using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data", fileName = "ScriptableObject/ProjectileData")]
public class ScriptableSlimeProjectile : ScriptableObject
{
    public int ProjectileCount;
    public float ProjectileVelocity;
    public float ProjectileAliveTime;
}
