using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SwipeDirection {
    UP,
    DOWN,
    LEFT,
    RIGHT,
    ERROR
}

public class SwipeController : MonoBehaviour {

    public float distanceTreshold = 10.0f;
    public float timeTreshold = 1.0f;

    private MenuController mc;
    private Vector2 deltaPosition = Vector2.zero;
    private float deltaTime = 0.0f;

    private void Start() {
        mc = gameObject.GetComponent<MenuController>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            // If a new touch is detected, resets the variables
            if (touch.phase == TouchPhase.Began) {
                deltaPosition = Vector2.zero;
                deltaTime = 0.0f;
            // If the touch ended, checks the movement done and applies it if necessary
            } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                deltaPosition += touch.deltaPosition;
                if (deltaTime <= timeTreshold && (Mathf.Abs(deltaPosition.x) >= distanceTreshold || Mathf.Abs(deltaPosition.y) >= distanceTreshold))
                    SwipeDetected(GetSwipeDirection(deltaPosition));
            // Updates movement as needed
            } else if (touch.phase == TouchPhase.Moved) {
                deltaPosition += touch.deltaPosition;
                deltaTime += touch.deltaTime;
            }
        // Resets variables when no touches occur
        } else {
            deltaPosition = Vector2.zero;
            deltaTime = 0.0f;
        }
	}

    // Gets swipe direction based on screen movement
    private SwipeDirection GetSwipeDirection(Vector2 deltaPosition) {
        if (deltaPosition.x > distanceTreshold && Mathf.Abs(deltaPosition.x) >= Mathf.Abs(deltaPosition.y)) {
            return SwipeDirection.LEFT;
        } else if (deltaPosition.y > distanceTreshold && Mathf.Abs(deltaPosition.y) >= Mathf.Abs(deltaPosition.x)) {
            return SwipeDirection.DOWN;
        } else if ((-1 * deltaPosition.y) > distanceTreshold && Mathf.Abs(deltaPosition.y) >= Mathf.Abs(deltaPosition.x)) {
            return SwipeDirection.UP;
        } else if ((-1 * deltaPosition.x) > distanceTreshold && Mathf.Abs(deltaPosition.x) >= Mathf.Abs(deltaPosition.y)) {
            return SwipeDirection.RIGHT;
        }
        return SwipeDirection.ERROR;
    }

    // Calls the desired functions based on swipe detected and current menu status
    private void SwipeDetected(SwipeDirection direction) {
        switch (direction) {
            case SwipeDirection.UP:
                if (mc.Position == MenuPosition.MAIN)
                    mc.Crebitz();
                else if(mc.Position == MenuPosition.INSTRUCTIONS)
                    mc.Back();
                break;
            case SwipeDirection.DOWN:
                if (mc.Position == MenuPosition.CREDITS)
                    mc.Back();
                else if (mc.Position == MenuPosition.MAIN)
                    mc.Instructions();
                break;
            case SwipeDirection.RIGHT:
                if (mc.Position == MenuPosition.OPTIONS)
                    mc.Back();
                else if (mc.Position == MenuPosition.MAIN)
                    mc.Play();
                break;
            case SwipeDirection.LEFT:
                if (mc.Position == MenuPosition.MAIN)
                    mc.Options();
                else if (mc.Position == MenuPosition.PLAY)
                    mc.Back();
                break;
        }
    }
}
