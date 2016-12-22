using UnityEngine;
using System.Collections;

public class TreasureHouseDoor : MonoBehaviour {
	private float rotationSpeed; // degress per second

	private Vector3 curEuler;
	private bool doorRotating;
	private bool playerIsInStoneVillage;
	private GameObject[] evils;
	private bool treasureDoorOpened;
	private GameObject Player;
	private GameController controller;
	private EnemyBoundaries StoneVillageBoundary;

	// Initiate the status of the door of the treasure house, game objects stoneVillageBoundary, player and game controller
	void Start () {
		curEuler = transform.eulerAngles;
		doorRotating = false;
		rotationSpeed = 15.0f;
		treasureDoorOpened = false;

		StoneVillageBoundary = GameObject.FindGameObjectWithTag ("stoneVillageBoundary").GetComponent<EnemyBoundaries> ();
		Player = GameObject.FindGameObjectWithTag ("Player");
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
	
	// Open the door of the treasure house or display the text to kill the evils
	void Update () {
		playerIsInStoneVillage = StoneVillageBoundary.playerIsInStoneVillage;
		evils = GameObject.FindGameObjectsWithTag ("Evil");
		if (playerIsInStoneVillage && evils.Length == 0 && !treasureDoorOpened) {
			StartCoroutine (OpenKeyDoor(90f));
		}

		float disPlayerToKeyDoor = Vector3.Distance (Player.transform.position, transform.transform.position);
		if (disPlayerToKeyDoor < 4 && !treasureDoorOpened) {
			controller.displayKeyText (true, "Kill all the evils in the Village to unlock the door!");
		} else {
			controller.displayKeyText (false, "");
		}
	}

	// Rotate the left/right treasure door
	IEnumerator OpenKeyDoor(float desiredAngle){
		if (doorRotating) yield break; // ignore calls to RotateAngle while rotating
		doorRotating = true;  // set the flag
		float new_Angle;
		if (transform.tag == "TreasureDoor_right") {
			new_Angle = curEuler.z - desiredAngle; // calculate the new angle
			while (curEuler.z > new_Angle){
				// move a little step at constant speed to the new angle:
				curEuler.z = Mathf.MoveTowards(curEuler.z, new_Angle, rotationSpeed*Time.deltaTime);
				transform.eulerAngles = curEuler; // update the object's rotation...
				yield return null;
			}
		} else {
			new_Angle = curEuler.z + desiredAngle;
			while (curEuler.z < new_Angle){
				// move a little step at constant speed to the new angle:
				curEuler.z = Mathf.MoveTowards(curEuler.z, new_Angle, rotationSpeed*Time.deltaTime);
				transform.eulerAngles = curEuler; // update the object's rotation...
				yield return null;
			}
		}
		doorRotating = false;
		treasureDoorOpened = true;
	}
}
