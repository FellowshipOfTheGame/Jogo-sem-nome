using UnityEngine;
using System.Collections;

public delegate void VoidFunction();

public class Timer : MonoBehaviour {

    private bool counting;
    public float time { get; private set; }
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
            timerFunction();
            time = 0;
        }
    }

    // Mostra o valor da contagem atual
	void OnGUI() {
		GUI.Box(new Rect(15, 15, 55, 20), "Test: " + time.ToString("0"));
	}
}
