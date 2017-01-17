using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    public delegate void VoidFunction();

    private bool counting;
    private float time;
    private float maxTime;
    private VoidFunction timerFunction;

	// Use this for initialization
	void Start () {
        counting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(counting)
		    UpdateTimer();
	}

    public void StartTimer(float maxTime, VoidFunction function){
        this.maxTime = maxTime;
        time = maxTime;
        timerFunction = function;
        counting = true;
    }

    void UpdateTimer(){
        time -= Time.deltaTime;
        if(time <= 0){
            counting = false;
            time = 0;
            timerFunction();
        }
    }

	void OnGUI() {
		GUI.Box(new Rect(15, 15, 55, 20), "Test: " + time.ToString("0"));
	}
}
