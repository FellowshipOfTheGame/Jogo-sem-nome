using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum MenuPosition {
    MAIN,
    OPTIONS,
    CREDITS,
    PLAY,
    SWIPEBLOCKED
}

public delegate void MoveEvent();

public class MenuController : MonoBehaviour {

    private GameManager gm;
    private SceneChanger sceneManager;

    private GameObject background;
    private MenuPosition position;
    private Connection connection = null;
    private string userInputIP = null;

    private GameObject[] menus = null;
    private Button[] buttons = null;
    private Slider[] sliders = null;

    private MoveEvent moveRoutine = null;

    public MenuPosition Position { get { return position; } }
    public GameObject bulletHole;
	public GameObject playerPrefab;

	public GameObject dummy;
	public Text localIp;
	public InputField serverIp;
	

	void Awake(){

        position = MenuPosition.MAIN;

		// Get GameManager reference
		GameObject tmp = GameObject.FindGameObjectWithTag("GameManager");
							
        if(tmp != null)
        	this.gm = tmp.GetComponent<GameManager>();
        
        // Get SceneManager reference
        tmp = GameObject.FindGameObjectWithTag("SceneManager");
        if(tmp != null)
        	this.sceneManager = tmp.GetComponent<SceneChanger>();

        this.menus = GameObject.FindGameObjectsWithTag("Menu");

		this.localIp.gameObject.SetActive(false);
		this.serverIp.gameObject.SetActive(false);

		foreach(GameObject go in menus){
			
			// Enable first menu and deactivate the rest
			if(go.name.Equals("MainMenu"))
				go.SetActive(true);
			else go.SetActive(false);
		}
	}

    public void LoadWifiBattle(){
		sceneManager.LoadBattleScene(connection);
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

	/********************/
    /* Events Callbacks */
    /********************/

	public void UpdateMaxDefenses(float value){ gm.MaxDefenses = (int)value; }
    public void UpdateMaxBullets(float value){ gm.MaxBullets = (int)value; }
    public void UpdateCountdownTime(float value){ gm.CountdownTime = (int)value; }
	private void SetIp(string ip){ this.userInputIP = ip; }

    /********************/
    /* Button Functions */
    /********************/

	public void ButtonGeneric(){
		
		string bName = gameObject.name;
		string tag = "";


		switch(bName){
		case "Play":
			tag = "PlayMenu";
			moveRoutine = sceneManager.MoveRight;
			break;
		case "Options":
			tag = "OptionsMenu";
			moveRoutine = sceneManager.MoveLeft;
			break;
		case "Crebitz":
			tag = "CrebitzMenu";
			moveRoutine = sceneManager.MoveUp;
			break;
		case "Wifi":
			tag = "WifiMenu";
			// moveRoutine = sceneManager.Fade;
			break;
		case "Client":
			tag = "ClientMenu";
			// moveRoutine = sceneManager.Fade;
			break;

		case "Back":
			tag = "ClientMenu";
			Back();
			return;

		case "Quit":
			tag = "ClientMenu";
			Quit();
			return;
		
		default:
			Debug.Log("Error");
			break;
		}

		if(position != MenuPosition.CREDITS){
			
			// Spawn bullet hole
			Vector2 pos = Input.GetTouch(0).position; // Touch position
			/*
            // TO DO: this.buttons does not currently have the desired value
			foreach (Button b in this.buttons){
				
				RectTransform rect = b.GetComponent<RectTransform>();

				// If touch is inside a button on screen
				if(RectTransformUtility.
					RectangleContainsScreenPoint(rect, pos, GetComponent<Camera>())){

					Destroy(Instantiate(bulletHole, pos, Quaternion.identity), 1.0f);
					break;
				}
			}
            */
		}

		List<string> names = new List<string>();

		foreach(GameObject go in this.menus)
			if(go.name.Equals(tag))
				names.Add(go.name);

		EnableGameObject(names);
        if(moveRoutine != null) moveRoutine();
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

        foreach (GameObject obj in this.menus)
            if (obj.name.Equals("OptionsMenu")) {
                names.Add(obj.name);
            }

		EnableGameObject(names);
        sceneManager.MoveLeft();
        position = MenuPosition.OPTIONS;

        GameObject go = GameObject.Find("MaxDefensesSlider");
        go.GetComponent<Slider>().value = gm.MaxDefenses;
        go = GameObject.Find("MaxBulletsSlider");
        go.GetComponent<Slider>().value = gm.MaxBullets;
        go = GameObject.Find("CountdownTimeSlider");
        go.GetComponent<Slider>().value = gm.CountdownTime;
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
        // Blocks Swiping
        position = MenuPosition.SWIPEBLOCKED;

		foreach(GameObject go in this.menus)
			if(go.name.Equals("WifiMenu"))
				names.Add(go.name);

		EnableGameObject(names);
	}

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

		if(this.connection){
			Debug.Log("[Debug]: Closing connection...");
			connection.CloseConnection();
			connection = null;
		}

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

		if(this.connection){
			Debug.Log("[Debug] Closing connection...");
			connection.CloseConnection();
			connection = null;
		}

		Wifi wifi = gameObject.AddComponent<Wifi>();
		this.connection = wifi; // Store a reference to this connection

		// Set prefab
		wifi.SetPlayerPrefab(this.dummy);
		wifi.isHost = false;

		// Join server
		wifi.SetIpAddress(userInputIP);
		wifi.Connect();
	}

	public void Bluetooth() {
        List<string> names = new List<string>();
        
        // Blocks Swiping
        position = MenuPosition.SWIPEBLOCKED;

        foreach (GameObject go in this.menus)
            if (go.name.Equals("BluetoothMenu"))
                names.Add(go.name);

        EnableGameObject(names);
    }

	public void Offline(){
        // NOTE: For testing reasons, there is no prompt or confirmation
        connection = gm.gameObject.AddComponent<Offline>();
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

    public void ClientServerBack() {

        localIp.gameObject.SetActive(false);
        serverIp.gameObject.SetActive(false);

        if (this.connection) {
            connection.CloseConnection();
            connection = null;
        }

        List<string> names = new List<string>();

        foreach (GameObject go in this.menus)
            if (go.name.Equals("WifiMenu"))
                names.Add(go.name);

        EnableGameObject(names);
    }

    public void WifiBack() {

        // Unblocks Swiping
        position = MenuPosition.PLAY;

        localIp.gameObject.SetActive(false);
        serverIp.gameObject.SetActive(false);
        List<string> names = new List<string>();

        if (this.connection) {
            connection.CloseConnection();
            connection = null;
        }

        foreach (GameObject go in this.menus)
            if (go.name.Equals("PlayMenu"))
                names.Add(go.name);

        EnableGameObject(names);

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

            List<string> names = new List<string>();
            foreach (GameObject go in this.menus)
                if (go.name.Equals("MainMenu"))
                    names.Add(go.name);

            EnableGameObject(names);
        }
	}
}
