using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{

    public NetworkVariable<int> IdMaterial = new NetworkVariable<int>(0);
    public List<Color> colorPlayer = new List<Color>();

    public override void OnNetworkSpawn()
    {
        IdMaterial.Value = GenerarColor();

    }
    public void ChangeColor()
    {
        if (!IsOwner) return;
        if (NetworkManager.Singleton.IsServer)
        {
            int tempNum = GenerarColor();
            IdMaterial.Value = tempNum != -1 ? tempNum : IdMaterial.Value;
            GetComponent<Renderer>().material.color = colorPlayer[IdMaterial.Value];
        }
        else
        {
            ChangeColorServerRpc();
        }
    }


    [ServerRpc]
    void ChangeColorServerRpc()
    {
        int tempNum = GenerarColor();
        IdMaterial.Value = tempNum != -1 ?  tempNum:IdMaterial.Value ;
      
    }
    private void Update()
    {
        if (GetComponent<Renderer>().material.color != colorPlayer[IdMaterial.Value])
        {
            GetComponent<Renderer>().material.color = colorPlayer[IdMaterial.Value];
        }
    }


    int GenerarColor()
    {
        int nu = 0;
        List<int> numeros = new List<int>();
        foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
        {
            numeros.Add(NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerNetwork>().IdMaterial.Value);

        }

        if (numeros.Count <= 0)
        {
            return nu;
        }
        else if (numeros.Count > 10)
        {
            Debug.Log("No se pueden generar más números aleatorios. Jugador es fuera de la partida");

            return -1;
        }
        else
        {
            while (numeros.Count < colorPlayer.Count)
            {
                nu = Random.Range(0, colorPlayer.Count);

                if (!numeros.Contains(nu))
                {
                    Debug.Log($"Número generado: { nu}");
                    break;
                }
            }
            return nu;
        }
    }


    void CloseClient()
    {
        //NetworkManager.Singleton.StopClient(); no existe en esta version y en la documentacion asocia a 
        //NetworkManager.Singleton.Shutdown(); pero este cierra la conexion del servidor
        //TODO 
        
    }

}