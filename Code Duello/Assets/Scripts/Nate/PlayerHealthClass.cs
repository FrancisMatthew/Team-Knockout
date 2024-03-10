using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum HitType 
{ 
    Head,
    Chest,
    Legs
}


public class PlayerHealthClass : MonoBehaviour
{

    [Header("PlayerMC Refs")] [SerializeReference] [Space]

    public MovementController playerMC;

    [Header("Health")][SerializeReference][Space]

    public float health = 25f;
    public Slider healthSlider;

    [Header("Damage Multiplyers")] [SerializeReference] [Space]
    [SerializeField] private float damageMultiplyerChest = 1;
    [SerializeField] private float damageMultiplyerHead = 1.5f;
    [SerializeField] private float blockMultiplyer = 0.2f;

    private void Update()
    {
        healthSlider.value = health;
        if(health <= 0) 
        {
            playerMC.KillPlayer();
        }
    }

    public void TakeDamage(HitType hitType, float damageIntake) 
    {
        if (playerMC.isBlocking)
        {
            BlockHit(damageIntake);
            return;
        }

        if (hitType == HitType.Head)
        {
            playerMC.DamageAnim();
            Debug.Log("HIT: " + hitType);
            health -= damageIntake * damageMultiplyerHead;
            Debug.Log(damageIntake * damageMultiplyerHead);
            playerMC.ApplyKnockback();
        }
        if(hitType == HitType.Chest) 
        {
            playerMC.DamageAnim();
            Debug.Log("HIT: " + hitType);
            health -= damageIntake * damageMultiplyerChest;
            Debug.Log(damageIntake * damageMultiplyerChest);
            playerMC.ApplyKnockback();
        }
    }

    public void BlockHit(float damageIntake) 
    {
        health -= damageIntake * blockMultiplyer;
        Debug.Log(damageIntake * blockMultiplyer);
    }

}
