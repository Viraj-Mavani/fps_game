using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public WaveUIManager waveUIManager; 
    public SpawnerScript spawnerScript; 
    public int currentWave = 0;
    private float waveTime = 120f;
    private float timer;

    public float timeBetweenWaves = 25f; 
    private int[] waveEnemyCounts = { 10, 15, 20 }; 

    private int[][] spawnerActivation = {
        new int[] { 0, 1 },
        new int[] { 0, 1, 2 },
        new int[] { 0, 1, 2, 3 } 
    };

    private bool isWaveInProgress = false;

    void Start()
    {
        StartNextWave();
    }
    
    public void StartFreeRoamPeriod()
    {
        StartCoroutine(FreeRoamCountdown());
    }
    
    void Update()
    {
        if (isWaveInProgress && timer > 0)
        {
            timer -= Time.deltaTime;
            waveUIManager.SetWaveTimeRemaining(timer);

            if (timer <= 0)
            {
                timer = 0;
                CheckWaveCompletion();
            }
        }
    }

    public void StartNextWave()
    {
        if (currentWave >= spawnerActivation.Length)
        {
            Debug.Log("All waves completed!");
            isWaveInProgress = false;
            waveUIManager.DisplayWaveNotification("All Waves Completed!");
            SceneManager.LoadScene("GameOverScene");
            return;
        }

        ActivateSpawnersForWave(currentWave);
        spawnerScript.StartSpawning(waveEnemyCounts[currentWave]);

        currentWave++;
        timer = waveTime;
        isWaveInProgress = true;

        waveUIManager.SetWaveNumber(currentWave);
        waveUIManager.SetWaveTimeRemaining(timer);
        
        Debug.Log($"Wave {currentWave} started!");
    }

    private void ActivateSpawnersForWave(int waveIndex)
    {
        Debug.Log($"Activating spawners for Wave {waveIndex + 1}");
        waveUIManager.DisplayWaveNotification($"Wave {waveIndex + 1} Started!");

        for (int i = 0; i < spawnerScript.spawners.Count; i++)
            spawnerScript.spawners[i].gameObject.SetActive(spawnerActivation[waveIndex].Contains(i));

        spawnerScript.RefreshActiveSpawners();
    }
    
    private IEnumerator FreeRoamCountdown()
    {
        float freeRoamTimer = timeBetweenWaves;

        while (freeRoamTimer > 0)
        {
            freeRoamTimer -= Time.deltaTime;
            waveUIManager.DisplayFreeRoamTimeRemaining(freeRoamTimer);
            yield return null;
        }

        waveUIManager.DisplayWaveNotification("Wave Starting!", 1f);
        StartNextWave();
    }
    
    public void OnEnemyKilled()
    {
        spawnerScript.activeEnemyCount--; 
        Debug.Log($"Enemy killed! Remaining enemies: {spawnerScript.activeEnemyCount}");

        if (spawnerScript.activeEnemyCount <= 0)
            CheckWaveCompletion();
    }
    
    public void CheckWaveCompletion()
    {
        if (spawnerScript.activeEnemyCount == 0 && !spawnerScript.isSpawning)
        {
            Debug.Log($"Wave {currentWave} completed! No enemies remain.");
            waveUIManager.DisplayWaveNotification($"Wave {currentWave} Completed!");
            EndWave();
        }
        else if (timer <= 0)
        {
            Debug.Log($"Wave {currentWave} failed. Time expired with {spawnerScript.activeEnemyCount} enemies remaining.");
            waveUIManager.DisplayWaveNotification("Game Over! Wave Failed!");
            PlayerFailed();
        }
        else
        {
            Debug.Log($"Wave {currentWave} still in progress. {spawnerScript.activeEnemyCount} enemies remain.");
        }
    }
    
    public void EndWave()
    {
        Debug.Log($"Wave {currentWave} completed!");
        isWaveInProgress = false;
        FindObjectOfType<PowerUps>().OnWaveEnd();
        StartFreeRoamPeriod();
    }
    

    public void PlayerFailed()
    {
        isWaveInProgress = false;
        SceneManager.LoadScene("GameFailedScene"); 
        Time.timeScale = 1f;
    }
}