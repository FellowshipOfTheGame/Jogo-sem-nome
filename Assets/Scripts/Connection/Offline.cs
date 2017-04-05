using UnityEngine;
using System.Collections;

// Media a interacao com a IA do jogo
public class Offline : Connection {	

	// algo assim
	public override bool Connect(){
        return true;
	}

	public override bool SendMessage(string msg){
        return true;
	}

	public override string GetMessage(){
        return "NOOP";
	}

	public override bool CloseConnection(){
        return true;
    }

}
