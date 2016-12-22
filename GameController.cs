using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public GameObject evil;
	public GUIText treasureBoxText;
	public GUIText endText;
	public GUIText doorText;

	private bool gameEnded;
	private Scene currentScene;

	public AudioSource[] sounds;
	public AudioSource power_upSound;

	public GameObject StoneVillage;
	public GameObject RainVillage;
	public GameObject Castle;
	public GameObject Bridge;
	public GameObject Player;

	// Disable game objects and GUItexts.
	void Awake() {
		RainVillage.SetActive (false);
		StoneVillage.SetActive (false);
		Castle.SetActive (false);
		Bridge.SetActive (false);
		treasureBoxText.gameObject.SetActive (false);
		endText.gameObject.SetActive (false);
	}

	// Initiate the current scene, powerup audio and the variable gameEnded 
	void Start () {
		gameEnded = false;
		currentScene = SceneManager.GetActiveScene ();

		sounds = GetComponents<AudioSource>();
		power_upSound = sounds [0];
	}
	
	// Update is called once per frame
	void Update(){
		// Load the current scene when the game is over
		if (gameEnded) {
			if(Input.GetKeyDown (KeyCode.R)){
				SceneManager.LoadScene (currentScene.name);
			}
		}

		//Calculate the distance between player and RainVillage
		float disPlayerToRainVillage = Vector3.Distance (Player.transform.position, RainVillage.transform.position);
		if (disPlayerToRainVillage < 200f) {
			RainVillage.SetActive (true);
		} else {
			RainVillage.SetActive (false);
		}

		//Calculate the distance between player and Castle
		float disPlayerToCastle = Vector3.Distance (Player.transform.position, Castle.transform.position);
		if (disPlayerToCastle < 250f) {
			Castle.SetActive (true);
		} else {
			Castle.SetActive (false);
		}

		//Calculate the distance between player and Bridge
		float disPlayerToBridge = Vector3.Distance (Player.transform.position, Bridge.transform.position);
		if (disPlayerToBridge < 100f) {
			Bridge.SetActive (true);
		} else {
			Bridge.SetActive (false);
		}

		//Calculate the distance between player and StoneVillage
		float disPlayerToStoneVillage = Vector3.Distance (Player.transform.position, StoneVillage.transform.position);
		if (disPlayerToStoneVillage < 250f) {
			StoneVillage.SetActive (true);
		} else {
			StoneVillage.SetActive (false);
		}
	}

	// Spawn enemies in random position within the given boundary
	public void SpawnEvil (Vector3 Pos, string boundary) {
		int amount = 0;
		if(boundary == "startAreaBoundary"){
			amount = 2;
		}else if(boundary == "rainVillageBoundary"){
			amount = 6;
		}else if(boundary == "castleBoundary"){
			amount = 8;
		}else if(boundary == "stoneVillageBoundary"){
			amount = 10;
		}else if(boundary == "smallBoundary"){
			amount = 3;
		}

		for(int j = 1; j <= amount; j++){
			Vector3 spawnPos = new Vector3 (Random.Range(Pos.x - 8, Pos.x + 8), 0, Random.Range(Pos.z - 3, Pos.z + 3));
			Instantiate (evil, spawnPos, Quaternion.identity);
		}
	}

	// Display the treasure box texts
	public void displayTBText(string message){
		if (message != "" && !gameEnded) {
			treasureBoxText.text = message;
			treasureBoxText.gameObject.SetActive (true);
		} else {
			treasureBoxText.gameObject.SetActive (false);
		}
	}

	// Display the end text
	public IEnumerator displayEndText(string message){
		gameEnded = true;
		treasureBoxText.gameObject.SetActive (false);
		yield return new WaitForSeconds (1);
		endText.text = message;
		endText.gameObject.SetActive (true);
	}

	// Enable the key image at the top right corner of the screen
	public void displayKey(){
		GameObject.Find ("Canvas/Key").GetComponent<Image> ().enabled = true;
	}

	// Remove all living enemies
	public void destroyAllEvils(){
		GameObject[] evils = GameObject.FindGameObjectsWithTag ("Evil");
		for(int i = 0; i < evils.Length; ++i){
			Destroy (evils[i]);
		}
	}

	// Display key text
	public void displayKeyText(bool display, string message){
		doorText.gameObject.SetActive (display);
		doorText.text = message;
	}
}
