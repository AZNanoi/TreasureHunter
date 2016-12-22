using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EvilAI : MonoBehaviour {
	PlayerControl playerScript;
	Animator e_Animator;
	GameObject player;
	Rigidbody e_Rigidbody;
	public bool attack;
	private bool damage;
	private bool walk;
	private bool death;
	public float e_MoveSpeedMultiplier = 1f;
	public int damageAmount;

	public Transform target;
	public int moveSpeed;
	public float rotationSpeed;
	private Transform myTransform;

	public float attackTimer;
	public float coolDown;

	private float damageTimer;
	public float d_coolDown;

	private int e_curHealth, e_maxHealth;
	private Image e_healthBar;

	public float distanceToPlayer;
	public float distanceToSpawnedPos;

	// Asign new value of E_CurHealth to e_curHealth and run the HandleEvilHealth ()
	public int E_CurHealth {
		get { return e_curHealth;}
		set { 
			e_curHealth = value;
			HandleEvilHealth ();
		}
	}

	public GameObject powerUpHealth;
	public GameObject powerUpSword;
	public Vector3 pu_SpawnPos;

	public Vector3 spawnedPosition;

	public AudioSource[] evilSounds;
	public AudioSource evilPainAudio;
	public AudioSource evilDieAudio;
	public AudioSource playerSwishEvil;

	// Asign the object's transform to the variable myTransform
	void Awake() {
		myTransform = transform;
	}

	// Initiate values to variables, asign objects and audios
	void Start () {
		spawnedPosition = myTransform.position;
		attack = false;
		damage = false;
		walk = false;
		death = false;
		e_Animator = GetComponent<Animator>();
		e_Rigidbody = e_Animator.GetComponent<Rigidbody>();

		player = GameObject.FindGameObjectWithTag ("Player");
		target = player.transform;

		coolDown = 4.0f;
		d_coolDown = 1f;
		damageAmount = 25;

		e_maxHealth = 100;
		e_curHealth = e_maxHealth;
		e_healthBar = myTransform.FindChild("EvilCanvas").FindChild("EvilVHealthBG").FindChild("EvilVHealth").GetComponent<Image> ();

		evilSounds = GetComponents<AudioSource>();
		evilPainAudio = evilSounds [0];
		evilDieAudio = evilSounds [1];
		playerSwishEvil = evilSounds [2];
	}
	
	// Update is called once per frame
	void Update () {
		// Asign the script PlayerControl of the player
		playerScript = player.GetComponent<PlayerControl> ();

		//Calculate the distance between evil and player
		distanceToPlayer = Vector3.Distance (target.transform.position, myTransform.position);

		//Look at target
		StartCoroutine(LookAtTarget(target.position, 0.3f));

		//Walks towards target/player
		if (distanceToPlayer > 1.2 && distanceToPlayer < 10) {
			walk = true;
		}else if (distanceToPlayer > 10 && spawnedPosition != myTransform.position) {
			//Calculate the distance between evil and spawnedPosition
			distanceToSpawnedPos = Vector3.Distance (spawnedPosition, myTransform.position);
			StartCoroutine(LookAtTarget(spawnedPosition, 0.1f));
			walk = true;
		}else {
			walk = false;
		}
		e_Animator.SetBool ("Walk", walk);

		float attackTimer_rounded = Mathf.Round (attackTimer * 10f) / 10f;

		// Damage the player
		if (attack && attackTimer_rounded == 3 && !damage) {
			playerScript.DamagePlayer (true);
		} else {
			playerScript.DamagePlayer (false);
		}

		// Attack the player
		if (!death && distanceToPlayer < 1.2 && attackTimer == 0 && damageTimer == 0) {
			attack = true;
			attackTimer = coolDown;
		} else if(attackTimer_rounded <= 3){
			attack = false;
		}
		e_Animator.SetBool ("Attack", attack);

		// Count down attackTimer and damageTimer
		if (attackTimer > 0)
			attackTimer -= Time.deltaTime;
		if (attackTimer < 0)
			attackTimer = 0;
		if (damageTimer > 0)
			damageTimer -= Time.deltaTime;
		if (damageTimer < 0)
			damageTimer = 0;

		// Play the damage animation of the enemy
		e_Animator.SetBool ("Damage", damage);
		if(damageTimer == 0){
			damage = false;
		}
	}

	// Rotate towards the target in certain time
	IEnumerator LookAtTarget(Vector3 targetPosition, float rotationTime){
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetPosition - myTransform.position), rotationSpeed *Time.deltaTime);
		yield return new WaitForSeconds (rotationTime);
	}

	// Check if the enemy get hit by the player
	// If so plays the damage animation and an audio
	void OnTriggerEnter(Collider other){
		if (other.tag == "PlayerSword" && playerScript.p_attack && damageTimer == 0 && !playerScript.death) {
			//Calculate direction
			Vector3 dir = (other.transform.position - myTransform.position).normalized;
			float direction = Mathf.Abs(Vector3.Dot (dir, myTransform.forward));

			if (direction < 0.6 && playerScript.p_attack) {
				damage = true;
				damageTimer = d_coolDown;
				E_CurHealth -= damageAmount;
				e_Animator.SetBool ("Damage", damage);

				StartCoroutine (playPainAudio());
			}
		}
	}

	// Play pain and swish sound audios
	IEnumerator playPainAudio(){
		if (evilPainAudio.isPlaying) {
			evilPainAudio.Stop ();
			playerSwishEvil.Stop ();
		}
		yield return new WaitForSeconds (0.15f);
		playerSwishEvil.Play ();
		if (!death) {
			evilPainAudio.Play ();
		}
	}

	// Moves the enemy while playing animation
	public void OnAnimatorMove()
	{
		// we implement this function to override the default root motion.
		// this allows us to modify the positional speed before it's applied.
		if (Time.deltaTime > 0)
		{
			Vector3 moveForward = myTransform.forward * e_Animator.GetFloat ("motionZ") * Time.deltaTime;
			Vector3 v = ((e_Animator.deltaPosition + moveForward)*e_MoveSpeedMultiplier) / Time.deltaTime;

			// we preserve the existing y part of the current velocity.
			v.y = e_Rigidbody.velocity.y;
			e_Rigidbody.velocity = v;
		}
	}

	// Reduces the enemy's health in the health bar
	// Spawns a powerup and plays audios if the enemy is death
	public void HandleEvilHealth(){
		if (e_curHealth < 0)
			e_curHealth = 0;

		if (damage && damageTimer != 0) {
			e_healthBar.fillAmount = (float) e_curHealth / (float) e_maxHealth;
		}

		if(e_curHealth == 0){
			death = true;
			SpawnPowerUp ();
			evilPainAudio.Stop ();
			evilDieAudio.Play ();
			StartCoroutine (RemoveEvil ());
		}
		e_Animator.SetBool ("Death", death);

	}

	// Spawn a powerup
	public void SpawnPowerUp(){
		pu_SpawnPos = new Vector3 (myTransform.position.x + 1, myTransform.position.y - 2, myTransform.position.z - 1);
		int choice = UnityEngine.Random.Range (1, 3);
		if (choice == 1) {
			Instantiate (powerUpHealth, pu_SpawnPos, Quaternion.identity);
		} else {
			Instantiate (powerUpSword, pu_SpawnPos, Quaternion.identity);
		}
	}

	// Removes the evil
	IEnumerator RemoveEvil(){
		yield return new WaitForSeconds (4);
		Destroy (gameObject);
	}
}
