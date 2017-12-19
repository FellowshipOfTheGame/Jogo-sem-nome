using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbleweed : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        // If it goes a bit off screen to the left destroy it
        if (transform.position.x < -4.0f)
            Destroy(gameObject);
	}
}
