using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Wifi : Connection {

	private bool isHost { get; set; }
	private string localIp;
	private string ip;
	private NetworkClient localClient, remoteClient;
	private NetworkManager net;

	public Wifi(){
		Debug.Log("[Debug]: Creating a new Wifi");
		net = new NetworkManager();
		Debug.Log("[Debug]: net: " + net);
		localIp = GetLocalIp();
		Debug.Log("[Debug]: localIp: " + localIp);
		ip = null;
		localClient = null;
		remoteClient = null;
		net = null;
		isHost = false;
	}


	public override bool Connect(){
		
		if(isHost)
			localClient = net.StartHost();
		else {
			net.networkPort = 7777;
			remoteClient = net.StartClient();
			remoteClient.Connect(net.networkAddress, net.networkPort);
		}

		return true;
    }
	
	public override bool OtterSendMessage(string msg){
        return true;
    }
	
	public override string GetMessage(){
        return "NOOP";
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
