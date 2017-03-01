using UnityEngine;
using System.Collections;

public enum Animation: byte { NOAMMO, CANATK, NODEF, CANDEF, NOREL, CANREL, NOTHING }
public enum Result : byte { VICTORY, DEFEAT, DRAW }

// Recebe as mensagens do player e do inimigo conectado, avalia o Animation nesse turno e envia o Animation calculado para os dois
public class GameManager : MonoBehaviour {


    // metodo para receber/enviar mensagem do/ao player, metodo para receber/enviar mensagem do/ao inimigo
    // criar scripts auxiliares(Ex.: AnimationController) para nao sobrecarregar o GameManager

    private Player localPlayer, enemyPlayer;
	private Timer timer;
	private Connection connection;
    private bool battleStarted;

    public bool battleEnded {
        get { return battleEnded; }
        private set { battleStarted = value; }
    }

    public int maxDefenses {
        get { return maxDefenses; }
        set {
            if(!battleStarted) {
                maxDefenses = value;
            }
            else Debug.Log("Can not change that parameter during game");
        }
    }

    public int maxBullets {
        get { return maxBullets; }
        set {
            if(!battleStarted) {
                maxBullets = value;
            }
            else Debug.Log("Can not change that parameter during game");
        }
    }

    public float countdownTime {
        get { return countdownTime; }
        set {
            if (!battleStarted) {
                countdownTime = value;
            } else Debug.Log("Can not change that parameter during game");
        }
    }

    // Use this for initialization
    void Start () {
        timer = new Timer();
        maxBullets = 3;
        maxDefenses = 3;
        countdownTime = 3;

        connection = null;
        battleStarted = false;
        battleEnded = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (battleStarted) {
            if(timer.time <= 0) {// Sempre que o timer não estiver ativo, uma ação deve ser realizada
                timer.StartTimer(countdownTime, SelectAction); // inicia o timer
            }   
        }
	}

    // Após a conexão ser estabelecida e for verificado que ela está funcionando, inicia-se a Result
    public void StartBattle(Connection successfulConection) {
        connection = successfulConection;
        localPlayer = new Player(maxDefenses, maxBullets);
        enemyPlayer = new Player(maxDefenses, maxBullets);

        // Inserir aqui qualquer animação de início de Result
        battleStarted = true;
    }

    // Função a ser chamada quando acabar a Result
    private void EndBattle(Result result) {
        if (result != Result.DEFEAT){
            // Roda a animação de morte no inimigo
        } else {
            // Roda a animação de vitória no inimigo
        }
        if (result != Result.VICTORY) {
            // Roda a animação de morte no player
        } else {
            // Roda a animação de vitoria no player
        }

        maxBullets = 3;
        maxDefenses = 3;
        countdownTime = 3;

        connection = null;
        battleStarted = false;
        battleEnded = true;
    }

    public void SelectAction() {
        // Envia a ação selecionada (esperar connection ser feito para implementar)
        // Recebe a ação do inimigo (esperar connection ser feito para implementar)
        Action enemyAction = Action.NOOP; // placeholder
        Animation localAnimation = localPlayer.DoAction();
        enemyPlayer.action = enemyAction;
        Animation enemyAnimation = enemyPlayer.DoAction();
        // Compara os Animations das duas ações e, de acordo com o que aconteceu chama as animações
        // Se necessário, chama a função de fim da Result
    }
}