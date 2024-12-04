using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameOverManager : MonoBehaviour
{
    void Start()
    {
        MouseLookScript.isUIActive = true;
        MouseLookScript.SetCursorState(true); // Enable UI cursor state
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        MouseLookScript.isUIActive = false;
        MouseLookScript.SetCursorState(false); // Reset cursor state
        SceneManager.LoadScene("MainScene"); // Replace with your game scene name
    }

    public void GoToMainmanu()
    {
        MouseLookScript.isUIActive = false;
        MouseLookScript.SetCursorState(false); // Reset cursor state
        SceneManager.LoadScene("MainMenu"); // Replace with your game scene name
    }

    public void ExitGame()
    {
        Debug.Log("Game Exit!");
        Application.Quit();
    }
}