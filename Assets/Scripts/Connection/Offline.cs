using UnityEngine;
using System.Collections;

// Media a interacao com a IA do jogo
public class Offline : Connection { 

    private GameManager gm;
    private short type = (short) MyMsgType.Null;
    private int nBullets;
    private int consecutiveDefs;
    private bool justDidNothing;

    // Makes sure the AI doesnt try to break the rules and does not stay idle twice in a row
    private bool IsValidChoice(int choice) {
        switch (choice) {
        case 0:
        return !justDidNothing;
        case 1:
        return nBullets > 0;
        case 2:
        return consecutiveDefs < gm.MaxDefenses;
        case 3:
        return nBullets < gm.MaxBullets;
        default:
        return true;
        }
    }

    void Start(){
        nBullets = 0;
        consecutiveDefs = 0;
        justDidNothing = false;
        this.gm = GameObject.FindGameObjectWithTag("GameManager").
            GetComponent<GameManager>();
    }

    public override bool Connect(){ return true; }
    public override bool CloseConnection(){
        Destroy(this);
        return true;
    }
    
    public override bool OtterSendMessage(object msg){ 
        type = (short) MyMsgType.Null;
        return true; 
    }

    public override bool GetMessage(ref object retVal){
        
        if(this.type == (short) MyMsgType.Action){

            type = (short) MyMsgType.Null;

            int choice;
            // Gets a random action until it is a valid choice
            do {
                choice = Random.Range(0, 4);
            } while (!IsValidChoice(choice));

            switch(choice) {
            case 0:
                retVal = "NOOP";
                justDidNothing = true;
                consecutiveDefs = 0;
                return true;
            case 1:
                justDidNothing = false;
                retVal = "ATK";
                consecutiveDefs = 0;
                nBullets--;
                return true;
            case 2:
                justDidNothing = false;
                retVal = "DEF";
                consecutiveDefs++;
                return true;
            case 3:
                justDidNothing = false;
                retVal = "REL";
                consecutiveDefs = 0;
                nBullets++;
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
