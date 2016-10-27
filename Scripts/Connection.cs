using UnityEngine;
using System.Collections;

public abstract static class Connection : MonoBehaviour {

	// algo assim
	public abstract bool StabilishConnection();
	public abstract bool SendMessage();
	public abstract bool GetMessage();
	public abstract bool CloseConnection();

}
