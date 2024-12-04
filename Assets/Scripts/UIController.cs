using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text waveText; 

    public void UpdateWaveText(int waveNumber, float countdownTime)
    {
        string timeFormatted = FormatTime(countdownTime);
        waveText.text = $"Wave {waveNumber} - {timeFormatted}";
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);
        return string.Format("{00:00}:{01:00}:{02:00}", minutes, seconds, milliseconds);
    }
}