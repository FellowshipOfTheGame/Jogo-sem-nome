using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class Connection : NetworkManager {

	// algo assim
	public abstract bool Connect();
	public abstract bool OtterSendMessage(string msg);
	public abstract string GetMessage();
	public abstract bool CloseConnection();

}
