using UnityEngine;
using System.Collections;

enum Action : byte {NOOP, ATK, DEF, REL}

// Responsavel por ter as informacoes do player e enviar as acoes realizadas para o GameManager
public class Player : MonoBehaviour {

	private int defCount, defMax;
	private int ammo, maxBullets;
	private Action action;

	// Use this for initialization
	void Start () {
		ammo = 0;
		defCount = 0;
	}

	/* Só seleciona a ação */
	public void SetAction(Action act) {
		action = act;
	}

	/* Realiza a ação, e será chamada quando o timer acabar */
	public void DoAction() {
		switch(action) {
			case DEF:
				defCount++;
				if(defCount > defMax) {//nao pode defender mais
					ammo = maxBullets; 
					return NODEF;
				}
				return CANDEF;
			case ATK:
				defCount = 0;
				if(ammo == 0) return NOAMMO;
				ammo--;
				return CANATK;
			case REL:
				defCount = 0;
				ammo++;
				if(ammo > maxBullets) {//nao pode carregar mais
					ammo = maxBullets; 
					return NOREL;
				}
				return CANREL;
			default: 
				defCount = 0;
				return NOTHING;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//nada por enquanto
	}


}
