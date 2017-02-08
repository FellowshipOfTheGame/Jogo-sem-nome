using UnityEngine;
using System.Collections;

public enum Resultado: byte {NOAMMO, CANATK, NODEF, CANDEF, NOREL, CANREL, NOTHING}

// Recebe as mensagens do player e do inimigo conectado, avalia o resultado nesse turno e envia o resultado calculado para os dois
public class GameManager : MonoBehaviour {

	// metodo para receber/enviar mensagem do/ao player, metodo para receber/enviar mensagem do/ao inimigo
	// criar scripts auxiliares(Ex.: AnimationController) para nao sobrecarregar o GameManager

	private Player localPlayer, enemyPlayer;
	private Timer timer;
	private Connection connection;
    private int maxDefenses = 3; // Parametros devem ser sujeitos a alteracoes de acordo com o modo de jogo
    private int maxBullets = 3;
    private float countdownTime = 3;
    private bool battleStarted;
    private bool battleEnded; // Isso pode ser desnecessario, vamos ver

    public int MaxDefenses {
        get { return maxDefenses; }
        set {
            if(battleStarted) {
                maxDefenses = value;
            }
            else Debug.Log("Can not change that parameter during game");
        }
    }

    public int MaxBullets {
        get { return maxBullets; }
        set {
            if(battleStarted) {
                maxBullets = value;
            }
            else Debug.Log("Can not change that parameter during game");
        }
    }

    public float CountdownTime {
        get { return countdownTime; }
        set {
            if(battleStarted) {
                countdownTime = value;
            }
            else Debug.Log("Can not change that parameter during game");
        }
    }


    // Use this for initialization
    void Start () {
        timer = new Timer();
        localPlayer = new Player(maxDefenses, maxBullets);
        enemyPlayer = new Player(maxDefenses, maxBullets);

        connection = null;
        battleStarted = false;
        battleEnded = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (battleStarted) {
            if(timer.time <= 0) {// Sempre que o timer não estiver ativo, uma ação deve ser realizada
                timer.StartTimer(countdownTime, SelectAction); // inicia o timer
            }   
        }
        if (battleEnded) {
            // To do, pode ser desnecessario tbm
        }
	}

    // Após a conexão ser estabelecida e for verificado que ela está funcionando, inicia-se a batalha
    public void StartBattle(Connection successfulConection) {
        connection = successfulConection;
        // Inserir aqui qualquer animação de início de batalha
        battleStarted = true;
    }

    // Função a ser chamada quando acabar a batalha
    private void EndBattle(/*Pode precisar de parametros pra saber quem venceu*/) {
        // To do
        battleStarted = false;
        battleEnded = true; // ¯\_(ツ)_/¯
        // To do?
    }

    public void SetPlayerAction(Action action) {
        localPlayer.action = action;
    }
    
    public void SelectAction() {
        // Envia a ação selecionada (esperar connection ser feito para implementar)
        // Recebe a ação do inimigo (esperar connection ser feito para implementar)
        Action enemyAction = Action.NOOP; // placeholder
        Resultado localResult = localPlayer.DoAction();
        enemyPlayer.action = enemyAction;
        Resultado enemyResult = enemyPlayer.DoAction();
        // Compara os resultados das duas ações e, de acordo com o que aconteceu chama as animações
        // Se necessário, chama a função de fim da batalha
    }
}