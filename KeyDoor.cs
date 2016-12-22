using UnityEngine;
using System.Collections;

public class KeyDoor : MonoBehaviour {
	private float rotationSpeed; // degress per second

	private Vector3 curEuler;
	private bool doorRotating;
	private bool playerIsInCastle;
	private GameObject[] evils;
	private bool doorOpened;
	private GameObject Player;
	private GameController controller;
	private EnemyBoundaries castleBoundary;

	// Initiate the status of the door, the game objects castleBoundary, player and game controller
	void Start () {
		curEuler = transform.eulerAngles;
		doorRotating = false;
		rotationSpeed = 15.0f;
		doorOpened = false;

		castleBoundary = GameObject.FindGameObjectWithTag ("castleBoundary").GetComponent<EnemyBoundaries> ();
		Player = GameObject.FindGameObjectWithTag ("Player");
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		playerIsInCastle = castleBoundary.playerIsInCastle;
		evils = GameObject.FindGameObjectsWithTag ("Evil");
		// Opens the key door
		if (playerIsInCastle && evils.Length == 0 && !doorOpened) {
			StartCoroutine (OpenKeyDoor(90f));
		}

		float disPlayerToKeyDoor = Vector3.Distance (Player.transform.position, transform.FindChild("default").transform.position);
		// Displays the key text
		if (disPlayerToKeyDoor < 1.2 && !doorOpened) {
			controller.displayKeyText (true, "Kill all the evils in the Village to unlock the door!");
		} else {
			controller.displayKeyText (false, "");
		}
	}

	// Opens the key door
	IEnumerator OpenKeyDoor(float desiredAngle){
		if (doorRotating) yield break; // ignore calls to RotateAngle while rotating
		doorRotating = true;  // set the flag
		float newAngle = curEuler.y+desiredAngle; // calculate the new angle
		while (curEuler.y < newAngle){
			// move a little step at constant speed to the new angle:
			curEuler.y = Mathf.MoveTowards(curEuler.y, newAngle, rotationSpeed*Time.deltaTime);
			transform.eulerAngles = curEuler; // update the object's rotation...
			yield return null;
		}
		doorRotating = false;
		doorOpened = true;
	}
}
