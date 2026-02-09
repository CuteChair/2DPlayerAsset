using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeBossProjectileSpawn : MonoBehaviour
{
    [SerializeField]
    private ScriptableSlimeProjectile projectileSO;
    private int projectileCount
    {
        get
        {
            if (projectileSO == null)
            {
                Debug.LogError("ProjectileSO is not assigned", this);
                return 0;

            }

            return projectileSO.ProjectileCount;
        }
    }

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform projectileSpawn;

    [SerializeField]
    private float StartAngle = 0f;
    [SerializeField]
    private float EndAngle = 180f;

    [SerializeField]
    private List<GameObject> projectilePool = new List<GameObject>();
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

        for (int i = 0; i < projectileCount; i++)
        {
            float t = (projectileCount == 1) ? 0.5f : i / (float)(projectileCount - 1);
            float currentAngle = Mathf.Lerp(StartAngle, EndAngle, t);

            Vector2 newDirection = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
                );

            print("Current Angle : " + currentAngle + "°");

            Quaternion newRotation = Quaternion.Euler(0f, 0f, currentAngle);

            GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawn.position, newRotation);

            Debug.DrawRay(projectileSpawn.position, newDirection * 5f, Color.green, 1f);
        }
    }

   
}
