using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Main Menu Active");
        MouseLookScript.isUIActive = true; 
        MouseLookScript.SetCursorState(true);
    }
    public void StartGame()
    {
        Debug.Log("Starting Game...");
        MouseLookScript.isUIActive = false;
        MouseLookScript.SetCursorState(false); 
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Debug.Log("Game Exit!"); 
        Application.Quit(); 
    }
}