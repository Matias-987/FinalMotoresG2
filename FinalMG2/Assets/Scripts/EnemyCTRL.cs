using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCTRL : MonoBehaviour
{
    [SerializeField] public float attackDamage = 10f;
    [SerializeField] public List<GameObject> powerUpPrefabs;
    [SerializeField] public float moveSpeed = 3f;
    [SerializeField] public float angularSpeed = 120f;
    [SerializeField] public float acceleration = 8f;
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float currentAttackCooldown;
    [SerializeField] public float maxHealth = 50f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool powerUpGenerated = false;
    [SerializeField][Range(0, 1)] private float dropChance = 1f;
    [SerializeField] private GameObject powerUpPrefab;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(player.position);
        currentAttackCooldown = attackCooldown;
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.angularSpeed = angularSpeed;
        navMeshAgent.acceleration = acceleration;
        currentHealth = maxHealth;
        Instantiator.Instance.RegisterEnemy(gameObject);
        navMeshAgent.stoppingDistance = attackRange;
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
        if (player == null || navMeshAgent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
            navMeshAgent.isStopped = true;
            FacePlayer();
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.position);
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        PlayerCTRL playerHealth = player.GetComponent<PlayerCTRL>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation,  lookRotation, Time.deltaTime * 5f);
    }

    private void GeneratePowerUp()
    {
        if (powerUpPrefab != null && Random.value <= dropChance)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 2f;
            Instantiate(powerUpPrefab, spawnPos, Quaternion.identity);
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
        GeneratePowerUp();
        if (!GameManager.isRestarting)
        {
            Instantiator.Instance.ReportEnemyDeath(gameObject);
        }
        Destroy(gameObject);
        Debug.Log($"Enemigo destruido. Power up generado: {powerUpGenerated}");
    }

    private void OnDestroy()
    {
        if (!GameManager.isRestarting && Instantiator.Instance != null)
        {
            Instantiator.Instance.ReportEnemyDeath(gameObject);
        }
    }
}
