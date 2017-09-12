using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;


public class Wifi : Connection {
	
	private enum MsgType : short { Debug, Op }

	public bool isHost;
	
	private string localIp;
	private NetworkClient localClient, remoteClient;
	private Queue<string> messages;


	public void Start(){

		this.localIp = GetLocalIp();
		Debug.Log("[Debug]: Creating a new Wifi");
		Debug.Log("[Debug]: localIp: " + localIp);

		this.localClient = null;
		this.remoteClient = null;
		this.isHost = false;
		this.autoCreatePlayer = false;
		this.messages = new Queue<string>();
	}

	public override bool Connect(){
		
		Debug.Log("[DEBUG] Connecting...");

		if(isHost){
			
			Debug.Log("[DEBUG] Host is up");
			this.localClient = StartHost();
			
			NetworkServer.RegisterHandler((short) MsgType.Op, HandleMessage);

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
		
		// msg.operation = msg;
		// msg.debug = "[debug]: Message + " + id;

		StringMessage sendMsg = new StringMessage();
		sendMsg.value = msg;

        return remoteClient.Send((short) MsgType.Op, sendMsg);
    }
	
	public override string GetMessage(){

		string msg = null;

		if(messages.Count > 0){
			Debug.Log("[Debug] New message(s)");
			msg = messages.Dequeue();
		}

        return msg;
    }

    public void HandleMessage(NetworkMessage msg){
    	messages.Enqueue(msg.ReadMessage<StringMessage>().value);
    	Debug.Log("[Debug] Message handler received: \"" + messages.Peek() + "\"");
    }
	
	public override bool CloseConnection(){
		NetworkManager.Shutdown();
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
