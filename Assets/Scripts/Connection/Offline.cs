using UnityEngine;
using System.Collections;

// Media a interacao com a IA do jogo
public class Offline : Connection { 

    private GameManager gm;
    private short type = (short) MyMsgType.Null;
    private int nBullets;
    private int consecutiveDefs;
    
    private bool IsValidChoice(int choice) {
        if (choice == 1 && nBullets <= 0)
            return false;
        if (choice == 2 && consecutiveDefs >= (gm.MaxDefenses - 1))
            return false;
        if (choice == 3 && nBullets >= (gm.MaxBullets - 1))
            return false;
        return true;
    }

    void Start(){
        nBullets = 0;
        consecutiveDefs = 0;
        this.gm = GameObject.FindGameObjectWithTag("GameManager").
            GetComponent<GameManager>();
    }

    public override bool Connect(){ return true; }
    public override bool CloseConnection(){ return true; }
    
    public override bool OtterSendMessage(object msg){ 
        type = (short) MyMsgType.Null;
        return true; 
    }

    public override bool GetMessage(ref object retVal){
        
        if(this.type == (short) MyMsgType.Action){

            type = (short) MyMsgType.Null;

            int choice;
            do {
                choice = Random.Range(0, 4);
            } while (!IsValidChoice(choice));

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
            Debug.Log("[Debug](Offline): Getting config message.");
            retVal = gm.CompileSettings();
            Debug.Log("[Debug](Offline): Returning: " + retVal);
            type = (short) MyMsgType.Null;
            return true;
        }

        return false;
    }

    public override bool SetMessageType(MyMsgType type){
        this.type = (short) type;
        return true;
    }
}
