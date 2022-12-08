using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeamPosition : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public static int lastTeamPosition;
    [SerializeField] public int position;
    [SerializeField] TeamBuilder teamBuilder;
    public static bool isHoveringOverTeamPosition = false;
    public static int hoveredPosition;

    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringOverTeamPosition = false;
        if (DragAndDrop.isDragging && !isHoveringOverTeamPosition && DragAndDrop.selectedCharacter != null)
        {
            if (!DragAndDrop.selectedCharacter.isTeamMember && TeamBuilder.teamIsFull)
            {
                return;
            }
            teamBuilder.HandleRemove(null, position);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringOverTeamPosition = true;
        hoveredPosition = position;
        if (DragAndDrop.isDragging && isHoveringOverTeamPosition && DragAndDrop.selectedCharacter != null)
        {
            foreach (var character in teamBuilder.teamArray)
            {
                if (DragAndDrop.selectedCharacter == character)
                {
                    return;
                }
            }
            lastTeamPosition = position;
            Debug.Log("postion = "+ position);
            teamBuilder.HandleInsert(DragAndDrop.selectedCharacter, position);
        }
    }
}
