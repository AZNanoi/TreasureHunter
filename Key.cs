using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Key : MonoBehaviour {

	// Check if the player collides the key
	// If so disable the key and show the key image on the screen.
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			gameObject.SetActive (false);
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().displayKey ();
		}
	}
}
