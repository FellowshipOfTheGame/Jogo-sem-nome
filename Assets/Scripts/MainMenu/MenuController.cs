using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

enum MenuPosition {
    MAIN,
    OPTIONS,
    CREDITS,
    PLAY
}

public class MenuController : MonoBehaviour {
    
	public GameObject dummy;
	public Text localIp;
	public InputField serverIp;
	
	private GameManager gameManager;
    private SceneChanger sceneManager;
	
    private GameObject background;
    private MenuPosition position;
	private Connection connection = null;
	private string userInputIP = null;
	
	private GameObject[] menus = null;
	private Slider[] sliders = null;

	void Awake(){

        position = MenuPosition.MAIN;

		// Get GameManager reference
		GameObject tmp = GameObject.FindGameObjectWithTag("GameManager");
							
        if(tmp != null)
        	this.gameManager = tmp.GetComponent<GameManager>();
        
        // Get SceneManager reference
        tmp = GameObject.FindGameObjectWithTag("SceneManager");
        if(tmp != null)
        	this.sceneManager = tmp.GetComponent<SceneChanger>();

        this.menus = GameObject.FindGameObjectsWithTag("Menu");

		this.localIp.gameObject.SetActive(false);
		this.serverIp.gameObject.SetActive(false);

		foreach (GameObject go in menus){
            
			// Enable first menu and deactivate the rest
			if(go.name.Equals("MainMenu"))
				go.SetActive(true);
			else go.SetActive(false);
		}
	}

	public void Play(){
		
		List<string> names = new List<string>();

		foreach(GameObject go in this.menus)
			if(go.name.Equals("PlayMenu"))
				names.Add(go.name);

		EnableGameObject(names);
        sceneManager.MoveRight();
        position = MenuPosition.PLAY;
	}

	public void Options(){

		List<string> names = new List<string>();

		foreach(GameObject go in this.menus)
			if(go.name.Equals("OptionsMenu"))
				names.Add(go.name);

		EnableGameObject(names);
        sceneManager.MoveLeft();
        position = MenuPosition.OPTIONS;
	}

	public void Crebitz(){
		
		List<string> names = new List<string>();

		foreach(GameObject go in this.menus)
			if(go.name.Equals("CrebitzMenu"))
				names.Add(go.name);

		EnableGameObject(names);
        sceneManager.MoveUp();
        position = MenuPosition.CREDITS;
	}

	public void Wifi(){

		List<string> names = new List<string>();

		foreach(GameObject go in this.menus)
			if(go.name.Equals("WifiMenu"))
				names.Add(go.name);

		EnableGameObject(names);
	}

	private void SetIp(string ip){ this.userInputIP = ip; }

	public void Client(){

		serverIp.gameObject.SetActive(true);
		serverIp.onEndEdit.AddListener(SetIp);

		foreach(GameObject go in this.menus){
			if(go.name.Equals("ClientMenu"))
				go.SetActive(true);
			else
				go.SetActive(false);
		}
	}

	public void Server(){

		Wifi wifi = gameObject.AddComponent<Wifi>();
		this.connection = wifi; // Store a reference to this connection
		
		this.localIp.gameObject.SetActive(true);
		localIp.text = "Your IP: " + (this.connection as Wifi).GetLocalIp();
		
		foreach(GameObject go in this.menus){
			if(go.name.Equals("ServerMenu"))
				go.SetActive(true);
			else
				go.SetActive(false);
		}

		Host();
	}

	public void Host(){

		Wifi wifi = this.connection as Wifi;

		// Set prefab
		wifi.SetPlayerPrefab(this.dummy);
		wifi.isHost = true;

		// Start hosting
		wifi.Connect();
	}

	public void Join(){

		Wifi wifi = gameObject.AddComponent<Wifi>();
		this.connection = wifi; // Store a reference to this connection

		// Set prefab
		wifi.SetPlayerPrefab(this.dummy);

		// Join server
		wifi.SetIpAddress(userInputIP);
		wifi.Connect();
	}

	public void LoadWifiBattle(){
		sceneManager.LoadBattleScene(connection);
	}

	public void Bluetooth(){
		return;
	}

	public void Offline(){
        // For testing reasons, there is no prompt or confirmation
        gameManager.gameObject.AddComponent<Offline>();
        connection = gameManager.gameObject.GetComponent<Offline>();
        sceneManager.LoadBattleScene(connection);
	}

	public void Quit(){

#if UNITY_EDITOR
		if(Application.isEditor)  // Exit editor play mode
			UnityEditor.EditorApplication.isPlaying = false;
		else 
#endif
			Application.Quit(); // Exit application
	}

    public void Back() {

        if (!sceneManager.Moving) {
            switch (position) {
                case MenuPosition.CREDITS:
                    sceneManager.MoveDown();
                    break;
                case MenuPosition.OPTIONS:
                    sceneManager.MoveRight();
                    break;
                case MenuPosition.PLAY:
                    sceneManager.MoveLeft();
                    break;
            }
            position = MenuPosition.MAIN;

            localIp.gameObject.SetActive(false);
            serverIp.gameObject.SetActive(false);
            List<string> names = new List<string>();

            if(this.connection){
				connection.CloseConnection();
				connection = null;
			}

            foreach (GameObject go in this.menus)
                if (go.name.Equals("MainMenu"))
                    names.Add(go.name);

            EnableGameObject(names);
        }
	}

    public void UpdateMaxDefenses(float value) {
        gameManager.MaxDefenses = (int)value;
    }

    public void UpdateMaxBullets(float value) {
        gameManager.MaxBullets = (int)value;
    }    public void UpdateCountdownTime(float value) {
        gameManager.CountdownTime = (int)value;
    }

    private void EnableGameObject(string[] names){
		EnableGameObject(new List<string>(names));
	}

	private void EnableGameObject(List<string> names){
	
		if(names.Count == 0){
			foreach(GameObject go in this.menus)
				go.gameObject.SetActive(false);

		} else {
			foreach(GameObject go in this.menus){
				if(names.Contains(go.name))
					go.gameObject.SetActive(true);
				else 
					go.gameObject.SetActive(false);
			}
		}
	}
}