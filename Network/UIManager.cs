using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField] 
    private Button startClientButton;

    [SerializeField] 
    private TextMeshProUGUI playersInGameText;

    [SerializeField]
    private GameObject registerCanvas;

    [SerializeField]
    private GameObject loginCanvas;

    private void Awake()
    {
        // Cursor.visible = true;
    }

    private void Update() 
    {
        // playersInGameText.text = $"Players in Game: {PlayersManager.Instance.PlayersInGame}";
    }

    private void Start() 
    {
        startHostButton.onClick.AddListener(() =>
        {
            if(NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host connected to server");
            }
            else
            {
                Debug.Log("Host could not be started");
            }
        });
        startServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                Debug.Log("Server Started");
            }
            else
            {
                Debug.Log("Server could not be started");
            }
        });
        startClientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client connected");
            }
            else
            {
                Debug.Log("Client failed to connect");
            }
        });
    }

    public void EnableRegisterCanvas()
    {
        registerCanvas.SetActive(true);
        loginCanvas.SetActive(false);
    }
    public void EnableLoginCanvas()
    {
        registerCanvas.SetActive(false);
        loginCanvas.SetActive(true);
    }
}
