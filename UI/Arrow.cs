using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Arrow : MonoBehaviour
{
    [SerializeField] SpriteShapeRenderer arrowHead;
    [SerializeField] SpriteRenderer arrowBody;
    [SerializeField] bool isRefundPosition;

    private void Start()
    {
        DragAndDrop.onCharacterSelect += ShowArrows;
        DragAndDrop.onCharacterDeselect += HideArrows;
    }
    private void OnDisable() {
        DragAndDrop.onCharacterSelect -= ShowArrows;
        DragAndDrop.onCharacterDeselect -= HideArrows;
    }

    public void ShowArrows(Character character)
    {
        if (isRefundPosition && !DragAndDrop.selectedCharacter.isTeamMember)
        {
            return;
        }
        arrowHead.enabled = true;
        arrowBody.enabled = true;
    }

    public void HideArrows(Character character)
    {
        arrowHead.enabled = false;
        arrowBody.enabled = false;
    }
}
