﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleButtons : MonoBehaviour {

    private GameManager gm;

	// Use this for initialization
	void Awake () {
        GameObject go = GameObject.FindGameObjectWithTag("GameManager");
        gm = go.GetComponent<GameManager>();
	}

    public void AttackPressed() {
        gm.SetPlayerAction(Action.ATK);
    }

    public void GuardPressed() {
        gm.SetPlayerAction(Action.DEF);
    }

    public void ReloadPressed() {
        gm.SetPlayerAction(Action.REL);
    }
}
