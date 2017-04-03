using UnityEngine;
using System.Collections;

public class Bluetooth : Connection {

	// Os scripts de conexao precisam de uma referencia para o GameManager local
	private GameManager localManager;

	// algo assim
	public override bool Connect(){
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
