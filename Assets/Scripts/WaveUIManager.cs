using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveUIManager : MonoBehaviour
{
    public TMP_Text waveNumberText;
    public TextMesh waveTimerText; 
    public TMP_Text waveNotifyText; 
    
    private int currentWave = 0;
    private float waveTimeRemaining = 0f;

    public void SetWaveNumber(int waveNumber)
    {
        currentWave = waveNumber;
        if (waveNumberText != null)
            waveNumberText.text = $"Wave: {currentWave}";
    }

    public void SetWaveTimeRemaining(float timeRemaining)
    {
        waveTimeRemaining = timeRemaining;
        if (waveTimerText != null)
        {
            int minutes = Mathf.FloorToInt(waveTimeRemaining / 60);
            int seconds = Mathf.FloorToInt(waveTimeRemaining % 60);
            waveTimerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
    
    public void DisplayWaveNotification(string message, float duration = 3f)
    {
        if (waveNotifyText == null) return;

        waveNotifyText.text = message; 
        waveNotifyText.gameObject.SetActive(true); 

        Invoke(nameof(HideWaveNotification), duration);
    }
    
    private void HideWaveNotification()
    {
        if (waveNotifyText != null)
            waveNotifyText.gameObject.SetActive(false);
    }
    
    public void DisplayFreeRoamTimeRemaining(float timeRemaining)
    {
        if (waveNotifyText == null) return;

        waveNotifyText.text = $"Free Roam Time: {Mathf.CeilToInt(timeRemaining)} seconds";
        waveNotifyText.gameObject.SetActive(true); 
    }
}