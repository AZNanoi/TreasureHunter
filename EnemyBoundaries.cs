using UnityEngine;
using System.Collections;

public class EnemyBoundaries : MonoBehaviour {
	GameController gameController;
	public bool playerIsInCastle;
	public bool playerIsInStoneVillage;

	// initiate game controller and variables playerIsInCastle and PlayerIsInStoneVillage
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		playerIsInCastle = false;
		playerIsInStoneVillage = false;
	}
	
	// Check if the player enters the boundary.
	// If so spawn enemies in the boundry and notify that the player is in castle or stonevillage.
	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Player"){
			gameController.SpawnEvil (transform.position, gameObject.tag);
			if(gameObject.tag == "castleBoundary")
				playerIsInCastle = true;

			if(gameObject.tag == "stoneVillageBoundary")
				playerIsInStoneVillage = true;
		}
	}

	// Check if the player exits the boundary.
	// Destroy all enemies in the boundary and notify that the player is not in castle or stonevillage.
	private void OnTriggerExit(Collider other) {
		if(other.tag == "Player"){
			gameController.destroyAllEvils();
			if(gameObject.tag == "castleBoundary")
				playerIsInCastle = false;

			if(gameObject.tag == "stoneVillageBoundary")
				playerIsInStoneVillage = false;
		}
	}
}
