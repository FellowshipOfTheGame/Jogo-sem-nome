using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {
	
	private GameManager gameManager;
	Button[] buttons = null;
	Slider[] sliders = null;

	void Start(){

		gameManager = Object.FindObjectOfType<GameManager> () as GameManager;
		buttons = Object.FindObjectsOfType<Button>() as Button[];
		sliders = Object.FindObjectsOfType<Slider>() as Slider[];

		Debug.Log("Buttons found:");
		foreach(Button b in buttons)	// Debug
			Debug.Log(b);

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
				Debug.Log("Got here");
				s.minValue = 3;
				s.maxValue = 10;
			}
			else if(s.name.Equals("CountdownTime"))  {
				s.minValue = 2;
				s.maxValue = 10;
			}
			s.enabled = false;
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

	public void Wifi(){

	}

	public void Bluetooth(){

	}

	public void Offline(){

	}

	public void Quit(){

		// Exit editor play mode
		if(Application.isEditor) 
			UnityEditor.EditorApplication.isPlaying = false;
		
		// Exit application
		else 
			Application.Quit();
	}

	public void Back(){
		Debug.Log("VER COMO IMPLEMENTAR UM BACK DECENTE!!!");
		Debug.Log("Back placeholder, sempre volta pro MainMenu");

		List<string> names = new List<string>();

		foreach(Button b in buttons)
			if(b.tag.Equals("MainMenu"))
				names.Add(b.name);
		
		EnableButton(names);
	}

	public void LoadBattleScene() {
		SceneManager.LoadScene("BattleScene");
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