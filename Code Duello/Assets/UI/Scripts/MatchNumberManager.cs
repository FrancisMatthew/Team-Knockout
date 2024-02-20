using UnityEngine;

public class MatchNumberManager : MonoBehaviour
{
    // Singleton instance
    public static MatchNumberManager Instance { get; private set; }

    // Match number key for PlayerPrefs
    private const string MatchNumberKey = "MatchNumber";

    // Current match number
    private int matchNumber = 1;

    // Property to access the current match number
    public int MatchNumber
    {
        get { return matchNumber; }
        set
        {
            matchNumber = value;
            // Save the match number when it's changed
            SaveMatchNumber(matchNumber);
        }
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Load the match number when the game starts
        LoadMatchNumber();
    }

    // Save the match number to PlayerPrefs
    private void SaveMatchNumber(int match)
    {
        PlayerPrefs.SetInt(MatchNumberKey, match);
        PlayerPrefs.Save();
    }

    // Load the match number from PlayerPrefs
    private void LoadMatchNumber()
    {
        if (PlayerPrefs.HasKey(MatchNumberKey))
        {
            matchNumber = PlayerPrefs.GetInt(MatchNumberKey);
        }
    }

    // Reset the match number to its default value
    public void ResetMatchNumber()
    {
        matchNumber = 1;
        SaveMatchNumber(matchNumber);
    }
}
