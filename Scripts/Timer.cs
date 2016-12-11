using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	private float time = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        UpdateTimer();
		
	}

    void UpdateTimer(){
        time -= Time.deltaTime;
        if(time <= 0){
            time = 0;
            print("Timer zerou");
            // resolve round
        }
    }

	void OnGUI() {
		GUI.Box(new Rect(15, 15, 55, 20), "Test: " + time.ToString("0"));
	}
}
