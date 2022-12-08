using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item, IHandleItem
{
    [SerializeField] int armor;
    [SerializeField] Character characterParent;

    private void Awake()
    {
        DragAndDrop.onItemAssigned += HandleItem;
        TeamBuilder.onItemAssigned += HandleItem;
    }

    private void OnDestroy()
    {
        DragAndDrop.onItemAssigned -= HandleItem;
        TeamBuilder.onItemAssigned -= HandleItem;
    }

    public void ReEquipItem(Character character, Item item) {
            characterParent = character;
            character.equippedItem = this;
            character.armor = armor;
    }

    public void HandleItem(Character character, Item item)
    {
        if (item == this || item == character.equippedItem)
        {
            characterParent = character;
            character.equippedItem = this;
            character.armor = armor;
        }
        else if (item != this && characterParent == character && item.isEquippable)
        {
            characterParent.armor = 0;
            Destroy(gameObject);
        }
        else 
        {
            return;
        }
    }
}

