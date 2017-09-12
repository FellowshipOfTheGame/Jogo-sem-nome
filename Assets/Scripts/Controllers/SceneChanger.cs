using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour {

    private GameObject background;
    private Vector3 target;
    private bool noFunctionsQueued;
    public float backgroundSpeed;
    public bool Moving { get; private set; }

    public GameManager gm;
    public Canvas cv;

    // Use this for initialization
    private void Awake() {
        backgroundSpeed = 50f;
        background = FindBackground();
        queuedFunction = FunctionQueue.None;
    }

    void Start() {
        Moving = false;
        background.transform.position = new Vector3(3.115f, 6.25f, 0.0f);
        target = background.transform.position;
        DontDestroyOnLoad(gameObject);
        LoadMenuScene(true);
    }

    // Update is called once per frame
    void Update () {
        // Checks if the background needs to move
        if (background.transform.position != target) {
            Vector3 nextPosition;
            float displacement;
            // Checks if the background needs to move on the x axis
            if (background.transform.position.x != target.x) {
                // Calculates the next position
                displacement = Mathf.Sign(target.x - background.transform.position.x) * backgroundSpeed * Time.deltaTime;
                nextPosition = new Vector3(background.transform.position.x + displacement, background.transform.position.y, 0.0f);
                // If the next position goes over the target, make next position be same as target
                if (Mathf.Sign(target.x - background.transform.position.x) != Mathf.Sign(target.x - nextPosition.x))
                    nextPosition.x = target.x;
                // Updates background position
                background.transform.position = nextPosition;
            // Only moves on the y axis if it doesn't need to move o the x axis
            } else if (background.transform.position.y != target.y) {
                // Exactly the same behaviour as above, but on the y axis
                displacement = Mathf.Sign(target.y - background.transform.position.y) * backgroundSpeed * Time.deltaTime;
                nextPosition = new Vector3(background.transform.position.x, background.transform.position.y + displacement, 0.0f);
                if (Mathf.Sign(target.y - background.transform.position.y) != Mathf.Sign(target.y - nextPosition.y))
                    nextPosition.y = target.y;
                background.transform.position = nextPosition;
            } else { // This shouldn't happen
                Debug.Log("The background should not have been moved along the z axis");
            }
        }else {
            Moving = false;
        }
	}

    public void MoveUp() {
        Moving = true;
        target = new Vector3(target.x, target.y - 10.52f, target.z);
    }

    public void MoveDown() {
        Moving = true;
        target = new Vector3(target.x, target.y + 10.52f, target.z);
    }

    public void MoveRight() {
        Moving = true;
        target = new Vector3(target.x - 6.21f, target.y, target.z);
    }

    public void MoveLeft() {
        Moving = true;
        target = new Vector3(target.x + 6.21f, target.y, target.z);
    }

    // Auxiliary function to get background reference
    private GameObject FindBackground() {
        foreach (Transform child in transform) {
            if (child.gameObject.tag == "Background")
                return child.gameObject;
        }
        return null;
    }

    private void OnMainMenuLoad(Scene scene, LoadSceneMode mode) {
        GameObject mc = GameObject.FindGameObjectWithTag("MenuController");
        MenuController menuController = mc.GetComponent<MenuController>();
        menuController.Play();
        gm.findCanvas();
    }

    private void OnBattleSceneLoad(Scene scene, LoadSceneMode mode) {
        gm.StartBattle();
    }

    public void LoadMenuScene(bool gameStart) {
        // If it's coming back from the battle scene, moves background
        if (!gameStart) {
            MoveLeft();
            // And queues a function that simulates the play button being pressed
            SceneManager.sceneLoaded += MoveBGFindCanvas;
            queuedFunction = FunctionQueue.MenuAgain;
        } else {
            gm.findCanvas();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadBattleScene(Connection successfulConection) {
        
        MoveRight();
        
        // Removes the OnMainMenuLoad function from queue
        if (!noFunctionsQueued)
            SceneManager.sceneLoaded -= OnMainMenuLoad;
        
        // Adds a different function to be called when the scene is loaded
        SceneManager.sceneLoaded += OnBattleSceneLoad;
        noFunctionsQueued = false;
        SceneManager.LoadScene("BattleScene");
        gm.connection = successfulConection;
    }
}
