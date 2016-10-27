using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Wifi : Connection {

    // Deve ser feito numa cena separada, tipo a cena do menu inicial
    public bool isAtStartup = true;
    NetworkClient client;

    public override bool StabilishConnection(){

        /* Diferenciar NetworkServer e NetworkClient 
           possívelmente dependendo do botão q o cara clicar?
        */

        if (isAtStartup){
            if (Input.GetKeyDown(KeyCode.S)) { }
                // SetupServer();

            if (Input.GetKeyDown(KeyCode.C)) { }
                // SetupClient();

            if (Input.GetKeyDown(KeyCode.B)) { }
                // SetupServer();
                // SetupLocalClient();
        }

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

