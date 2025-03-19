using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCTRL : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 5;
    private bool isGrounded;
    public float jumpForce = 5f;
    public float health = 100f;
    private float currentHealth;
    public static PlayerCTRL Player;
    public static PlayerCTRL Instance;
    public Transform firePoint;

    private void Awake()
    {
        if(Player == null)
        {
            Player = this;
        }
        else Destroy(gameObject);

        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = health;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        Move();
        Jump();

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Restart();
        }
    }

    private void Move()
    {
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");

        Vector3 movimiento = (transform.forward * movimientoV + transform.right * movimientoH) * moveSpeed;

        movimiento.y = rb.velocity.y;

        rb.velocity = movimiento;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponentInChildren<GunCTRL>()?.ResetPowerUps();
        GunCTRL gun = GetComponentInChildren<GunCTRL>();
        if (gun != null) gun.ResetPowerUps();

        Time.timeScale = 0;
        Debug.Log("Jugador muerto. Power-ups reseteados");
    }

    public static void ResetStaticInstance()
    {
        Instance = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            GunCTRL gun = GetComponentInChildren<GunCTRL>();

            if(gun != null)
            {
                gun.powerUpCount++;
                Destroy(other.gameObject);
            }
        }
    }
}
