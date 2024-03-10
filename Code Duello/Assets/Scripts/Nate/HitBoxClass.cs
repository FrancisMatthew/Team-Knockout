using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitBoxClass : MonoBehaviour
{
    [SerializeField] private PlayerHealthClass playerPHC;

    [SerializeField] private float damageMultiplyer;

    public void TakeDamage(int damageIntake)
    {
        playerPHC.health = damageIntake * damageMultiplyer;
        Debug.Log(damageIntake * damageMultiplyer);
    }

    


    
}
