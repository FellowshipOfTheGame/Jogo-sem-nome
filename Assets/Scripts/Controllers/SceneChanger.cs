using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour {

    public GameManager gm;
    public Canvas cv;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        LoadMenuScene();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void LoadMenuScene() {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadBattleScene(Connection successfulConection) {
        Debug.Log("entrou");
        SceneManager.LoadScene("BattleScene");
        Debug.Log("criou");
        gm.StartBattle(successfulConection);
    }
}
