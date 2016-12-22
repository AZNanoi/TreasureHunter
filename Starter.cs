using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Starter : MonoBehaviour {
	private Text instruction;
	private Text loading;
	private Text startText;

	// Initiate and disable the intruction, loading and startText texts
	void Start () {
		instruction = GameObject.Find ("Instruction").GetComponent<Text> ();
		instruction.enabled = false;
		loading = GameObject.Find ("Loading").GetComponent<Text> ();
		loading.enabled = false;
		startText = GameObject.Find ("StartText").GetComponent<Text> ();
	}
	
	// Start the game or display the instructon text
	void Update () {
		if(Input.GetKeyDown (KeyCode.Space)){
			startText.enabled = false;
			instruction.enabled = false;
			loading.enabled = true;
			SceneManager.LoadScene ("Level1");
		}else if(Input.GetKeyDown (KeyCode.I)){
			startText.enabled = false;
			instruction.enabled = true;
		}
	}
}
