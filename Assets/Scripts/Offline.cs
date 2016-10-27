using UnityEngine;
using System.Collections;

public class Offline : Connection {

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
