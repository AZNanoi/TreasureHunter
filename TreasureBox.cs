using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TreasureBox : MonoBehaviour {
	private bool keyPickedUP;
	private Animator boxAnimator;
	public GameObject player;
	private GameController controller;

	// Initiate keyPickedUP, animator, player and game controller
	void Start () {
		keyPickedUP = false;
		boxAnimator = gameObject.GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}

	// Opens the treasure box or display texts
	void Update () {
		//Calculate the distance between the chest and player
		float distance = Vector3.Distance (player.transform.position, transform.position);

		//Calculate the player's direction toward the chest
		Vector3 dir = (player.transform.position - transform.position).normalized;
		float direction = Vector3.Dot (dir, transform.forward);

		// Open the chest or display texts to unlock the chest or get the key and disable the player
		if (distance < 1.2 && direction < 1 && direction > 0) {
			keyPickedUP = GameObject.Find ("Canvas/Key").GetComponent<Image> ().enabled;
			if (keyPickedUP) {
				controller.displayTBText ("Press the U key to unlock the chest!");
				if(Input.GetKeyDown (KeyCode.U)){
					boxAnimator.SetBool ("Opened", true);
					string message = "Congratulations, you've finished the level!\n                   Press R to restart";
					StartCoroutine (controller.displayEndText(message));
					player.SetActive (false);
				}
			} else {
				controller.displayTBText ("You are missing the key. Get the key first!");
			}
		} else {
			controller.displayTBText ("");
		}
	}
}
