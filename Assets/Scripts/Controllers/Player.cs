using UnityEngine;
using System.Collections;

// Responsavel por ter as informacoes do player e enviar as acoes realizadas para o GameManager
public class Player : MonoBehaviour {

	private int defCount, defMax;
	private int ammo, maxBullets;
    private bool configured;
    private Animator anim;
	public Action action { get; set; }
    public bool alive { get; private set; }
    private bool finishedAnimation;

    public bool FinishedAnimation {
        get {
            // Se certifica que o valor só será lido como true uma vez
            bool v = finishedAnimation;
            finishedAnimation = false;
            return v;
        }
        private set{
            finishedAnimation = value;
        }
    }

    public void Configure(int maxDef, int maxShots) {
        // Só pode ser configurado uma vez, armazena as regras
        if (!configured) {
            configured = true;
            defMax = maxDef;
            maxBullets = maxShots;
            finishedAnimation = false;
        }
    }

	// Use this for initialization
	void Awake () {
        // Inicializa os valores
        alive = true;
        configured = false;
		ammo = 0;
		defCount = 0;
		defMax = 0;
		maxBullets = 0;
        anim = gameObject.GetComponent<Animator>();
        finishedAnimation = false;
	}

	// Realiza a ação desejada
	public Animation DoAction() {
		switch(action) {
			case Action.DEF: // player quer defender
				defCount++; //incrementa a contagem de defesas
				if(defCount > defMax) { //nao pode defender mais
					defCount = defMax;
                    PlayAnimation(Animation.NOTHING); //To do: talvez criar uma animação específica
                    return Animation.NOTHING;
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
        // Se o inimigo atacar e o player não defender, ele morre
        if (enemyAction == Animation.CANATK && playerAction != Animation.CANDEF) {
            alive = false;
            PlayAnimation(Animation.DEATH);
        // Se o player defender, avalia se a defesa foi bem sucedida
        } else if (playerAction == Animation.CANDEF) {
            PlayAnimation((enemyAction == Animation.CANATK) ? Animation.DEFHIT : Animation.DEFMISS);
        // Se o player atacar, avalia se o ataque acertou
        } else if (playerAction == Animation.CANATK) {
            PlayAnimation((enemyAction == Animation.CANDEF) ? Animation.ATKMISS : Animation.ATKHIT);
        // Se o player consegue recarregar, mostra a animação de recarregar
        } else if (playerAction == Animation.CANREL) {
            PlayAnimation(Animation.RELOK);
        // Se o player não conseguiu recarregar, mostra a animação correspondente
        } else if (playerAction == Animation.NOREL) {
            PlayAnimation(Animation.RELFAIL);
        // Se o player tenta atacar sem munição, mostra a animação correspondente
        }else if (playerAction == Animation.NOAMMO) {
            PlayAnimation(Animation.ATKMISS);
        // Se o player não fez nada e não foi atacado, aparece um tumbleweed passando na frente dele
        } else {
            PlayAnimation(Animation.TUMBLEWEED);
        }

        // Reseta a ação do player para o próximo turno
        playerAction = Animation.NOTHING;
    }

    private void PlayAnimation(Animation selectedAnim) {
        switch (selectedAnim) {
            // Seta o trigger da animação correspondente
            case Animation.NOTHING:
                anim.SetTrigger("batata");
                break;
            case Animation.TUMBLEWEED:
                anim.SetTrigger("tumbleweed");
                break;
            case Animation.ATKHIT:
            case Animation.RELOK:
            case Animation.DEFHIT:
                anim.SetTrigger("success");
                break;
            case Animation.ATKMISS:
            case Animation.RELFAIL:
            case Animation.DEFMISS:
                anim.SetTrigger("failure");
                break;
            case Animation.DEATH:
                anim.SetTrigger("dead");
                break;
            case Animation.NOREL:
            case Animation.CANREL:
                anim.SetTrigger("Reload");
                break;
            case Animation.NODEF:
            case Animation.CANDEF:
                anim.SetTrigger("Guard");
                break;
            case Animation.NOAMMO:
            case Animation.CANATK:
                anim.SetTrigger("Attack");
                break;
            default:
                Debug.Log("That animation doesn't exist");
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (anim.GetBool("enteredIdle")) {
            anim.SetBool("enteredIdle", false);
            finishedAnimation = true;
        }
	}
}
