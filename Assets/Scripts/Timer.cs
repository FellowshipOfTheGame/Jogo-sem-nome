using UnityEngine;
using System.Collections;

public delegate void VoidFunction();

public class Timer : MonoBehaviour {
    
    private bool counting;
    public float time { get; private set; }
    private float maxTime;
    private VoidFunction timerFunction;
    private bool show;

	// Use this for initialization
	void Awake () {
        time = 0;
        counting = false;
        timerFunction = null;
        show = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (counting)
            UpdateTimer();
	}

    // Essa função deve ser chamada de fora para iniciar o timer
    public void StartTimer(float countdownTime, VoidFunction function, bool showOnScreen){
        // O tempo máximo e o tempo atual são atualizados
        time = countdownTime;
        maxTime = countdownTime;
        // A função a ser executada no final da contagem é armazenada
        timerFunction = function;
        // Indica o início da contagem
        counting = true;
        show = showOnScreen;
    }

    void UpdateTimer(){
        // Decrementa o tempo atual
        float newTime = time - Time.deltaTime;
        // Quando a contagem acabar
        if (newTime <= 0) {
            // Indica o fim, zera time e chama a função salva
            counting = false;
            if(timerFunction != null)
                timerFunction();
            time = 0;
            timerFunction = null;
        } else
            time = newTime;
    }

    // Mostra o valor da contagem atual
	void OnGUI() {
        if(show)
		    GUI.Box(new Rect(15, 15, 55, 20), "Test: " + time.ToString("0"));
	}
}
