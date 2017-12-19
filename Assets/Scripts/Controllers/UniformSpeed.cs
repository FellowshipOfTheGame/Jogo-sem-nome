using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UniformSpeed : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed = 10;
    public Vector2 direction = new Vector3(0.0f, -1.0f);

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {
        // Normalize the direction defined through the unity editor
        direction.Normalize();
        // Applies the desired speed to the object
        if(direction.x == 0.0f)
            rb.velocity = new Vector2(rb.velocity.x, direction.y * speed);
        else if (direction.y == 0.0f)
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
	}
}
