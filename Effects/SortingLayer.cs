using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEditor;
[InitializeOnLoad]

public class SpriteSorter : MonoBehaviour
{
    static SpriteSorter()
    {
        Initialize();
    }
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 1.0f);
    }
}
