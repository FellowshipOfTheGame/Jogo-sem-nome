using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Wifi : Connection {

	public bool isHost;
	private string localIp;
	private NetworkClient localClient, remoteClient;

	public void Start(){

		this.localIp = GetLocalIp();
		Debug.Log("[Debug]: Creating a new Wifi");
		Debug.Log("[Debug]: localIp: " + localIp);

		this.localClient = null;
		this.remoteClient = null;
		this.isHost = false;
		this.autoCreatePlayer = false;
	}

	public override bool Connect(){
		
		Debug.Log("[DEBUG] Connecting...");

		if(isHost){
			Debug.Log("[DEBUG] Host is up");
			this.localClient = StartHost();
		} else {
			Debug.Log("[DEBUG] Host ip: " + networkAddress);
			Debug.Log("[DEBUG] Host port: " + networkPort);
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
		GameObject.Destroy(this);
        return true;
    }

    public void SetPlayerPrefab(GameObject pp){
    	this.playerPrefab = pp;
    }

    public void SetIpAddress(string remoteIp){ this.networkAddress = remoteIp; }

    public string GetLocalIp(){ return Network.player.ipAddress; }
    public string GetRemoteIp(){ return this.networkAddress; }

    public void OnConnected(NetworkConnection conn, NetworkReader reader) {
		Debug.Log("Connected!");
		GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>().LoadWifiBattle();
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
