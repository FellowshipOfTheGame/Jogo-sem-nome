using UnityEngine;
using System.Collections;

public delegate void VoidFunction();

public class Timer : MonoBehaviour {
    
    private bool counting;
    private float maxTime;
    private int currentPosition;
    private VoidFunction timerFunction;
    private bool show;
    private GameObject[] fuse;
    private GameObject barrel, tip;
    public float fuseSize;
    public int nFuses;
    public float time { get; private set; }

    // Use this for initialization
    void Awake () {
        time = 0;
        counting = false;
        timerFunction = null;
        show = false;
        fuse = new GameObject[nFuses];
        currentPosition = nFuses;
        foreach(Transform child in transform){
            if (child.gameObject.tag == "Fuse") {
                string positionString = child.gameObject.name.Split(' ')[1];
                int position = int.Parse(positionString);
                if(position < nFuses)
                    fuse[position] = child.gameObject;
            } else if (child.gameObject.tag == "Tip")
                tip = child.gameObject;
            else if (child.gameObject.tag == "Barrel")
                barrel = child.gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Updates timer if needed
        if (counting)
            UpdateTimer();
        // Calculates new tip position
        int newPosition = Mathf.Min(Mathf.FloorToInt((time / maxTime) * nFuses), nFuses);
        // If the new position is to the left of the current one, moves tip deactivating the fuse components
        if (newPosition < currentPosition) {
            for (int i = currentPosition; i > newPosition; i--) {
                fuse[currentPosition - 1].SetActive(false);
                tip.transform.position = new Vector3(tip.transform.position.x - fuseSize, tip.transform.position.y, tip.transform.position.z);
            }
            currentPosition = newPosition;
        // If the new position is to the right of the current one, moves tip activating the components instead
        } else if (newPosition > currentPosition) {
            for (int i = currentPosition; i < newPosition; i++) {
                fuse[i].SetActive(true);
                tip.transform.position = new Vector3(tip.transform.position.x + fuseSize, tip.transform.position.y, tip.transform.position.z);
            }
            currentPosition = newPosition;
        }
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
}
