using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;

/*
Tirar selectedAction do timer e fazer o timer simplesmente mudar o estado do gameManager para "Waiting for message (type Action)"
		Timer acabou -> Esperando Mensagem -> In battle parte2

Adicionar handler de mensagem pra configurações (cliente vai aceitar a porra das configurações do server)



Baixa prioridade - fazer mecanismos de sincronização do código, mandar uma mensagem com estado atual do jogo, numero de balas e defesas do jogador
*/

public class Wifi : Connection {
	
	private enum MyMsgType : short { Debug = 100, Action, Config, Null }

	public bool isHost;

	private short type = Null;
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
			remoteClient.RegisterHandler(MsgType.Connect, OnConnected);
			remoteClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
			remoteClient.RegisterHandler(MsgType.Error, OnError);
			networkAddress = "172.28.143.20"; // Debug
			remoteClient.Connect(networkAddress, networkPort);
		}

		return true;
	}

	public override bool OtterSendMessage(string msg){
		
		if(this.type == MyMsgType.Null)
			throw new WifiConnectionException("No message type defined");

		Debug.Log("[Debug] Sending message: " + msg);

		StringMessage sendMsg = new StringMessage();
		sendMsg.value = msg;

		return client.Send(this.type, sendMsg);
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

	public void HandleActionMessage(NetworkMessage msg){
		
		string str = msg.ReadMessage<StringMessage>().value;

		if(str.Equals("START"))
			GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>().LoadWifiBattle();
		else {
			messages.Enqueue(str);
			Debug.Log("[Debug] Message handler received: \"" + messages.Peek() + "\"");
			Debug.Log("[Debug] str: \"" + str + "\"");
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
		NetworkServer.RegisterHandler((short) MyMsgType.Action, HandleActionMessage);
	}

	public void OnConnected(NetworkMessage netMsg){
		Debug.Log("Connected!");
		// OtterSendMessage("START", (short) MyMsgType.Action);
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
