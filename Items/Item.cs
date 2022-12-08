using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public int cost;
    [SerializeField] public bool isConsumable;
    [SerializeField] public bool isEquippable;   
    [SerializeField] public bool isPurchased;
    [SerializeField] public string itemClass;

    public static Action<Item> onItemConsumed;

    private void Start()
    {
        DragAndDrop.onItemAssigned += HandleItemConsumed;
        TeamBuilder.onItemAssigned += HandleItemConsumed;
    }

    private void OnDisable()
    {
        DragAndDrop.onItemAssigned -= HandleItemConsumed;
        TeamBuilder.onItemAssigned -= HandleItemConsumed;
        if (isConsumable && TeamPosition.isHoveringOverTeamPosition)
        {
            onItemConsumed?.Invoke(this);
        }
    }

    private void HandleItemConsumed(Character character, Item consumedItem)
    {
        Debug.Log("Trying");
        if (consumedItem.gameObject == this.gameObject)
        {
            isPurchased = true;
            Debug.Log("Succeeded");
        }
    }
}
