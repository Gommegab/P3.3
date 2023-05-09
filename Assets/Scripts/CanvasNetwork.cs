using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CanvasNetwork : NetworkBehaviour
{

    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clienButton;
    [SerializeField] private Button ChangeColor;

    private void Awake()
    {
        hostButton.onClick.AddListener(()
            =>
        {
            NetworkManager.Singleton.StartHost();
            DesActive("ButtonStarting", false);

        }
            );
        serverButton.onClick.AddListener(()
            =>
        {
            NetworkManager.Singleton.StartServer();
            DesActive("ButtonStarting", false);

        }
            );
        clienButton.onClick.AddListener(()
            =>
        {
            NetworkManager.Singleton.StartClient();
            DesActive("ButtonStarting", false);

        }
            );


        ChangeColor.onClick.AddListener(()
         =>
        {
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            {
                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerNetwork>().ChangeColor();
            }
            else
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<PlayerNetwork>();
                player.ChangeColor();
            }

        }
       );
    }


    private void DesActive(string go, bool b)
    {
        GameObject.Find("ButtonStarting").SetActive(b);
    }


}
