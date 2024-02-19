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
    public MovementController playerMC;

    public float health = 25f;

    public Slider healthSlider;

    [SerializeField] private float damageMultiplyerChest = 1;
    [SerializeField] private float damageMultiplyerHead = 1.5f;

    private void Update()
    {
        healthSlider.value = health;
    }

    public void TakeDamage(HitType hitType, float damageIntake) 
    {
        if (playerMC.isBlocking)
        {
            BockHit();
            return;
        }

        if (hitType == HitType.Head) 
        {
            Debug.Log("HIT: " + hitType);
            health -= damageIntake * damageMultiplyerHead;
            Debug.Log(damageIntake * damageMultiplyerHead);
            playerMC.ApplyKnockback();
        }
        if(hitType == HitType.Chest) 
        {
            
            Debug.Log("HIT: " + hitType);
            health -= damageIntake * damageMultiplyerChest;
            Debug.Log(damageIntake * damageMultiplyerChest);
            playerMC.ApplyKnockback();
        }
        
    }

    public void BockHit() 
    { 
    
    }

}
