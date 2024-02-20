using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthClass : MonoBehaviour
{
    public float health = 25f;
    public Slider healthSlider;
    public Image fillImage;

    private void Start()
    {
        // Set the initial value of the health slider
        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DamageBox"))
        {
            // Decrease the player's health
            health -= 10f;

            // Update the health slider value
            if (healthSlider != null)
            {
                healthSlider.value = health;
            }

            // Check if the player's health has reached zero
            if (health <= 0f)
            {
                // Perform actions when player health is zero (e.g., game over)
                Debug.Log("Player defeated!");
            }
        }
    }
}
