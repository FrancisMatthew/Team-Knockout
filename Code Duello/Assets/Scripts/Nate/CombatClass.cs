using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatClass : MonoBehaviour
{
    [SerializeField] private int baseDamage;

    public bool attackLanded = false;
    public HitBoxClass hbc = null;

    public void DamageHitCkeck() 
    {
        if (attackLanded) 
        {
            //hbc.TakeDamage(baseDamage);
            attackLanded = false;
            hbc = null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DamageBox"))
        {
            hbc = other.gameObject.GetComponent<HitBoxClass>();
            attackLanded = true;
        }
    }
}
