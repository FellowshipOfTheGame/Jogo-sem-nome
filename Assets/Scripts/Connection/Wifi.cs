using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Wifi : Connection {

	private bool isHost;
	private string localIp;
	private string ip;
	private NetworkClient localClient, remoteClient;
	private NetworkManager net;

	public Wifi(){
		net = new NetworkManager();
		localIp = GetLocalIp();
		ip = null;
	}


	public override bool Connect(){
		
		if(isHost)
			localClient = net.StartHost();
		else {
			net.networkPort = 7777;
			remoteClient = net.StartClient();
		}

		return true;
    }
	
	public override bool SendMessage(){
        return true;
    }
	
	public override bool GetMessage(){
        return true;
    }
	
	public override bool CloseConnection(){
		NetworkManager.Shutdown();
        return true;
    }

    public void SetIpAddress(string ip){
    	net.networkAddress = ip;
    }



    public string GetLocalIp(){
		return Network.player.ipAddress;
    }

    public void OnConnected(NetworkConnection conn, NetworkReader reader) {
		Debug.Log("Connected!");
	}

	public void OnConnectedToServer(){
		Debug.Log("Connected to server!");
	}

	public void OnDisconnected(NetworkConnection conn, NetworkReader reader) {
		Debug.Log("Disconnected from server");
	}

	public void OnError(NetworkConnection conn, NetworkReader reader) {
		Debug.Log("Error connecting with message: Vish deu ruim :c");
	}

}
