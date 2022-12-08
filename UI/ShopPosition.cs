using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShopPosition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static ShopPosition shopPosition = null;
    [SerializeField] public TextMeshProUGUI costText;
    private int cost;

    public void OnPointerEnter(PointerEventData eventData)
    {
        shopPosition = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shopPosition = null;
    }

    private void Start()
    {
    }
}
