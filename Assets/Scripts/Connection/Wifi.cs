using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Wifi : Connection {

	private int[] ports = {11100, 21025, 2626, 6262, 77777, 4444};
	private string hostIp;
	private NetworkClient myClient;

	public Wifi(){
		myClient = new NetworkClient();
		hostIp = "127.0.0.1";
	}

	// algo assim
	protected override bool Connect(){
		Debug.Log("Connecting at port 11100");
		myClient.Connect(hostIp, 11100);
        return true;
    }
	
	public override bool SendMessage(){
        return true;
    }
	
	public override bool GetMessage(){
        return true;
    }
	
	public override bool CloseConnection(){
        return true;
    }

    public void WifiConnect(string ip){
    	Debug.Log("ip: " + ip);
    	hostIp = ip;
    	Connect();
    }

    public string GetLocalIp(){
		return Network.player.ipAddress;
    }
    
    public void Host(){
    	NetworkServer.Listen(11100);
		myClient.Connect("127.0.0.1", 11100);
    }

    public void OnConnected(NetworkConnection conn, NetworkReader reader) {
		Debug.Log("Connected!");
	}

	public void OnConnectedToServer() {
		Debug.Log("Connected to server!");
	}

	public void OnDisconnected(NetworkConnection conn, NetworkReader reader) {
		Debug.Log("Disconnected from server");
	}

	public void OnError(NetworkConnection conn, NetworkReader reader) {
		Debug.Log("Error connecting with message: Vish deu ruim :c");
	}

}
