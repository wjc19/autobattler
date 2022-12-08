using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMe : MonoBehaviour
{
    [SerializeField] Canvas button;

    public void SetButton(bool setButton)
    {
        Debug.Log("This was called");
        button.enabled = setButton;
    }

}
