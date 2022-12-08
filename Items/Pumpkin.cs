using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : Item, IHandleItem
{
    [SerializeField] int damageBonus;
    [SerializeField] int healthBonus;
    string test;

    private void Awake()
    {
        DragAndDrop.onItemAssigned += HandleItem;
    }

    private void OnDestroy()
    {
        DragAndDrop.onItemAssigned -= HandleItem;
    }

    public void HandleItem(Character character, Item item)
    {
        Debug.Log("we are subscribed");
        if (item == this)
        {
            Debug.Log("trying to add");
            character.Damage += damageBonus;
            character.Health += healthBonus;
            Destroy(gameObject, 1f);
        }
    }
}