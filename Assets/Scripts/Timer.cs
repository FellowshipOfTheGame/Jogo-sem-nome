using UnityEngine;
using System.Collections;

public delegate void VoidFunction();

public class Timer : MonoBehaviour {
    
    private bool counting;
    private float maxTime, firstPosition, currentPosition;
    private int currentIndex;
    private VoidFunction timerFunction;
    private GameObject[] fuse;
    private GameObject barrel, tip;
    public float fuseSize, speed;
    public int nFuses;
    public float time { get; private set; }

    // Use this for initialization
    void Awake () {
        time = 0;
        counting = false;
        timerFunction = null;
        fuse = new GameObject[nFuses];
        foreach(Transform child in transform){
            if (child.gameObject.tag == "Fuse") {
                string positionString = child.gameObject.name.Split(' ')[1];
                int position = int.Parse(positionString);
                if (position < nFuses) {
                    fuse[position] = child.gameObject;
                    child.gameObject.SetActive(true);
                }
            } else if (child.gameObject.tag == "Tip")
                tip = child.gameObject;
            else if (child.gameObject.tag == "Barrel")
                barrel = child.gameObject;
        }
        firstPosition = fuse[0].transform.position.x - (fuseSize/2);
        Debug.Log("first position = " + firstPosition);
        currentPosition = firstPosition + (nFuses * fuseSize);
        Debug.Log("current position = " + currentPosition);
        currentIndex = nFuses;
        //tip.transform.position = new Vector3(currentPosition, tip.transform.position.y, tip.transform.position.z);
        tip.GetComponent<RectTransform>().anchoredPosition = new Vector3(currentPosition, tip.transform.position.y, tip.transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        // Updates timer if needed
        if (counting)
            UpdateTimer();
        // Calculates new tip position
        int targetIndex = Mathf.FloorToInt((time / maxTime) * nFuses);
        float targetPosition = firstPosition + (targetIndex * fuseSize);
        currentPosition = tip.GetComponent<RectTransform>().anchoredPosition.x;
        currentIndex = Mathf.FloorToInt((currentPosition - firstPosition) / fuseSize);
        Debug.Log("current index = " + currentIndex);
        Debug.Log("target index = " + targetIndex);
        // If the new position is to the left of the current one, moves tip deactivating the fuse components
        if (targetIndex < currentIndex) {
            for (int i = currentIndex; i > targetIndex; i--) {
                fuse[i - 1].SetActive(false);
            }
            // If the new position is to the right of the current one, moves tip activating the components instead
        } else if (targetIndex > currentIndex) {
            for (int i = currentIndex; i < targetIndex; i++) {
                fuse[i].SetActive(true);
            }
        }
        tip.GetComponent<RectTransform>().anchoredPosition = new Vector3(targetPosition, 0.0f, 0.0f);
    }

    // Essa função deve ser chamada de fora para iniciar o timer
    public void StartTimer(float countdownTime, VoidFunction function){
        // O tempo máximo e o tempo atual são atualizados
        time = countdownTime;
        maxTime = countdownTime;
        // A função a ser executada no final da contagem é armazenada
        timerFunction = function;
        // Indica o início da contagem
        counting = true;
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
