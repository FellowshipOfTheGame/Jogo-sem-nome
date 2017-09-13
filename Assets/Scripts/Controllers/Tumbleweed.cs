using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbleweed : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        if (transform.position.x < -4.0f)
            Destroy(gameObject);
	}
}
