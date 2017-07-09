using UnityEngine;
using UnityEngine.UI;
// using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	private GameManager gameManager;
    private SceneChanger sceneManager;
	public Text localIp;
	public InputField serverIp;


	private Connection connection = null;
	private string userInputIP = null;
	private Button[] buttons = null;
	private Slider[] sliders = null;

	void Awake(){

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneChanger>();

		buttons = Object.FindObjectsOfType<Button>() as Button[];
		sliders = Object.FindObjectsOfType<Slider>() as Slider[];

		localIp.gameObject.SetActive(false);
		serverIp.gameObject.SetActive(false);

		foreach(Button b in buttons){
			
			// Disable bluetooth button until we have it
			if(b.name.Equals("Bluetooth"))
				b.interactable = false;

			else if(b.name.Equals("NS só queira deixar quadradinho bunitu :3"))
				b.interactable = false;

			// Enable firsts buttons and deactivate the rest
			if(b.tag.Equals("MainMenu"))
				b.gameObject.SetActive(true);
			else b.gameObject.SetActive(false);
		}

		foreach(Slider s in sliders) {
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

		foreach(Button b in buttons)
			if(b.tag.Equals("PlayMenu"))
				names.Add(b.name);

		EnableButton(names);
	}

	public void Options(){

		List<string> names = new List<string>();

		foreach(Button b in buttons)
			if(b.tag.Equals("OptionMenu"))
				names.Add(b.name);
		foreach(Slider s in sliders)
			if(s.tag.Equals("OptionMenu"))
				names.Add(s.name);

		EnableButton(names);
	}

	public void Crebitz(){
		
		List<string> names = new List<string>();

		foreach(Button b in buttons)
			if(b.tag.Equals("CrebitzMenu"))
				names.Add(b.name);

		EnableButton(names);
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

		foreach(Button b in buttons)
			if(b.tag.Equals("WifiMenu"))
				names.Add(b.name);

		EnableButton(names);
	}

	private void SetIp(string ip){
	
		userInputIP = ip;
	}

	public void Client(){

		serverIp.gameObject.SetActive(true);
		serverIp.onEndEdit.AddListener(SetIp);

		foreach(Button b in buttons){
			if(b.tag.Equals("ClientMenu"))
				b.gameObject.SetActive(true);
			else
				b.gameObject.SetActive(false);
		}
	}

	public void Server(){
		
		localIp.gameObject.SetActive(true);
		
		foreach(Button b in buttons){
			if(b.tag.Equals("ServerMenu"))
				b.gameObject.SetActive(true);
			else
				b.gameObject.SetActive(false);
		}
	}

	public void Host(){

	}

	public void Join(){

        // Arrumar esse cast?
        this.connection = new Wifi();
        // gameManager.gameObject.AddComponent<Wifi>();
        // connection = gameManager.gameObject.GetComponent<Wifi>();		// Create a new connection
        
		Debug.Log("[Debug]: Connection: " + connection);

		Debug.Log("Connecting to: " + userInputIP);

		// Join server
		// wifi.SetIpAddress(userInputIP);
		// wifi.Connect();

	}

	public void Bluetooth(){

	}

	public void Offline(){
        // For testing reasons, there is no prompt or confirmation
        gameManager.gameObject.AddComponent<Offline>();
        connection = gameManager.gameObject.GetComponent<Offline>();
        sceneManager.LoadBattleScene(connection);
	}

	public void Quit(){

		// Exit editor play mode
		// if(Application.isEditor) 
		// 	UnityEditor.EditorApplication.isPlaying = false;
		
		// Exit application
		// else 
			Application.Quit();
	}

	public void Back(){

		localIp.gameObject.SetActive(false);
		serverIp.gameObject.SetActive(false);
		List<string> names = new List<string>();

		if (connection != null)
			connection.CloseConnection();

		foreach(Button b in buttons)
			if(b.tag.Equals("MainMenu"))
				names.Add(b.name);
		
		EnableButton(names);

		this.connection = null;
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

	private void EnableButton(string[] names){
		EnableButton(new List<string>(names));
	}

	private void EnableButton(List<string> names){
	
		if(names.Count == 0){
			foreach(Button b in buttons)
				b.gameObject.SetActive(false);
			foreach(Slider s in sliders)
				s.enabled = false;

		} else {
			foreach(Button b in buttons){
				if(names.Contains(b.name))
					b.gameObject.SetActive(true);
				else 
					b.gameObject.SetActive(false);
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