using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item, IHandleItem
{
    Character characterParent;


    private void Start()
    {
        DragAndDrop.onItemAssigned += HandleItem;
    }

    private void OnDisable()
    {
        DragAndDrop.onItemAssigned -= HandleItem;
    }

    public void HandleItem(Character character, Item item)
    {
        if (item == this)
        {
            characterParent = character;
            character.equippedItem = this;
        }
        else if (item != this.gameObject && characterParent == character && item.isEquippable)
        {
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }
}