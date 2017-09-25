using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;


public class Wifi : Connection {
	
	private enum MyMsgType : short { Debug = 100, Op }

	public bool isHost;

	private string localIp;
	private NetworkClient localClient, remoteClient;
	private Queue<string> messages;

	public void Start(){

		this.localIp = GetLocalIp();

		localClient = null;
		remoteClient = null;
		this.isHost = false;
		this.autoCreatePlayer = false;
		messages = new Queue<string>();
	}

	public void Update(){

		if(Input.GetKeyDown(KeyCode.Space)){

			Debug.Log("[Debug] Server connections: ");
			foreach (NetworkConnection nc in NetworkServer.connections)
				if(nc != null)
					Debug.Log("[Debug] Connection: " + nc.connectionId);

			// Debug.Log("[Debug] Message: " + msg);

			StringMessage sendMsg = new StringMessage();
			sendMsg.value = "batata";
			client.Send((short) MyMsgType.Op, sendMsg);
		}
	}

	public override bool Connect(){
		
		if(isHost){
			
			this.networkPort = 7777;
			localClient = StartHost();
			
			localClient.RegisterHandler(MsgType.Connect, OnHostConnected);
			localClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
			localClient.RegisterHandler(MsgType.Error, OnError);

			Network.maxConnections = 2;

		} else {

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
		
		Debug.Log("[Debug] Sending message: " + msg);

		StringMessage sendMsg = new StringMessage();
		sendMsg.value = msg;

		return client.Send((short) MyMsgType.Op, sendMsg);
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
		
		if(msg.ReadMessage<StringMessage>().value.Equals("START"))
			GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>().LoadWifiBattle();
		else {
			messages.Enqueue(msg.ReadMessage<StringMessage>().value);
			Debug.Log("[Debug] Message handler received: \"" + messages.Peek() + "\"");
		}
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

	public override void OnStartHost() {
		Debug.Log("[Debug]: Host started!");	
		NetworkServer.RegisterHandler((short) MyMsgType.Op, HandleMessage);
	}

	public void OnConnected(NetworkMessage netMsg){
		Debug.Log("Connected!");
		OtterSendMessage("START");
		GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>().LoadWifiBattle();
	}
	
	public void OnDisconnected(NetworkMessage netMsg){ 
		Debug.Log("[Debug]: Disconnected from server");
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EndBattle();
		CloseConnection();
	}

	public void OnHostConnected(NetworkMessage netMsg){ Debug.Log("[Debug]: Host connected!"); }
	public void OnError(NetworkMessage netMsg){ Debug.Log("[ERROR]: Error connecting - Vish deu ruim :c"); }
}
