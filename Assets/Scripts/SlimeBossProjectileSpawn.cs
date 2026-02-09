using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossProjectileSpawn : MonoBehaviour
{
    [SerializeField]
    private int projectileCount;

    [SerializeField]
    private GameObject projectilePrefab;

    Vector2 newDirection;
    private void OnEnable()
    {
        SlimeBossController.OnSlimeSmash += CreateProjectile;
    }

    private void OnDisable()
    {
        SlimeBossController.OnSlimeSmash -= CreateProjectile;
    }

    private void CreateProjectile()
    {
        for (int i = 1; i <= projectileCount; i++)
        {
            print(i);
        }
    }

    private void GetProjectileVector()
    {
        //slime will launch attack from a 180 degree angle represented by x-1 y1 x1
        //when creating projectile you divide the max degree : 1 - -1 = 2
        //And then divide it by the number of projectile : 2 / 5 = 0.4
        //so now we now that the different vector will be x1 y0 : 
    }
}
