using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneControl : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] Button quitGameButton;
    [SerializeField] InputControls inputControls;

    public static event Action<Scene> onSceneLoad;

    public static void NextScene(string scene)
    {
        SceneManager.LoadScene(scene);
        onSceneLoad?.Invoke(SceneManager.GetSceneByName("scene"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
