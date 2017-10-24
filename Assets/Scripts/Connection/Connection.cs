using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class Connection : NetworkManager {

	public abstract bool Connect();
	public abstract bool OtterSendMessage(object msg);
    public abstract bool GetMessage(ref object retVal);
	public abstract bool CloseConnection();
	public abstract bool SetMessageType(MyMsgType type);

	public enum MyMsgType : short { Debug = 100, Action, Config, Null }
}
