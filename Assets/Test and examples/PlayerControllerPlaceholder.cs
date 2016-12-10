using UnityEngine;
using UnityEngine.Networking;

public class PlayerControllerPlaceholder : NetworkBehaviour {
    
    void Update(){

    	ProcessInput();
    }

    void ProcessInput(){

        // Check if the object is local player and not some player over the net
    	if(!isLocalPlayer) return;
    	
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        transform.Translate(x, y, 0);
    }

    public override void OnStartLocalPlayer(){
        // Change the color of local player to differentiate from players over the net
		GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}