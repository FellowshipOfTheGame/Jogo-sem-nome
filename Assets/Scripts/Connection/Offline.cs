using UnityEngine;
using System.Collections;

// Media a interacao com a IA do jogo
public class Offline : Connection { 

    // algo assim
    public override bool Connect(){ return true; }
    public override bool OtterSendMessage(string msg){ return true; }
	public override bool CloseConnection(){ return true; }

	public override string GetMessage(){
        
        int choice = Random.Range(0, 4);
        switch (choice) {
            case 0:
                return "NOOP";
            case 1:
                return "ATK";
            case 2:
                return "DEF";
            case 3:
                return "REL";
            default:
                Debug.Log("This REALLY shouldn't happen");
                return "NOOP";
        }
    }
}
