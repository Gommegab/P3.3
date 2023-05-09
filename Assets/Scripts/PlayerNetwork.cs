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
            IdMaterial.Value = GenerarColor();
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
        IdMaterial.Value = GenerarColor();

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
            Debug.Log("No se pueden generar m�s n�meros aleatorios. Jugador es fuera de la partida");
            return 0;
        }
        else
        {
            // generar n�meros aleatorios mientras haya n�meros posibles que no se hayan generado
            while (numeros.Count < colorPlayer.Count)
            {
                nu = Random.Range(0, colorPlayer.Count); // generar un n�mero aleatorio entre 0 y 9

                // si el n�mero generado ya est� en la lista, generar otro n�mero aleatorio
                if (!numeros.Contains(nu))
                {
                    Debug.Log($"N�mero generado: { nu}");

                    break;
                }
            }

            return nu;

        }
    }

}