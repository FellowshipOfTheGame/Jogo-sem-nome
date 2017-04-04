using UnityEngine;
using System.Collections;

public enum Animation: byte { NOAMMO, CANATK, NODEF, CANDEF, NOREL, CANREL, NOTHING, DEATH, ATKHIT, ATKMISS, DEFHIT, DEFMISS}
public enum Result : byte { VICTORY, DEFEAT, DRAW }
public enum Action : byte { NOOP, ATK, DEF, REL }

// Recebe as mensagens do player e do inimigo conectado, avalia o comando nesse turno e envia o resultado calculado para os dois
public class GameManager : MonoBehaviour {


    // metodo para receber/enviar mensagem do/ao player, metodo para receber/enviar mensagem do/ao inimigo
    // criar scripts auxiliares(Ex.: AnimationController) para nao sobrecarregar o GameManager

    private Player localPlayer, enemyPlayer;
	private Timer timer;
	private Connection connection;
    private bool battleStarted;
    public SceneChanger sc;
    public bool battleEnded { get; private set; }

    private int maxDefenses;
    private int maxBullets;
    private float countdownTime;

    public int MaxDefenses {
        get { return maxDefenses; }
        set {
            if(!battleStarted) {
                maxDefenses = value;
            }
            else Debug.Log("Can not change that parameter during game");
        }
    }

    public int MaxBullets {
        get { return maxBullets; }
        set {
            if(!battleStarted) {
                maxBullets = value;
            }
            else Debug.Log("Can not change that parameter during game");
        }
    }

    public float CountdownTime {
        get { return countdownTime; }
        set {
            if (!battleStarted) {
                countdownTime = value;
            } else Debug.Log("Can not change that parameter during game");
        }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
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
                localPlayer.action = Action.NOOP;
                timer.StartTimer(countdownTime, SelectAction); // inicia o timer
            }   
        }
	}

    // Após a conexão ser estabelecida e for verificado que ela está funcionando, inicia-se a batalha
    public void StartBattle(Connection successfulConection) {
        sc.LoadBattleScene(successfulConection);
        connection = successfulConection;
        // Instancia a prefab do player
        GameObject go = Instantiate(Resources.Load("Assets/PlayerCharacter"), gameObject.transform) as GameObject;
        // Obtem uma referencia para o script e configura
        localPlayer = go.GetComponent<Player>();
        localPlayer.Configure(maxDefenses, maxBullets);
        // Faz o mesmo para o inimigo
        go = Instantiate(Resources.Load("Assets/EnemyCharacter"), gameObject.transform) as GameObject;
        enemyPlayer = go.GetComponent<Player>();
        enemyPlayer.Configure(maxDefenses, maxBullets);

        // Inserir aqui qualquer animação de início de batalha

        battleStarted = true;
    }

    // Função a ser chamada quando acabar a batalha
    private void EndBattle() {

        Result result = localPlayer.alive? Result.VICTORY : enemyPlayer.alive? Result.DEFEAT : Result.DRAW;

        // Faz alguma coisa

        // Volta as configurações para o default
        maxBullets = 3;
        maxDefenses = 3;
        countdownTime = 3;

        Destroy(localPlayer.gameObject);
        Destroy(enemyPlayer.gameObject);

        connection = null;
        battleStarted = false;
        battleEnded = true;

        // Muda de cena
        sc.LoadMenuScene();
    }

    public void SetPlayerAction(Action selectedAction) {
        localPlayer.action = selectedAction;
    }

    public void SelectAction() {
        // Envia a ação selecionada (esperar connection ser feito para implementar)
        Animation localAnimation = localPlayer.DoAction();
        // Recebe a ação do inimigo (esperar connection ser feito para implementar)
        Action enemyAction = Action.NOOP; // placeholder
        enemyPlayer.action = enemyAction; // placeholder
        Animation enemyAnimation = enemyPlayer.DoAction();
        // Compara os Animations das duas ações e, de acordo com o que aconteceu chama as animações de reação
        localPlayer.DoReaction(localAnimation, enemyAnimation);
        enemyPlayer.DoReaction(enemyAnimation, localAnimation);
        // Se necessário, chama a função de fim da Result
        if (!localPlayer.alive || !enemyPlayer.alive)
            EndBattle();
    }
}