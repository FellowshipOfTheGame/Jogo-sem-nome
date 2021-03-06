﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderValue : MonoBehaviour {

    private Text txt;
    private Slider slider;

	// Use this for initialization
	void Start () {
        slider = gameObject.GetComponentInParent<Slider>();
        txt = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        // Make sure the text has the current value on the slider
        txt.text = "" + (int)slider.value;
	}
}
