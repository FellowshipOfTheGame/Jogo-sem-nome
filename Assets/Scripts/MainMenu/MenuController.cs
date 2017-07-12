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

	public GameObject playerPrefab;
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
        	this.gameManager = tmp.GetComponent<GameManager>() as GameManager;
        
        // Get SceneManager reference
        tmp = GameObject.FindGameObjectWithTag("SceneManager");
        if(tmp != null)
        	this.sceneManager = tmp.GetComponent<SceneChanger>() as SceneChanger;

        this.menus = GameObject.FindGameObjectsWithTag("Menu");
		this.sliders = Object.FindObjectsOfType<Slider>() as Slider[]; // TODO

		this.localIp.gameObject.SetActive(false);
		this.serverIp.gameObject.SetActive(false);

		foreach (GameObject go in menus){

			Button[] buttons = go.GetComponentsInChildren<Button>() as Button[];
			
			foreach(Button b in buttons){
				
				// Disable bluetooth button until we have it
				if(b.name.Equals("Bluetooth"))
					b.interactable = false;

				else if(b.name.Equals("NS só queira deixar quadradinho bunitu :3"))
					b.interactable = false;
			}
			
			// Enable first menu and deactivate the rest
			if(go.name.Equals("MainMenu"))
				go.SetActive(true);
			else go.SetActive(false);
		}

		foreach(Slider s in this.sliders) {
			if(s.name.Equals("MaxDefenses") || s.name.Equals("MaxBullets")) {
				s.minValue = 3;
				s.maxValue = 10;
			}
			else if(s.name.Equals("CountdownTime"))  {
				s.minValue = 2;
				s.maxValue = 10;
			}
			s.gameObject.SetActive(false);
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
		foreach(Slider s in sliders)
			if(s.tag.Equals("OptionMenu"))
				names.Add(s.name);

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

	public void ChangedMaxDef(){
		foreach(Slider s in sliders) {
			if(s.name.Equals("MaxDefenses")) {
				gameManager.MaxDefenses = (int) s.value;
			}
		}
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
		
		this.localIp.gameObject.SetActive(true);
		
		foreach(GameObject go in this.menus){
			if(go.name.Equals("ServerMenu"))
				go.SetActive(true);
			else
				go.SetActive(false);
		}

		Host();
	}

	public void Host(){
		
		Wifi wifi = gameObject.AddComponent<Wifi>();
		this.connection = wifi; // Store a reference to this connection

		// Set prefab
		wifi.SetPlayerPrefab(this.playerPrefab);
		wifi.isHost = true;

		// Start hosting
		wifi.Connect();
	}

	public void Join(){

		Wifi wifi = gameObject.AddComponent<Wifi>();
		this.connection = wifi; // Store a reference to this connection

		// Set prefab
		wifi.SetPlayerPrefab(this.playerPrefab);

		// Join server
		wifi.SetIpAddress(userInputIP);
		wifi.Connect();
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

    public void ChangedCountdownTime() {
        foreach(Slider s in sliders) {
			if(s.name.Equals("MaxBullets")) {
				gameManager.MaxBullets = (int) s.value;
			}
		}
    }

    public void ChangedMaxBullets() {
       foreach(Slider s in sliders) {
			if(s.name.Equals("CountdownTime")) {
				gameManager.CountdownTime = s.value;
			}
		}
	}

	private void EnableGameObject(string[] names){
		EnableGameObject(new List<string>(names));
	}

	private void EnableGameObject(List<string> names){
	
		if(names.Count == 0){
			foreach(GameObject go in this.menus)
				go.gameObject.SetActive(false);
			foreach(Slider s in sliders)
				s.enabled = false;

		} else {
			foreach(GameObject go in this.menus){
				if(names.Contains(go.name))
					go.gameObject.SetActive(true);
				else 
					go.gameObject.SetActive(false);
			}
			foreach(Slider s in sliders){
				if(names.Contains(s.name))
					s.enabled = true;
				else 
					s.enabled = false;
			}
		}
	}
}