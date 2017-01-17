using UnityEngine;
using System.Collections;

public enum Action : byte {NOOP, ATK, DEF, REL}

// Responsavel por ter as informacoes do player e enviar as acoes realizadas para o GameManager
public class Player : MonoBehaviour {

	private int defCount, defMax; 
	private int ammo, maxBullets; 
	private Action action;

	// Use this for initialization
	void Start () {
		ammo = 0;
		defCount = 0;
		defMax = 5; 	//placeholder
		maxBullets = 5; //placeholder
	}

	/* Setter da ação */
	public void SetAction(Action act) {
		action = act;
	}

	/* Realiza a ação, e será chamada quando o timer acabar */
	public Action DoAction() {
		switch(action) {
			case DEF: // player quer defender
				defCount++; //incrementa a contagem de defesas
				if(defCount > defMax) { //nao pode defender mais
					defCount = defMax; 
					return NODEF;
				}
				
				return CANDEF;
			case ATK: // player quer atacar
				defCount = 0; //reinicia a contagem de defesas
				if(ammo == 0) return NOAMMO; //nao pode atacar				
				ammo--; 

				return CANATK;		
			case REL: // player quer recarregar
				defCount = 0; //reinicia a contagem de defesas
				ammo++;
				if(ammo > maxBullets) { //nao pode carregar mais
					ammo = maxBullets; 
					return NOREL;
				}

				return CANREL;
			default: // player nao quer fazer nada
				defCount = 0; //reinicia a contagem de defesas
				return NOTHING;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//nada por enquanto
	}


}
