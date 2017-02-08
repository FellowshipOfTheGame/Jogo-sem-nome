using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class Connection : NetworkManager {

	// algo assim
	protected abstract bool Connect();
	public abstract bool SendMessage();
	public abstract bool GetMessage();
	public abstract bool CloseConnection();

}
