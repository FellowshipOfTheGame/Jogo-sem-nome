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
        time = 0;
        counting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(counting)
		    UpdateTimer();
	}

    // getter da variável time
    public float getTime() {
        return time;
    }

    // Essa função deve ser chamada de fora para iniciar o timer
    public void StartTimer(float maxTime, VoidFunction function){
        // O tempo máximo e o tempo atual são atualizados
        this.maxTime = maxTime;
        time = maxTime;
        // A função a ser executada no final da contagem é armazenada
        timerFunction = function;
        // Indica o início da contagem
        counting = true;
    }

    void UpdateTimer(){
        // Decrementa o tempo atual
        time -= Time.deltaTime;
        // Quando a contagem acabar
        if(time <= 0){
            // Indica o fim, zera time e chama a função salva
            counting = false;
            time = 0;
            timerFunction();
        }
    }

    // Mostra o valor da contagem atual
	void OnGUI() {
		GUI.Box(new Rect(15, 15, 55, 20), "Test: " + time.ToString("0"));
	}
}
