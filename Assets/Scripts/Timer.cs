using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    private float time = 5.0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        time -= Time.deltaTime;
        if(time <= 0){
            time = 0;
            print("Timer zerou");
        }
    }

    void OnGUI() {
        GUI.Box(new Rect(10, 10, 50, 20), "Test: " + time.ToString("0"));
    }
}
