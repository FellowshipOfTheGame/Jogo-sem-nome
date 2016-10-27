using UnityEngine;
using System.Collections;

public abstract class Connection : MonoBehaviour {
    
	public abstract bool StabilishConnection();
	public abstract bool SendMessage();
	public abstract bool GetMessage();
	public abstract bool CloseConnection();

}
