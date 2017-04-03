using UnityEngine;
using System.Collections;

public enum Action : byte {NOOP, ATK, DEF, REL}

// Responsavel por ter as informacoes do player e enviar as acoes realizadas para o GameManager
public class Player : MonoBehaviour {

	private int defCount, defMax; 
	private int ammo, maxBullets;
    private bool configured;
    private Animator anim;
	public Action action { get; set; }
    public bool alive { get; private set; }

    public void Configure(int maxDef, int maxShots) {
        if (!configured) {
            configured = true;
            defMax = maxDef;
            maxBullets = maxShots;
        }
    }

	// Use this for initialization
	void Start () {
        alive = true;
        configured = false;
		ammo = 0;
		defCount = 0;
		defMax = 0;
		maxBullets = 0;
	}

	/* Realiza a ação, e será chamada quando o timer acabar */
	public Animation DoAction() {
		switch(action) {
			case Action.DEF: // player quer defender
				defCount++; //incrementa a contagem de defesas
				if(defCount > defMax) { //nao pode defender mais
					defCount = defMax;
                    PlayAnimation(Animation.NODEF);
					return Animation.NODEF;
				}

                PlayAnimation(Animation.CANDEF);
				return Animation.CANDEF;
			case Action.ATK: // player quer atacar
				defCount = 0; //reinicia a contagem de defesas
                if (ammo == 0) {
                    PlayAnimation(Animation.NOAMMO);
                    return Animation.NOAMMO; //nao pode atacar
                }
				ammo--;

                PlayAnimation(Animation.CANATK);
				return Animation.CANATK;		
			case Action.REL: // player quer recarregar
				defCount = 0; //reinicia a contagem de defesas
				ammo++;
				if(ammo > maxBullets) { //nao pode carregar mais
					ammo = maxBullets;
                    PlayAnimation(Animation.NOREL);
					return Animation.NOREL;
				}

                PlayAnimation(Animation.CANREL);
				return Animation.CANREL;
			default: // player nao quer fazer nada
				defCount = 0; //reinicia a contagem de defesas
                PlayAnimation(Animation.NOTHING);
				return Animation.NOTHING;
		}
	}

    public void DoReaction(Animation playerAction, Animation enemyAction) {
        if (enemyAction == Animation.CANATK && playerAction != Animation.CANDEF) {
            alive = false;
            PlayAnimation(Animation.DEATH);
        }else if (playerAction == Animation.CANDEF) {
            PlayAnimation((enemyAction == Animation.CANATK)? Animation.DEFHIT : Animation.DEFMISS);
        }else if(playerAction == Animation.CANATK) {
            PlayAnimation((enemyAction == Animation.CANDEF) ? Animation.ATKMISS : Animation.ATKHIT);
        } else {
            PlayAnimation(Animation.NOTHING);
        }
    }

    private void PlayAnimation(Animation selectedAnim) {
        // To do: Esperar até que a animação atual seja nothing
        switch (selectedAnim) {
            // To do
            case Animation.NOTHING:
                break;
            case Animation.NOREL:
                break;
            case Animation.NODEF:
                break;
            case Animation.NOAMMO:
                break;
            case Animation.DEFMISS:
                break;
            case Animation.DEFHIT:
                break;
            case Animation.DEATH:
                break;
            case Animation.CANREL:
                break;
            case Animation.CANDEF:
                break;
            case Animation.CANATK:
                break;
            case Animation.ATKMISS:
                break;
            case Animation.ATKHIT:
                break;
            default:
                Debug.Log("That animation doesn't exist");
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		//nada por enquanto
	}


}
