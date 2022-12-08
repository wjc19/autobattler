using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] public int souls;

        
    private void Start() {
        souls = 10;
    }

    public Action<int> onSoulsChanged;

    public int Souls
    {
        get { return souls; }
        set
        {
            souls = value;
            this?.onSoulsChanged(souls);
        }
    }
}
