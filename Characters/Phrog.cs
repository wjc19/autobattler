using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phrog : Character
{
    public static event Action<Character, Item> onItemAssigned;
    [SerializeField] ParticleSystem particleSystem;
    string test = "test";
    
    private void Awake()
    {
        Character.onDeath += Explode;
        DragAndDrop.onItemAssigned += IncreaseHealth;
    }
    private void OnDestroy()
    {
        DragAndDrop.onItemAssigned -= IncreaseHealth;
        Character.onDeath -= Explode;
    }

    private void IncreaseHealth(Character character, Item usedItem)
    {
        if (character.gameObject == this.gameObject && usedItem.isConsumable)
        {
            Health += 1;
        } 
    }

    private void Explode(Character obj, Character attacker)
    {
        if (obj == this)
        {
            var explosion = Instantiate(particleSystem, transform.position, Quaternion.identity);
            Destroy(explosion, .2f);
        }
    }
}
