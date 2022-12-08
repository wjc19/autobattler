using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Ghost : Character
{
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] GameObject haunt;
    [SerializeField] int hauntWeakenInt = 2;
    string test = "test";

    private void Awake() 
    {
        Character.onDeath += Haunt;
    }
    private void OnDestroy()
    {
        Character.onDeath -= Haunt;
    }

    private void Haunt(Character character, Character attacker)
    {
        if (character.gameObject == this.gameObject)
        {
            
            Instantiate(haunt, attacker.transform.position, Quaternion.identity, attacker.transform);
            attacker.Damage -= hauntWeakenInt;
        }
    }
}


