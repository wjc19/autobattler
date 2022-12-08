using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Box : MonoBehaviour
{
    [SerializeField] SpriteRenderer Border;
    [SerializeField] bool isRefundPosition;

    private void Start()
    {
        DragAndDrop.onCharacterSelect += ShowBox;
        DragAndDrop.onCharacterDeselect += HideBox;
    }
    private void OnDisable() {
        DragAndDrop.onCharacterSelect -= ShowBox;
        DragAndDrop.onCharacterDeselect -= HideBox;
    }

    public void ShowBox(Character character)
    {
        if (isRefundPosition && !DragAndDrop.selectedCharacter.isTeamMember)
        {
            return;
        }
        Border.enabled = true;
    }

    public void HideBox(Character character)
    {
        Border.enabled = false;
    }
}
