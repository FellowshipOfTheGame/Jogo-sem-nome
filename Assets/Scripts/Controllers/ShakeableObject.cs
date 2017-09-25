using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeableObject : MonoBehaviour {

    public float shakeRange = 0.0f;
    public float shakeTime = 0.0f;
    private float currentTime;
    private bool shaking = false;
    private Vector3 startPosition;

    public void Start() {
        shaking = false;
    }

    public void Shake() {
        startPosition = transform.position;
        shaking = true;
        currentTime = shakeTime;
    }
    
	void Update () {
        if (shaking) {
            if (currentTime > 0.0f) {
                Vector2 randomPosition = Random.insideUnitCircle * shakeRange;
                transform.position = new Vector3(startPosition.x + randomPosition.x, startPosition.y + randomPosition.y, startPosition.z);
                currentTime -= Time.deltaTime;
            } else {
                currentTime = 0.0f;
                shaking = false;
                transform.position = startPosition;
            }
        }
	}
}
