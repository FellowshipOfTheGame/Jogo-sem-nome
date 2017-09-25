using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;


public class Wifi : Connection {
	
	private enum MyMsgType : short { Debug = 100, Op }

	public static bool con = false;

	public bool isHost;

	private string localIp;
	static private NetworkClient localClient, remoteClient;
	static private Queue<string> messages;

	public void Update(){
		if(Wifi.con){
			Wifi.con = false;
			OtterSendMessage("TEST MESSAGE");
		}
	}

	public void Start(){

		this.localIp = GetLocalIp();

		localClient = null;
		remoteClient = null;
		this.isHost = false;
		this.autoCreatePlayer = false;
		messages = new Queue<string>();
	}

	public override bool Connect(){
		
		if(isHost){
			
			Debug.Log("[DEBUG] Starting host...");
			this.networkPort = 7777;
			localClient = StartHost();
			localClient.RegisterHandler(MsgType.Connect, OnHostConnected);
			localClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
			localClient.RegisterHandler(MsgType.Error, OnError);

		} else {

			Debug.Log("[DEBUG] Initializing client...");
			Debug.Log("[DEBUG] Connecting to: " + networkAddress + ":" + networkPort);

			this.networkPort = 7777;
			remoteClient = StartClient();

			if(remoteClient == null){
				Debug.Log("[FATAL] Failed to initialize client.");
				return false;
			}
			remoteClient.RegisterHandler((short) MyMsgType.Op, HandleMessage);
			remoteClient.RegisterHandler(MsgType.Connect, OnConnected);
			remoteClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
			remoteClient.RegisterHandler(MsgType.Error, OnError);
			remoteClient.Connect(networkAddress, networkPort);
		}

		return true;
	}

	public override bool OtterSendMessage(string msg){
		
		Debug.Log("[Debug] Message: " + msg);

		StringMessage sendMsg = new StringMessage();
		sendMsg.value = msg;

		// Debug.Log("[Debug] remoteClient: " + remoteClient);

		return remoteClient.Send((short) MyMsgType.Op, sendMsg);
		return false;
	}

	public override string GetMessage(){

		string msg = null;

		if(messages.Count > 0){
			Debug.Log("[Debug] New message(s)");
			msg = messages.Dequeue();
		}

		return msg;
	}

	/*****************************/
	/* Network Manager Functions */
	/*****************************/

	public void HandleMessage(NetworkMessage msg){
		messages.Enqueue(msg.ReadMessage<StringMessage>().value);
		Debug.Log("[Debug] Message handler received: \"" + messages.Peek() + "\"");
	}

	public override bool CloseConnection(){
		NetworkManager.Shutdown();
		GameObject.Destroy(this);
	    return true;
	}

	public void SetPlayerPrefab(GameObject pp){ this.playerPrefab = pp; }
	public void SetIpAddress(string remoteIp){ this.networkAddress = remoteIp; }

	public string GetLocalIp(){ return Network.player.ipAddress; }
	public string GetRemoteIp(){ return this.networkAddress; }

	public override void OnStartServer() {
		Debug.Log("[Debug]: Server started!");	
		NetworkServer.RegisterHandler((short) MyMsgType.Op, HandleMessage);
	}

	public override void OnStartHost() {
		Debug.Log("[Debug]: Host started!");	
		NetworkServer.RegisterHandler((short) MyMsgType.Op, HandleMessage);
	}

	public override void OnClientConnect(NetworkConnection conn){
		Debug.Log("[Debug] OnClientConnect");
	}

	public void OnConnected(NetworkMessage netMsg){
		Wifi.con = true;
		Debug.Log("Connected!");
		// OtterSendMessage("TEST MESSAGE");
		// GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>().LoadWifiBattle();
	}
	public void OnDisconnected(NetworkMessage netMsg){ Debug.Log("[Debug]: Disconnected from server"); }
	public void OnHostConnected(NetworkMessage netMsg){ Debug.Log("[Debug]: Host connected!"); }
	public void OnError(NetworkMessage netMsg){ Debug.Log("[ERROR]: Error connecting - Vish deu ruim :c"); }
}
