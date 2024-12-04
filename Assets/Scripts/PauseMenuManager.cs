using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu; 
    private bool isPaused = false;
    
    private GunScript gunScript;
    private GunInventory gunInventory;

    void Start()
    {
        gunInventory = FindObjectOfType<GunInventory>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        MouseLookScript.isUIActive = true; 
        MouseLookScript.SetCursorState(true);
        Time.timeScale = 0f;
        
        pauseMenu.SetActive(true); 
        
        if (gunScript == null)
            gunScript = FindObjectOfType<GunScript>();
        
        gunInventory.isInventoryVisible = false;
        gunScript.isCrosshairVisible = false;
        Debug.Log("Crosshair and Inventory visibility set to false");
    }

    public void ResumeGame()
    {
        Debug.Log("Resume");
        isPaused = false;
        MouseLookScript.isUIActive = false;
        MouseLookScript.SetCursorState(false); 
        Time.timeScale = 1f;
        
        pauseMenu.SetActive(false);
        
        if (gunScript == null)
            gunScript = FindObjectOfType<GunScript>();

        gunInventory.isInventoryVisible = true;
        gunScript.isCrosshairVisible = true;
        Debug.Log("Crosshair and Inventory visibility set to true");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        MouseLookScript.isUIActive = true;
        MouseLookScript.SetCursorState(true);
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); 
    }
}