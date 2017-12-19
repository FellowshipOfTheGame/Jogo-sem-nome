using UnityEngine;
using System.Collections;

public class Bluetooth : Connection {

    // Not implemented
	public override bool Connect(){
        return false;
	}
	
	public override bool OtterSendMessage(object msg){
        return false;
	}
	
	public override bool GetMessage(ref object retVal){
        return false;
	}
	
	public override bool CloseConnection(){
        return false;
	}

	public override bool SetMessageType(MyMsgType type){
		return false;
	}
}
