using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime = 180f; // Total time in seconds
    private float currentTime; // Current time left
    public TMP_Text timerText;

    void Start()
    {
        currentTime = totalTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        currentTime = Mathf.Max(currentTime, 0f);

        UpdateTimerText();

        if (currentTime <= 0f)
        {
            
        }
    }

    void UpdateTimerText()
    {
        int secondsRemaining = Mathf.FloorToInt(currentTime);

        timerText.text = secondsRemaining.ToString() + "";
    }
}
