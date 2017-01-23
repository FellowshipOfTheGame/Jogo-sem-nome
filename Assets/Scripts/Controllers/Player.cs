using UnityEngine;
using System.Collections;

public enum Action : byte {NOOP, ATK, DEF, REL}

// Responsavel por ter as informacoes do player e enviar as acoes realizadas para o GameManager
public class Player : MonoBehaviour {

	private int defCount, defMax; 
	private int ammo, maxBullets; 
	public Action action { get; set; }

    public Player(int maxDef, int maxShot) {
        defMax = maxDef;
        maxBullets = maxShot;
    }

	// Use this for initialization
	void Start () {
		ammo = 0;
		defCount = 0;
		//defMax = 5; 	//placeholder
		//maxBullets = 5; //placeholder
	}

	/* Realiza a ação, e será chamada quando o timer acabar */
	public Resultado DoAction() {
		switch(action) {
			case Action.DEF: // player quer defender
				defCount++; //incrementa a contagem de defesas
				if(defCount > defMax) { //nao pode defender mais
					defCount = defMax;
					return Resultado.NODEF;
				}
				
				return Resultado.CANDEF;
			case Action.ATK: // player quer atacar
				defCount = 0; //reinicia a contagem de defesas
				if(ammo == 0) return Resultado.NOAMMO; //nao pode atacar				
				ammo--; 

				return Resultado.CANATK;		
			case Action.REL: // player quer recarregar
				defCount = 0; //reinicia a contagem de defesas
				ammo++;
				if(ammo > maxBullets) { //nao pode carregar mais
					ammo = maxBullets; 
					return Resultado.NOREL;
				}

				return Resultado.CANREL;
			default: // player nao quer fazer nada
				defCount = 0; //reinicia a contagem de defesas
				return Resultado.NOTHING;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//nada por enquanto
	}


}
