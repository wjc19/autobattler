using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Item, IHandleItem
{
    string test;

    private void Start()
    {
        DragAndDrop.onItemAssigned += HandleItemAssignment;
    }

    private void OnDisable()
    {
        DragAndDrop.onItemAssigned -= HandleItemAssignment;
    }

    private void HandleItemAssignment(Character arg1, Item arg2)
    {
        throw new NotImplementedException();
    }

    public void HandleItem(Character character, Item item)
    {
        throw new System.NotImplementedException();
    }
}