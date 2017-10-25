using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopChains : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision) {
        GetComponent<AudioSource>().Stop();
    }
}
