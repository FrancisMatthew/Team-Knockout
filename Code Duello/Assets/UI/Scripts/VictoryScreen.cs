using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class VictoryScreen : MonoBehaviour
{
    public GameObject victory;
    public GameObject GameOverScrn;
    public EnemyHealthBar enemyhealth;
    public PlayerHealth playerHealth;
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI gameOverText;
    public Gradient textColorGradient; // Define your color gradient in the inspector
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI for displaying the score

    private Coroutine colorTransitionCoroutine;

    void Start()
    {
        // Set the initial score text
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        ActivateVictoryScreen();
        ActivateGameOverScreen();

        if (victory.activeSelf || GameOverScrn.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                reActivateGameScene();
            }
        }
    }

    public void reActivateGameScene()
    {
        SceneManager.LoadScene("GameLevel");
        // Increment the match number and update the score text
        MatchNumberManager.Instance.MatchNumber++;
        UpdateScoreText();
    }

    public void ActivateVictoryScreen()
    {
        if (enemyhealth.currentHealth <= 0 && !victory.activeSelf)
        {
            victory.SetActive(true);
            colorTransitionCoroutine = StartCoroutine(TextColorTransition(victoryText));
        }
    }

    public void ActivateGameOverScreen()
    {
        if (playerHealth.currentHealth <= 0 && !GameOverScrn.activeSelf)
        {
            GameOverScrn.SetActive(true);
            colorTransitionCoroutine = StartCoroutine(TextColorTransition(gameOverText));
        }
    }

    IEnumerator TextColorTransition(TextMeshProUGUI text)
    {
        while (true)
        {
            float duration = 2f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                Color textColor = textColorGradient.Evaluate(t);

                text.color = textColor;

                yield return null;

                elapsedTime += Time.deltaTime;
            }

            // Reverse the gradient for the next iteration
            textColorGradient = ReverseGradient(textColorGradient);
        }
    }

    Gradient ReverseGradient(Gradient originalGradient)
    {
        Gradient reversedGradient = new Gradient();

        // Swap the colors at the start and end points
        reversedGradient.SetKeys(
            new GradientColorKey[] { originalGradient.colorKeys[1], originalGradient.colorKeys[0] },
            new GradientAlphaKey[] { originalGradient.alphaKeys[1], originalGradient.alphaKeys[0] }
        );

        return reversedGradient;
    }

    void UpdateScoreText()
    {
        // Update the score text with the current match number
        scoreText.text = "Match No: " + MatchNumberManager.Instance.MatchNumber;
    }
}
