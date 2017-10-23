using UnityEngine;
using System.Collections;

// Media a interacao com a IA do jogo
public class Offline : Connection { 

    private short type = (short) MyMsgType.Null;
    
    public override bool Connect(){ return true; }
    public override bool CloseConnection(){ return true; }
    
    public override bool OtterSendMessage(string msg){ 
        type = (short) MyMsgType.Null;
        return true; 
    }

    public override bool GetMessage(ref object retVal){
        
        if(this.type == (short) MyMsgType.Action){

            type = (short) MyMsgType.Null;
            
            int choice = Random.Range(0, 4);
            switch(choice) {
            case 0:
                retVal = "NOOP";
                return true;
            case 1:
                retVal = "ATK";
                return true;
            case 2:
                retVal = "DEF";
                return true;
            case 3:
                retVal = "REL";
                return true;
            default:
                Debug.Log("This REALLY shouldn't happen");
                retVal = "NOOP";
                return false;
            }
        } else if(this.type == (short) MyMsgType.Config){
            retVal = 333;
            return true;
        }

        return false;
    }

    public override bool SetMessageType(MyMsgType type){
        return false;
    }
}
