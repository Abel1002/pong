using System;
using System.Collections.Generic;
using Unity.Networking.Transport;
using Unity.Collections;
using UnityEngine;

public class Servidor : MonoBehaviour
{
    public string ipAddress = "127.0.0.1";
    public ushort port = 9000;
    public NetworkDriver network_driver;
    private NativeList<NetworkConnection> connexions;

    private Dictionary<int, NetworkConnection> connexions_dic = new Dictionary<int, NetworkConnection>();
    private int total_connexions = 0;

    void Start()
    {
        string ipAddressCanvas = MenuInicialManager.Instance.InputFieldIP.text;
        ushort.TryParse(MenuInicialManager.Instance.InputFieldPort.text, out var portCanvas);
        ipAddress = string.IsNullOrEmpty(ipAddressCanvas) ? ipAddress : ipAddressCanvas;
        port = portCanvas == 0 ? port : portCanvas;

        //creem el driver i l'intentem vincular a l'adreça indicada
        network_driver = NetworkDriver.Create();
        var servidor_ws = NetworkEndPoint.Parse(ipAddress, port);
        if (network_driver.Bind(servidor_ws) != 0)
        {
            Debug.Log("SERVIDOR::No ens hem pogut vincular amb el port:" + servidor_ws.Port + " i IP:" + servidor_ws.Address);
        }
        else
        {
            //iniciem la llista que emmagatzemi les conexions
            connexions = new NativeList<NetworkConnection>(4, Allocator.Persistent);
            //ens quedem a l'espera de noves conexion
            network_driver.Listen();
            Debug.Log("SERVIDOR:: a la espera en el port:" + servidor_ws.Port + " i IP:" + servidor_ws.Address);
        }
    }

}