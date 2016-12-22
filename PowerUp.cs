using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class PowerUp : MonoBehaviour {
	PlayerControl player;
	GameController controller;

	// Initiate the game objects player and game controller
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}

	// Check if the player collides the powerup particle
	// If so increase the player health or swordpower, play the powerup sound and remove the powerup
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			if (gameObject.tag == "PowerUpHealth") {
				player.HealPlayer (5);
			} else if (gameObject.tag == "PowerUpSword") {
				player.HandleSwordPower ();
			}
			if (controller.power_upSound.isPlaying){
				controller.power_upSound.Stop ();
			}
			controller.power_upSound.Play ();
			Destroy (transform.parent.gameObject);
		}
	}
}
