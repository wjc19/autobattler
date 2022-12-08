using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item, IHandleItem
{
    string test;

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
        throw new System.NotImplementedException();
    }
}