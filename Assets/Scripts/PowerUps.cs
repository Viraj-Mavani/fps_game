using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PowerUps : MonoBehaviour
{
    public GameObject[] collectableSpawners; // Array to store CollectableSpawner objects
    public GameObject[] powerUpPrefabs; // Array to store power-up prefabs (Health, Kill, etc.)
    public float spawnDuration = 50f; // Time to keep power-ups active
    private GameObject activePowerUp1; // Reference to the first active power-up
    private GameObject activePowerUp2; // Reference to the second active power-up
    private bool isSpawningActive = false;

    private WaveManager waveManager;  // Reference to WaveManager
    public WaveUIManager waveUIManager; // Assign in Inspector
    
    public AudioClip powerUpSound; // Drag and drop your sound clip here in the inspector.
    public AudioClip spawnSound; // Drag and drop your sound clip here in the inspector.
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component.
        waveManager = FindObjectOfType<WaveManager>();
    }
    
    public void StartPowerUpSpawning()
    {
        if (!isSpawningActive)
        {
            StartCoroutine(SpawnPowerUpsForDuration());
        }
    }
    
    private IEnumerator SpawnPowerUpsForDuration()
    {
        isSpawningActive = true;

        List<GameObject> shuffledSpawners = new List<GameObject>(collectableSpawners);
        List<GameObject> shuffledPowerUps = new List<GameObject>(powerUpPrefabs);

        System.Random rng = new System.Random();
        shuffledSpawners = shuffledSpawners.OrderBy(x => rng.Next()).ToList();
        shuffledPowerUps = shuffledPowerUps.OrderBy(x => rng.Next()).ToList();

        GameObject powerUp1 = shuffledPowerUps[0];
        GameObject powerUp2 = shuffledPowerUps[1];
        GameObject spawner1 = shuffledSpawners[0];
        GameObject spawner2 = shuffledSpawners[1];

        activePowerUp1 = Instantiate(powerUp1, spawner1.transform.position, spawner1.transform.rotation);
        activePowerUp2 = Instantiate(powerUp2, spawner2.transform.position, spawner2.transform.rotation);

        PlayPowerUpSpawnSound();

        yield return new WaitForSeconds(spawnDuration);

        Destroy(activePowerUp1);
        Destroy(activePowerUp2);

        isSpawningActive = false;
    }
    
    // Call this method when a wave ends or at a designated time to start spawning power-ups
    public void OnWaveEnd()
    {
        StartPowerUpSpawning();
    }
    
    // Spawn a random power-up
    // public void SpawnRandomPowerUp()
    // {
    //     // Choose a random spawner and a random power-up prefab
    //     Transform randomSpawner = spawners[Random.Range(0, spawners.Length)];
    //     GameObject randomPowerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
    //
    //     // Instantiate the power-up and assign its effect
    //     GameObject spawnedPowerUp = Instantiate(randomPowerUp, randomSpawner.position, Quaternion.identity);
    //     PowerUp powerUpScript = spawnedPowerUp.AddComponent<PowerUp>();
    //     powerUpScript.InitializePowerUp(this);
    //
    //     // Destroy the power-up after the maximum duration
    //     Destroy(spawnedPowerUp, powerUpDuration);
    // }
    
    // Random effect function
    private void RandomEffect(GameObject player)
    {
        // List of possible random effects
        int randomEffect = Random.Range(0, 3); // Random number from 0 to 2

        switch (randomEffect)
        {
            case 0:
                Debug.Log("Random Effect: Health refill applied.");
                ApplyHealthRefill(player); // Apply health refill
                break;

            case 1:
                Debug.Log("Random Effect: Kill all enemies applied.");
                KillAllEnemies(); // Kill all enemies
                break;

            case 2:
                Debug.Log("Random Effect: Kill random enemies applied.");
                KillRandomEnemies(); // Kill a random number of enemies (1 to 3)
                break;

            default:
                Debug.LogWarning("Unknown random effect!");
                break;
        }
    }
    
    // Apply the power-up effect based on its type
    public void ApplyPowerUpEffect(GameObject powerUp, GameObject player)
    {
        string powerUpTag = powerUp.tag;
        // Play the power-up sound when the player collects a power-up
        PlayPowerUpSound();
        
        switch (powerUpTag)
        {

            case "HealthSphear":
                Debug.Log("Health refill effect applied.");
                ApplyHealthRefill(player); // Implement health restoration
                break;
            
            case "KillSphear":
                Debug.Log("Kill all enemies effect applied.");
                KillAllEnemies(); // Implement killing all enemies
                break;
            
            case "RandomSphear":
                Debug.Log("Kill all enemies effect applied.");
                RandomEffect(player); // Implement killing all enemies
                break;

            // case "AmmoSphear":
            //     Debug.Log("Ammo refill effect applied.");
            //     ApplyAmmoRefill(player); // Implement ammo refill
            //     break;
            // case "SpeedSphear":
            //     Debug.Log("Speed boost effect applied.");
            //     StartCoroutine(ApplySpeedBoost(player)); // Implement speed boost
            //     break;
            // case "PowerSphear":
            //     Debug.Log("Power Bullet effect applied.");
            //     IncreaseBulletDamage(); // Implement killing all enemies
            //     break;
            
            default:
                Debug.LogWarning("Unknown Power-Up tag!");
                break;
        }
    }

    // Health refill effect
    private void ApplyHealthRefill(GameObject player)
    {
        Debug.Log("Player's health restored!");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.RestoreHealth(); // Adjust health restore value as needed
            waveUIManager.DisplayWaveNotification("Player's health restored!");
        }
    }
    
    // Kill all enemies effect
    private void KillAllEnemies()
    {
        EnemyFollow[] enemies = FindObjectsOfType<EnemyFollow>();
        foreach (EnemyFollow enemy in enemies)
        {
            if (waveManager != null)
                waveManager.OnEnemyKilled();
            Destroy(enemy.gameObject);
        }
        waveUIManager.DisplayWaveNotification("All enemies killed!");
        Debug.Log("All enemies killed!");
    }
    
    private void KillRandomEnemies()
    {

        EnemyFollow[] enemies = FindObjectsOfType<EnemyFollow>(); // Assuming enemies have the "Enemy" tag
        if (enemies.Length > 0)
        {
            int enemiesToKill = Random.Range(0, enemies.Length); // Random number between 1 and 3
            for (int i = 0; i < enemiesToKill; i++)
            {
                if (enemies.Length > 0)
                {
                    int randomIndex = Random.Range(0, enemies.Length);
                    Destroy(enemies[randomIndex].gameObject); // Destroy randomly chosen enemy
                    enemies = FindObjectsOfType<EnemyFollow>(); // Refresh enemies list
                }
            }
            waveUIManager.DisplayWaveNotification("Killed "+ enemiesToKill +" enemies!");
            Debug.Log("Killed "+ enemiesToKill +" enemies!");
        }
    }
    
    // Play the power-up sound
    private void PlayPowerUpSound()
    {
        if (audioSource != null && powerUpSound != null)
        {
            audioSource.PlayOneShot(powerUpSound); // Play the sound once
        }
        else
        {
            Debug.LogWarning("AudioSource or PowerUpSound not assigned!");
        }
    }
    
    private void PlayPowerUpSpawnSound()
    {
        // Add your sound logic here to play a sound when power-up spawns
        if (audioSource != null && spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound); // Play the sound once
        }
    }
    // // Ammo refill effect
    // private void ApplyAmmoRefill(GameObject player)
    // {
    //     Debug.Log("Player's ammo refilled!");
    //     if (gunScript != null)
    //     {
    //         gunScript.RefillAmmo(100); // Adjust ammo refill value as needed
    //     }
    // }
    
    // Increase the bullet damage by 10
    // private void IncreaseBulletDamage()
    // {
    //     if (gunScript != null)
    //     {
    //         gunScript.SetGunDamage(gunScript.gunDamage + 100f);  // Increase gun damage by 10
    //         Debug.Log("Bullet damage increased to: " + gunScript.gunDamage);
    //
    //         // Optionally, reset the damage after a set period, e.g., 10 seconds
    //         StartCoroutine(ResetBulletDamageAfterDelay(10f));
    //     }
    // }
    
    // private IEnumerator ResetBulletDamageAfterDelay(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     if (gunScript != null)
    //     {
    //         gunScript.SetGunDamage(gunScript.gunDamage - 100f);  // Reset to original damage
    //         Debug.Log("Bullet damage reset to original.");
    //     }
    // }
    
    // Speed boost effect
    // private IEnumerator ApplySpeedBoost(GameObject player)
    // {
    //     // Increase player's speed temporarily
    //     PlayerMovementScript playerMovement = player.GetComponent<PlayerMovementScript>();
    //     GunScript gunScript = player.GetComponent<GunScript>();
    //     if (playerMovement != null)
    //     {
    //         int originalMaxSpeed = playerMovement.maxSpeed;
    //         playerMovement.maxSpeed *= 2; // Double the speed for the boost duration
    //         gunScript.Sprint();
    //         playerMovement.isSpeedBoosted = true; // Set speed boost flag
    //         // playerMovement.maxSpeed = gunScript.walkingSpeed;
    //         Debug.Log("Speed boosted! Current max speed: " + playerMovement.maxSpeed);
    //
    //         // Wait for 10 seconds
    //         yield return new WaitForSeconds(10);
    //
    //         // Reset speed back to original
    //         playerMovement.maxSpeed = originalMaxSpeed;
    //         playerMovement.isSpeedBoosted = false; // Reset speed boost flag
    //         
    //         // // Re-enable sprinting (set maxSpeed back to original)
    //         // if (playerMovement.maxSpeed > gunScript.walkingSpeed)
    //         // {
    //         //     playerMovement.maxSpeed = gunScript.runningSpeed;
    //         // }
    //         
    //         Debug.Log("Speed boost ended! Current max speed: " + playerMovement.maxSpeed);
    //     }
    //
    // }
}
