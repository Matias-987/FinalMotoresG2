using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCTRL : MonoBehaviour
{
    public float attackDamage = 10f;
    public List<GameObject> powerUpPrefabs;
    public float moveSpeed = 3f;
    public float angularSpeed = 120f;
    public float acceleration = 8f;
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private float attackCooldown = 2f;
    private float currentAttackCooldown;
    public float maxHealth = 50f;
    private float currentHealth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentAttackCooldown = attackCooldown;
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.angularSpeed = angularSpeed;
        navMeshAgent.acceleration = acceleration;
        currentHealth = maxHealth;
        Instantiator.Instance.RegisterEnemy(gameObject);
    }

    private void InitializeComponents()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.stoppingDistance = 1.5f;
    }

    private void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) < 2f)
            {
                if (currentAttackCooldown <= 0)
                {
                    AttackPlayer();
                    currentAttackCooldown = attackCooldown;
                }
                else
                {
                    currentAttackCooldown -= Time.deltaTime;
                }
            }
        }
    }

    private void AttackPlayer()
    {
        player.GetComponent<PlayerCTRL>().TakeDamage(attackDamage);
    }

    private void GeneratePowerUp()
    {
        if(Random.Range(0, 4) == 2)
        {
            Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Count)], transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"Recibiendo daño: {damage}");
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!GameManager.isRestarting)
        {
            GeneratePowerUp();
        }
        Destroy(gameObject);
    }
}
