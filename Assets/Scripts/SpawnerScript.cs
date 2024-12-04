using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SpawnerScript : MonoBehaviour
{
    public List<Transform> spawners; 
    public List<GameObject> spawnableEnemies;
    public Transform player;
    public int activeEnemyCount = 0;

    private List<Transform> activeSpawners = new List<Transform>();
    public float spawnInterval = 2f;
    public bool isSpawning = false;

    void Start()
    {
        RefreshActiveSpawners();
    }

    public void RefreshActiveSpawners()
    {
        activeSpawners.Clear();
        foreach (var spawner in spawners)
        {
            if (spawner.gameObject.activeSelf)
                activeSpawners.Add(spawner);
        }
    }

    public void StartSpawning(int maxEnemiesToSpawn)
    {
        if (!isSpawning)
            StartCoroutine(SpawnEnemies(maxEnemiesToSpawn));
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnEnemies(int maxEnemiesToSpawn)
    {
        isSpawning = true;
        int enemiesSpawned = 0;

        while (isSpawning && enemiesSpawned < maxEnemiesToSpawn)
        {
            if (activeSpawners.Count == 0 || spawnableEnemies.Count == 0)
            {
                Debug.LogWarning("No active spawners or enemies to spawn!");
                break;
            }

            Transform randomSpawner = activeSpawners[Random.Range(0, activeSpawners.Count)];
            GameObject enemyPrefab = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
            
            GameObject spawnedEnemy = Instantiate(enemyPrefab, randomSpawner.position, Quaternion.identity);
            EnemyFollow enemyFollow = spawnedEnemy.GetComponent<EnemyFollow>();
            if (enemyFollow != null)
                enemyFollow.SetPlayer(player);
            
            enemiesSpawned++;
            activeEnemyCount++;
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false; 
    }

    public void TeleportEnemiesToSpawners()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (activeSpawners.Count == 0)
        {
            Debug.LogWarning("No active spawners found!");
            return;
        }

        foreach (var enemy in enemies)
        {
            Transform randomSpawner = activeSpawners[Random.Range(0, activeSpawners.Count)];
            enemy.transform.position = randomSpawner.position;
        }
    }
}
