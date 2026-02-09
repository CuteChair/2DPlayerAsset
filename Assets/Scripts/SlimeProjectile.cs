using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    [SerializeField]
    private ScriptableSlimeProjectile projectileSO;

    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private float velocity;

    [SerializeField]
    private float aliveTime;

    Vector2 newDirection;

    private void OnEnable()
    {
        aliveTime = projectileSO.ProjectileAliveTime;
        velocity = projectileSO.ProjectileVelocity;

        if (rb2d == null)
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        rb2d.AddForce(transform.right * velocity, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (aliveTime <= 0)
            gameObject.SetActive(false);

        aliveTime -= Time.deltaTime;
        
    }

    //private void FixedUpdate()
    //{
    //    transform.Translate(transform.right * velocity * Time.deltaTime);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("Touched");
        }
    }
}
