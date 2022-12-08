using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayersManager : NetworkBehaviour
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();
    public int PlayersInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if(IsServer)
            {  
                Debug.Log("Client has connected");
                playersInGame.Value++;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log("Client has disconnected");
                playersInGame.Value--;
            }
        };
    }
}
