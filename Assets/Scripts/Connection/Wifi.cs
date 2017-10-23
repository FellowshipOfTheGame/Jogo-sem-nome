using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;

public class Wifi : Connection {
	
	public bool isHost;

	private GameManager gm;
	private short type = (short) MyMsgType.Null;
	private string localIp;
	private NetworkClient localClient, remoteClient;
	private Queue<string> actions;
	private Queue<int> configs;

	public void Start(){

		this.localIp = GetLocalIp();

		this.localClient = null;
		this.remoteClient = null;
		this.isHost = false;
		this.autoCreatePlayer = false;
		this.actions = new Queue<string>();
		this.configs = new Queue<int>();

		this.gm = GameObject.FindGameObjectWithTag("GameManager").
			GetComponent<GameManager>();
	}

	public void Update(){

		/* Debug */
		if(Input.GetKeyDown(KeyCode.Space)){

			Debug.Log("[Debug] Server connections: ");
			foreach (NetworkConnection nc in NetworkServer.connections)
				if(nc != null)
					Debug.Log("[Debug] Connection: " + nc.connectionId);

			// Debug.Log("[Debug] Message: " + msg);

			StringMessage sendMsg = new StringMessage();
			sendMsg.value = "batata";
			client.Send((short) MyMsgType.Action, sendMsg);
		}
	}

	/**********************************/
	/* Connection Interface Functions */
	/**********************************/

	public override bool Connect(){
		
		if(isHost){
			
			this.networkPort = 7777;
			localClient = StartHost();
			
			localClient.RegisterHandler(MsgType.Connect, OnHostConnected);
			// localClient.RegisterHandler(MsgType.Config, config callback);
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

			remoteClient.RegisterHandler((short) MyMsgType.Action, HandleActionMessage);
			remoteClient.RegisterHandler((short) MyMsgType.Config, HandleConfigMessage);
			remoteClient.RegisterHandler(MsgType.Connect, OnConnected);
			remoteClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
			remoteClient.RegisterHandler(MsgType.Error, OnError);
			// networkAddress = "172.28.143.20"; // Debug
			remoteClient.Connect(networkAddress, networkPort);
		}

		return true;
	}

	// These functions always set msg type to null to force user to always set 
	// which message type to send (safer)
	public override bool OtterSendMessage(string msg){
		
		if(this.type == (short) MyMsgType.Null)
			throw new WifiConnectionException("No message type defined");

		Debug.Log("[Debug] Sending message: " + msg);

		StringMessage sendMsg = new StringMessage();
		sendMsg.value = msg;

		SetMessageType(MyMsgType.Null);
		return client.Send(this.type, sendMsg);
	}

	// These functions always set msg type to null to force user to always set 
	// which message type to send (safer)
	public override bool GetMessage(ref object retVal){

		bool ret = true;
		retVal = null;

		if(this.type == (short) MyMsgType.Null)
			throw new WifiConnectionException("No message type defined");

		switch(this.type){
		case (short) MyMsgType.Config:
			if(configs.Count > 0){
				Debug.Log("[Debug] New message(s)");
				retVal = configs.Dequeue(); // Int message
			} else ret = false;
			break;

		case (short) MyMsgType.Action:
			if(actions.Count > 0){
				Debug.Log("[Debug] New message(s)");
				retVal = actions.Dequeue(); // string message
			} else ret = false;
			break;

		default:
			throw new WifiConnectionException("Invalid message type");
		}

		SetMessageType(MyMsgType.Null);
		return ret;
	}

	public override bool SetMessageType(MyMsgType type){
		
		// If message is not in enum, throw exception
		if(!MyMsgType.IsDefined(typeof(MyMsgType), type)) 
			throw new InvalidMessageTypeException();

		this.type = (short) type;
		return true;
	}

	/*****************************/
	/* Network Manager Functions */
	/*****************************/

	public void HandleActionMessage(NetworkMessage msg){
		
		string str = msg.ReadMessage<StringMessage>().value;

		if(str.Equals("START"))
			GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>().LoadWifiBattle();
		else {
			actions.Enqueue(str);
			Debug.Log("[Debug] Action message handler received: \"" + actions.Peek() + "\"");
			Debug.Log("[Debug] str: \"" + str + "\"");
		}
	}

	public void HandleConfigMessage(NetworkMessage msg){
		
		int config = msg.ReadMessage<IntegerMessage>().value;

		configs.Enqueue(config);
		Debug.Log("[Debug] Config message handler received: \"" + configs.Peek() + "\"");
		Debug.Log("[Debug] config: \"" + config + "\"");
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

	public void OnHostConnected(NetworkMessage netMsg){ Debug.Log("[Debug]: Host connected!"); }
	public override void OnStartHost() {
		Debug.Log("[Debug]: Host started!");	
		NetworkServer.RegisterHandler((short) MyMsgType.Action, HandleActionMessage);
	}

	public void OnConnected(NetworkMessage netMsg){
		Debug.Log("Connected!");
		SetMessageType(MyMsgType.Action);
		OtterSendMessage("START");
		GameObject.FindGameObjectWithTag("MenuController").
			GetComponent<MenuController>().LoadWifiBattle();
	}
	
	public void OnDisconnected(NetworkMessage netMsg){ 
		Debug.Log("[Debug]: Disconnected from server");
		GameObject.FindGameObjectWithTag("GameManager").
			GetComponent<GameManager>().EndBattle();
		
		CloseConnection();
	}

	void OnPlayerConnected(NetworkPlayer player) {
		
		Debug.Log("Client connected!");

		IntegerMessage msg = new IntegerMessage();
		msg.value = gm.CompileSettings();

		NetworkServer.SendToClient(
						remoteClient.connection.connectionId, 
						(short) MyMsgType.Config, 
						msg);
    }

	public void OnError(NetworkMessage netMsg){ Debug.Log("[ERROR]: Error connecting - Vish deu ruim :c"); }
}
