using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject[] lives; 
    private int currentLives;
    
    public Slider healthSlider;
    public GameObject deathMessagePanel; 
    public TMP_Text deathMessage;
    public TMP_Text respawnText;
    public GameObject respawnPlatform; 
    public Transform respawnPoint;
    public WaveManager waveManager; 
    public WaveUIManager waveUIManager; 
    public SpawnerScript spawnerScript;
    private GunScript gunScript;
    private GunInventory gunInventory;
    
    private bool isRespawning = false;
    
    void Start()
    {
        gunInventory = FindObjectOfType<GunInventory>();
        currentHealth = maxHealth;
        currentLives = lives.Length;
        UpdateHealthUI();
        if (deathMessagePanel != null) deathMessagePanel.SetActive(false); 
        if (respawnPlatform != null) respawnPlatform.SetActive(false); 
    }
    
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth / maxHealth;
    }
    
    void Update()
    {
        if (currentLives <= 0 && currentHealth <= 0)
            waveManager.PlayerFailed();
    }

    public void TakeDamage(float damageAmount)
    {
        if (isRespawning) return; 

        currentHealth -= damageAmount;
        PlayerMovementScript playerMovement = GetComponent<PlayerMovementScript>();
        if (playerMovement != null)
        {
            playerMovement.PlayHurtSound();
        }
        else
        {
            Debug.LogWarning("PlayerMovementScript is not attached to the GameObject.");
        }        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        isRespawning = true;
        LoseLife();

        if (currentLives > 0)
        {
            if (deathMessagePanel != null)
            {
                MouseLookScript.isUIActive = true;
                MouseLookScript.SetCursorState(true); 
                if (gunScript == null)
                    gunScript = FindObjectOfType<GunScript>();
                gunInventory.isInventoryVisible = false;
                gunScript.isCrosshairVisible = false;
                
                deathMessagePanel.SetActive(true);
                deathMessage.text = "You Died!";
            }

            Time.timeScale = 0f; 

            for (int i = 5; i > 0; i--)
            {
                if (respawnText != null)
                    respawnText.text = $"Respawning in {i} seconds...";
                yield return new WaitForSecondsRealtime(1f); 
            }
            if (deathMessagePanel != null) deathMessagePanel.SetActive(false);
            
            MouseLookScript.isUIActive = false;
            MouseLookScript.SetCursorState(false); 
            if (gunScript == null)
                gunScript = FindObjectOfType<GunScript>();
            gunInventory.isInventoryVisible = true;
            gunScript.isCrosshairVisible = true;
            
            if (respawnPlatform != null) respawnPlatform.SetActive(true); 
            
            if (spawnerScript != null)
            {
                spawnerScript.RefreshActiveSpawners();
                spawnerScript.TeleportEnemiesToSpawners();
            }
            
            RespawnPlayer();
            Time.timeScale = 1f;
            yield return new WaitForSecondsRealtime(3f);
            if (respawnPlatform != null) respawnPlatform.SetActive(false);
        }
        else
        {
            GameOver();
        }

        isRespawning = false;
    }

    private void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            Destroy(lives[currentLives]);
        }
    }

    private void RespawnPlayer()
    {
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    
    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        waveUIManager.DisplayWaveNotification("Game Over! Wave Failed!");
        waveManager.PlayerFailed();
    }
}