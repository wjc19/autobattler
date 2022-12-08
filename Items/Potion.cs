using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item, IHandleItem
{
    [SerializeField] int bonusDamage;

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
        if (item.gameObject == this.gameObject)
        {
            character.Damage += bonusDamage;
            Destroy(gameObject, 1f);
        }
    }
}