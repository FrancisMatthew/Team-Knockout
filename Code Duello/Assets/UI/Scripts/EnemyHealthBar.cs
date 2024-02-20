using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthBar : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public UnityEvent OnHealthChange = new UnityEvent();
    public float previousHealth; 

    public HealthBar healthbar;

    void Start()
    {
        currentHealth = maxHealth;
        previousHealth = currentHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(int damage)
    {
        previousHealth = currentHealth; 
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
        OnHealthChange.Invoke();
    }
}
