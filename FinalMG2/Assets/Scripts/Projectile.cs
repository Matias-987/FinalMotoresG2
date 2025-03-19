using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public float speed = 30f;
    [SerializeField] public float lifespan = 2f;
    [SerializeField] public float damageAmount = 25f;
    [SerializeField] private Rigidbody rb;

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
            if (enemy != null) enemy.TakeDamage(damageAmount);
        }
        Destroy(gameObject);
    }
}
