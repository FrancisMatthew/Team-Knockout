using UnityEngine;

public class ActivateScreenWithSound : MonoBehaviour
{
    public GameObject screenToActivate;
    public AudioClip activationSound;
    public float activationDuration = 3f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        ActivateScreen();
    }

    void ActivateScreen()
    {

        if (screenToActivate != null)
        {
            screenToActivate.SetActive(true);
        }
        else
        {
            Debug.LogError("Screen to activate is not assigned!");
        }


        if (activationSound != null && audioSource != null)
        {

            audioSource.PlayOneShot(activationSound);
        }

        Invoke("DeactivateScreen", activationDuration);
    }

    void DeactivateScreen()
    {
        if (screenToActivate != null)
        {
            screenToActivate.SetActive(false);
        }
    }
}
