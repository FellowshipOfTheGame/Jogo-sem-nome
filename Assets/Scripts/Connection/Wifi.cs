using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Wifi : Connection {

	public bool isHost { get; set; }
	private string localIp;
	private NetworkClient localClient, remoteClient;

	public void Start(){

		this.localIp = GetLocalIp();
		Debug.Log("[Debug]: Creating a new Wifi");
		Debug.Log("[Debug]: localIp: " + localIp);

		this.localClient = null;
		this.remoteClient = null;
		this.isHost = false;
	}


	public override bool Connect(){
		
		if(isHost)
			this.localClient = StartHost();
		else {
			this.networkPort = 7777;
			this.remoteClient = StartClient();
			this.remoteClient.Connect(networkAddress, networkPort);
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

    public void SetIpAddress(string remoteIp){ this.networkAddress = remoteIp; }

    public string GetLocalIp(){ return Network.player.ipAddress; }
    public string GetRemoteIp(){ return this.networkAddress; }

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
		Debug.Log("Error connecting - Vish deu ruim :c");
	}

}
