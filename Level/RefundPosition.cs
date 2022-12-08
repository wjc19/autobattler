using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RefundPosition : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public static bool isHoveringOverRefundPosition;
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringOverRefundPosition = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringOverRefundPosition = true;
    }
}
