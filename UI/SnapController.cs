using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapController : MonoBehaviour
{   
    bool trySnap;
    int snapDistance = 5;
    Character snappedCharacter = null;

    // // private void Start() {
    // //     DragAndDrop.onDrop += TrySnap;
    // // }

    // // private void OnDisable() {
    // //     DragAndDrop.onDrop -= TrySnap;
    // // }

    // // public void TrySnap(Character character)
    // // {
    // //     if (Vector3.Distance(gameObject.transform.position, character.gameObject.transform.position) <= snapDistance)
    // //     {
    //         // character.transform.SetParent(gameObject.transform);
    // //         // character.transform.position = transform.position;
    // //     }
    //     // else
    //     // {
    //     //     return false;
    //     // }
    // }

    public Character GetSnappedCharacter()
    {
        return snappedCharacter;
    }

    public void SetSnappedCharacter(Character character)
    {
        snappedCharacter = character;
    }
}
