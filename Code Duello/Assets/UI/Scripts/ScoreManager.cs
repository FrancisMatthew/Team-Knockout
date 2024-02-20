using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int scoreIncreaseAmount = 10;
    private EnemyHealthBar enemyHealth;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        enemyHealth = FindObjectOfType<EnemyHealthBar>();
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealthBar component not found!");
        }
        else
        {
            enemyHealth.OnHealthChange.AddListener(IncreaseScoreOnHealthDecrease);
        }
    }

    private void OnDestroy()
    {

        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChange.RemoveListener(IncreaseScoreOnHealthDecrease);
        }
    }

    // Method to increase score when enemy's health decreases
    private void IncreaseScoreOnHealthDecrease()
    {
        int scoreIncrease = Mathf.RoundToInt((enemyHealth.previousHealth - enemyHealth.currentHealth) * scoreIncreaseAmount);

        scoreIncrease = Mathf.Max(scoreIncrease, 0);

        score += scoreIncrease;

        UpdateScoreText();

        Debug.Log("Score increased by: " + scoreIncrease + ". Current Score: " + score);
    }

    // Method to update the TextMeshProUGUI component with the current score
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = " " + score;
        }
    }
}
