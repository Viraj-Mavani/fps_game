using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private PowerUps powerUpManager;
    
    private void Start()
    {
        if (powerUpManager == null)
        {
            powerUpManager = FindObjectOfType<PowerUps>();
            if (powerUpManager == null)
            {
                Debug.LogError("PowerUps Manager not found in the scene!");
            }
        }
    }

    public void InitializePowerUp(PowerUps manager)
    {
        powerUpManager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (powerUpManager == null)
            {
                Debug.LogError("PowerUps Manager reference is missing!");
                return;
            }

            Debug.Log($"{gameObject.tag} collected by Player."); // Debug log for testing
            powerUpManager.ApplyPowerUpEffect(gameObject, other.gameObject);
            Destroy(transform.parent.gameObject); 
        }
    }
}