using UnityEngine;
using System.Collections;

public class Wifi : Connection {

	

	// algo assim
	public override bool StabilishConnection(){
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
}
