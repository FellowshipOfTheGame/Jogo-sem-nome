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

    // Saves initial position and starts the countdown
    public void Shake() {
        startPosition = transform.position;
        shaking = true;
        currentTime = shakeTime;
    }
    
	void Update () {
        if (shaking) {
            // While the countdown is not over
            if (currentTime > 0.0f) {
                // Moves to a random position inside the defined radius
                Vector2 randomPosition = Random.insideUnitCircle * shakeRange;
                transform.position = new Vector3(startPosition.x + randomPosition.x, startPosition.y + randomPosition.y, startPosition.z);
                // Counts down
                currentTime -= Time.deltaTime;
            } else {
                // Sets the object back to the starting position and timer to zero
                currentTime = 0.0f;
                shaking = false;
                transform.position = startPosition;
            }
        }
	}
}
