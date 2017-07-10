using UnityEngine;
using System.Collections;

public enum Animation: byte { NOAMMO, CANATK, NODEF, CANDEF, NOREL, CANREL, NOTHING, DEATH, ATKHIT, ATKMISS, DEFHIT, DEFMISS, RELOK, RELFAIL, TUMBLEWEED}
public enum Result : byte { VICTORY, DEFEAT, DRAW }
public enum Action : byte { NOOP, ATK, DEF, REL }

// Recebe as mensagens do player e do inimigo conectado, avalia o comando nesse turno e envia o resultado calculado para os dois
public class GameManager : MonoBehaviour {


    // metodo para receber/enviar mensagem do/ao player, metodo para receber/enviar mensagem do/ao inimigo
    // criar scripts auxiliares(Ex.: AnimationController) para nao sobrecarregar o GameManager
    
    private Player localPlayer, enemyPlayer;
    private GameObject[] playerObjects;
    private Timer timer;
    private GameObject timerObject;
    private uint i;
	public Connection connection;
    private GameObject canvasObject;
    public bool battleStarted;
    public SceneChanger sc;
    public bool battleEnded { get; private set; }
    public GameObject playerPrefab, enemyPrefab, timerPrefab;

    private int maxDefenses;
    private int maxBullets;
    private float countdownTime;
    private bool turnHappening, playerAnimFinished, enemyAnimFinished;

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

    public void findCanvas() {
        canvasObject = GameObject.FindGameObjectWithTag("Canvas");
    }

    // Use this for initialization
    void Start () {
        // Define que o manager não deve ser destruído e qual o seu estado inicial
        DontDestroyOnLoad(gameObject);
        playerObjects = new GameObject[2];
        playerObjects[0] = playerObjects[1] = null;
        timer = null;
        maxBullets = 3;
        maxDefenses = 3;
        countdownTime = 3.0f;

        connection = null;
        battleStarted = false;
        battleEnded = true;
    }
	
	// Update is called once per frame
	void Update () {
        // Loop de batalha
        if (battleStarted) {
            // Verifica se o player já acabou sua animação e caso true armazena esse resultado
            if (!playerAnimFinished) {
                playerAnimFinished = localPlayer.FinishedAnimation;
            }
            // Verifica se a animação do inimigo já acabou
            if (!enemyAnimFinished) {
                enemyAnimFinished = enemyPlayer.FinishedAnimation;
            }
            // Caso as duas animações tenham acabado
            if (playerAnimFinished && enemyAnimFinished) {
                // Indica que o turno terminou
                playerAnimFinished = false;
                enemyAnimFinished = false;
                turnHappening = false;
            }
            // Caso o turno tenha acabado e o timer esteja em 0
            if (timer.time <= 0 && !turnHappening) {// Sempre que o timer não estiver ativo, uma ação deve ser realizada
                // Caso necessário, reinicia o timer
                if (!battleEnded)
                    timer.StartTimer(countdownTime, SelectAction, true); // inicia o timer
                // Senão, acaba a batalha
                else
                    EndBattle();
            }
        }
	}

    // Após a conexão ser estabelecida e for verificado que ela está funcionando, inicia-se a batalha
    public void StartBattle() {
        findCanvas();
        // Instancia a prefab do player
        playerObjects[0] = GameObject.Instantiate(playerPrefab, gameObject.transform);
        // Obtem uma referencia para o script e configura
        localPlayer = playerObjects[0].GetComponent<Player>();
        localPlayer.Configure(maxDefenses, maxBullets);
        // Faz o mesmo para o inimigo
        playerObjects[1] = GameObject.Instantiate(enemyPrefab, gameObject.transform);
        enemyPlayer = playerObjects[1].GetComponent<Player>();
        enemyPlayer.Configure(maxDefenses, maxBullets);
        
        // Instancia e salva referencia para o timer
        timerObject = GameObject.Instantiate(timerPrefab, canvasObject.transform);
        timer = timerObject.GetComponent<Timer>();

        // Inserir aqui qualquer animação de início de batalha

        // Seta as variáveis booleanas que indicam o estado da batalha
        playerAnimFinished = false;
        enemyAnimFinished = false;
        turnHappening = false;
        battleStarted = true;
        battleEnded = false;
    }

    // Função a ser chamada quando acabar a batalha
    private void EndBattle() {

        // Avalia qual foi o resultado
        Result result = localPlayer.alive? Result.VICTORY : enemyPlayer.alive? Result.DEFEAT : Result.DRAW;

        // Faz alguma coisa (Mensagem de vitória/derrota talvez)

        // Volta as configurações para o default
        maxBullets = 3;
        maxDefenses = 3;
        countdownTime = 3;
        // Destrói os objetos e componentes desnecessários
        Destroy(playerObjects[0]);
        Destroy(playerObjects[1]);
        playerObjects[0] = playerObjects[1] = null;
        Destroy(timerObject);
        timerObject = null;
        localPlayer = null;
        enemyPlayer = null;
        Destroy(connection);
        connection = null;
        battleStarted = false;
        canvasObject = null;

        // Muda de cena
        sc.LoadMenuScene(false);
    }

    public void SetPlayerAction(Action selectedAction) {
        localPlayer.action = selectedAction;
    }

    private void SelectAction() {

        // Avisa que o turno começou
        turnHappening = true;

        // Envia a ação selecionada
        sendLocalAction(localPlayer.action);
        Animation localAnimation = localPlayer.DoAction();
        // Recebe a ação do inimigo
        enemyPlayer.action = getEnemyAction();
        Animation enemyAnimation = enemyPlayer.DoAction();
        // Compara os Animations das duas ações e, de acordo com o que aconteceu chama as animações de reação
        localPlayer.DoReaction(localAnimation, enemyAnimation);
        enemyPlayer.DoReaction(enemyAnimation, localAnimation);
        // Se necessário, indica que acabou a batalha
        if (!localPlayer.alive || !enemyPlayer.alive)
            battleEnded = true;
    }

    private Action getEnemyAction () {
        string message = connection.GetMessage();
        switch (message) {
            case "ATK":
                return Action.ATK;
            case "DEF":
                return Action.DEF;
            case "REL":
                return Action.REL;
            case "NOOP":
                return Action.NOOP;
        }
        throw new System.Exception("Invalid message received");
    }

    private void sendLocalAction(Action localAction) {
        switch (localAction) {
            case Action.ATK:
                connection.OtterSendMessage("ATK");
                break;
            case Action.DEF:
                connection.OtterSendMessage("DEF");
                break;
            case Action.REL:
                connection.OtterSendMessage("REL");
                break;
            case Action.NOOP:
                connection.OtterSendMessage("NOOP");
                break;
            default:
                throw new System.Exception("Trying to send invalid message");
        }
    }
}