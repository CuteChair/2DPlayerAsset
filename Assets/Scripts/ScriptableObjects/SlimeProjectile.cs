using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    [SerializeField]
    private ScriptableSlimeProjectile projectileSO;

    [SerializeField]
    private float velocity;

    [SerializeField]
    private float aliveTime;

    Vector2 newDirection;

    private void OnEnable()
    {
        aliveTime = projectileSO.ProjectileAliveTime;
    }

    private void Update()
    {
        if (aliveTime <= 0)
            gameObject.SetActive(false);

        aliveTime -= Time.deltaTime;
        transform.Translate(newDirection * velocity * Time.deltaTime);
    }

    public void SetProjectileDirection(Vector2 direction)
    {
        newDirection = direction;
    }
}
