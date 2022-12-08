using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamSetText : MonoBehaviour
{
    [SerializeField] public int teamSetCount = 0;
    [SerializeField] string setName;
    [SerializeField] TextMeshProUGUI teamText;
    [SerializeField] public CanvasGroup setTextGroup;

    public int TeamSetCount
    {
        get { return teamSetCount; } 
        set
        {
            teamSetCount = value;
            SetText(teamSetCount);
        }
    }

    private void SetText(int count)
    {
        teamText.text = string.Format("{0}: {1}", setName, teamSetCount);
    }
}
