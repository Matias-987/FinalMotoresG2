using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 30f;
    public float lifespan = 2f;
    public float damageAmount = 25f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifespan);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyCTRL enemy = collision.gameObject.GetComponent<EnemyCTRL>();

            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }
        Destroy(gameObject);
    }
}
