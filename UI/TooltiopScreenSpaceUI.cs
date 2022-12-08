using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltiopScreenSpaceUI : MonoBehaviour
{
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private TextMeshProUGUI textBox;



    private void SetText(string tooltipText)
    {
        textBox.SetText(tooltipText);
        textBox.ForceMeshUpdate();

        Vector2 textSize = textBox.GetRenderedValues(false);

        backgroundRectTransform.sizeDelta = textSize;
    }

    private void Awake()
    {
        SetText("Hello World!");
    }
}
