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
        enemiesRemaining--;
        currentEnemies.Remove(enemy);

        if (enemiesRemaining <= 0)
        {
            waveActive = false;
        }
    }

    private void StartNextWave()
    {
        waveActive = true;
        int enemiesToSpawn = baseEnemies * (int)Mathf.Pow(2, currentWave - 1);
        StartCoroutine(SpawnWave(enemiesToSpawn));
        currentWave++;
    }

    IEnumerator SpawnWave(int totalEnemies)
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Vector3 spawnPosition = spawnPoint.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);

            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnPosition, Quaternion.identity);

            RegisterEnemy(enemy);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
