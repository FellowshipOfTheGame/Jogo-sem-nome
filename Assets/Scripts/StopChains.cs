using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopChains : MonoBehaviour {

    // Stop playing the sound of the object (chains) when it collides with this
    private void OnCollisionEnter2D(Collision2D collision) {
        GetComponent<AudioSource>().Stop();
    }
}
