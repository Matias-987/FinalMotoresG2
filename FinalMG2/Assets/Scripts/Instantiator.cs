using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator: MonoBehaviour
{
    public static Instantiator Instance;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private int baseEnemies = 1;
    [SerializeField] private int currentWave = 1;
    [SerializeField] private int enemiesRemaining;
    [SerializeField] private bool waveActive = false;
    private List<GameObject> currentEnemies = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (!waveActive && enemiesRemaining <= 0)
        {
            StartNextWave();
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        currentEnemies.Add(enemy);
        enemiesRemaining++;
    }

    public void ReportEnemyDeath(GameObject enemy)
    {
        {
            if (currentEnemies.Contains(enemy))
            {
                currentEnemies.Remove(enemy);
                enemiesRemaining = currentEnemies.Count;
                Debug.Log($"Enemigos restantes: {enemiesRemaining}");

                if (enemiesRemaining <= 0)
                {
                    waveActive = false;
                    StartNextWave();
                }
            }
        }
    }

    private void StartNextWave()
    {
        waveActive = true;
        int enemiesPerSpawnPoint = baseEnemies * (int)Mathf.Pow(2, currentWave - 1);
        int totalEnemies = enemiesPerSpawnPoint * spawnPoints.Count;

        StartCoroutine(SpawnWave(totalEnemies));
        currentWave++;
    }

    IEnumerator SpawnWave(int totalEnemies)
    {
        enemiesRemaining = totalEnemies;
        currentEnemies.Clear();

        int enemiesPerPoint = Mathf.CeilToInt((float)totalEnemies / spawnPoints.Count);

        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < enemiesPerPoint; i++)
            {
                if (enemiesRemaining <= 0) yield break;
                Vector3 spawnPos = spawnPoint.position + new Vector3(  Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);

                GameObject enemy = Instantiate( enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnPos, Quaternion.identity);

                RegisterEnemy(enemy);
                enemiesRemaining--;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void OnDestroy()
    {
        if(!GameManager.isRestarting && Instantiator.Instance != null)
        {
            Instantiator.Instance.ReportEnemyDeath(gameObject);
        }
    }
}
