using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UniformFall : MonoBehaviour {

    private Rigidbody2D rb;
    public float fallSpeed = 10;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {
        rb.velocity = new Vector2(rb.velocity.x, (-1) * fallSpeed);
	}
}
