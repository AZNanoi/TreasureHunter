using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof (ThirdPersonCharacter))]
	public class PlayerControl : MonoBehaviour
	{
		private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
		private Transform m_Cam;                  // A reference to the main camera in the scenes transform
		private Vector3 m_CamForward;             // The current forward direction of the camera
		private Vector3 m_Move;
		private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
		private Animator p_Animator;
		private bool run;
		public bool swingSword;
		public bool death;
		private bool p_Damage;

		public bool p_attack;

		public float curHealth;

		// Asign new values of the CurHealth to curHealth and run the function HandleHealth ()
		private float CurHealth
		{
			get{ return curHealth;}
			set{ 
				curHealth = value;
				HandleHealth ();
			}
		}

		private float maxHealth;
		private float maxXValue;
		private float barLength;
		private Image p_HealthBar_I;
		private Image p_HealthBar_LifeSymbol;
		private RectTransform p_HealthBar_T;
		private float yValue;

		private GameObject[] evils;

		public Image swordPower;
		public Image swordBar;
		private bool usingSwordPower;
		private ParticleSystem p_SwordParticle;

		public AudioSource[] sounds;
		public AudioSource swordSwish;
		public AudioSource painAudio;
		public AudioSource dieAudio;

		// Initiate variables, game objects, transform of the main camera, animator, health/sword bar, player's sword and audios
		private void Start()
		{
			run = false;
			p_Damage = false;
			p_attack = false;
			usingSwordPower = false;
			death = false;

			maxHealth = 100;
			curHealth = maxHealth;

			p_HealthBar_T = GameObject.Find ("VisualHealth").GetComponent<RectTransform> ();
			p_HealthBar_I = GameObject.Find ("VisualHealth").GetComponent<Image> ();
			p_HealthBar_LifeSymbol = GameObject.Find ("LifeSymbol").GetComponent<Image> ();
			barLength = p_HealthBar_T.rect.width;
			yValue = p_HealthBar_T.localPosition.y;

			// get the transform of the main camera
			if (Camera.main != null)
			{
				m_Cam = Camera.main.transform;
			}
			else
			{
				Debug.LogWarning(
					"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
				// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
			}

			// get the third person character
			m_Character = GetComponent<ThirdPersonCharacter>();
			p_Animator = m_Character.GetComponent<Animator> ();

			swordPower = GameObject.Find ("VisualSwordPower").GetComponent<Image> ();
			swordBar = swordPower.transform.parent.parent.GetComponent<Image> ();

			p_SwordParticle = GameObject.FindGameObjectWithTag ("PlayerSword").GetComponent<ParticleSystem> ();
			p_SwordParticle.Stop ();

			sounds = GetComponents<AudioSource>();
			swordSwish = sounds [0];
			painAudio = sounds [1];
			dieAudio = sounds [2];
		}

		// Handle the player's inputs and play animations, get all the living enemies
		private void Update()
		{
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift) && !death) {
				run = true;
			} else {
				run = false;
			}

			if (Input.GetKeyDown (KeyCode.J) && !death) {
				m_Jump = true;
			} else {
				m_Jump = false;
			}

			if (!p_attack && Input.GetKeyDown (KeyCode.Space) && !death) {
				swingSword = true;
				StartCoroutine (AttackCountDown());

			} else {
				swingSword = false;
			}

			if (Input.GetKeyDown (KeyCode.P) && !usingSwordPower && !death) {
				usingSwordPower = true;
				StartCoroutine (useSwordPower ());
			}

			p_Animator.SetBool ("Run", run);
			p_Animator.SetBool ("SwingSword", swingSword);
			p_Animator.SetBool ("Damage", p_Damage);

			// play dying animation
			if(curHealth <= 0)
				p_Animator.SetBool ("Death", death);

			// Stop the player moving forward
			if(Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl) && !death){
				p_Animator.SetFloat ("Forward", 0.0f);
			}

			evils = GameObject.FindGameObjectsWithTag ("Evil");
		}


		// Fixed update is called in sync with physics
		private void FixedUpdate()
		{	
			float h = CrossPlatformInputManager.GetAxis("Horizontal");
			float v = CrossPlatformInputManager.GetAxis("Vertical");
			bool crouch = Input.GetKey(KeyCode.C);

			// calculate move direction to pass to character
			if (m_Cam != null)
			{
				// calculate camera relative direction to move:
				m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
				m_Move = v*m_CamForward + h*m_Cam.right;
			}
			else
			{
				// we use world-relative directions in the case of no main camera
				m_Move = v*Vector3.forward + h*Vector3.right;
			}
			#if !MOBILE_INPUT
			// walk speed multiplier
			if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
			#endif

			// pass all parameters to the character control script
			m_Character.Move(m_Move, crouch, m_Jump);
			m_Jump = false;
		}

		// Reduce the player's health, play damage animation and pain sound
		public void DamagePlayer(bool d){
			p_Damage = d;
			if(p_Damage && !death){
				CurHealth -= 5;
				p_Animator.SetBool ("Damage", p_Damage);

				if(painAudio.isPlaying){
					painAudio.Stop ();
				}
				painAudio.Play ();
			}
		}

		// Increase the player's health
		public void HealPlayer(int life){
			if(curHealth < 100){
				CurHealth += life;
			}
		}

		// Calculate the position or color of the visual health bar with the value of the current health
		private float MapValues(float curH, float barL, float maxH)
		{
			return curH * (barL / maxH) - barL;
		}

		// Reduce/increase the visual health bar, change the color of it
		// Play the player's dealth animation and audio, and display the game over text
		private void HandleHealth()
		{
			float curXValue = MapValues(curHealth, barLength, maxHealth);
			p_HealthBar_T.localPosition = new Vector3(curXValue, yValue);

			if (curHealth > maxHealth / 2) {
				byte redChannel = (byte)(255f - MapValues (curHealth, 255, maxHealth / 2));
				p_HealthBar_I.color = new Color32 (redChannel, 255, 0, 255);
				p_HealthBar_LifeSymbol.color = new Color32 (redChannel, 255, 0, 255);
			} else {
				byte greenChannel = (byte)(MapValues (curHealth, 255, maxHealth / 2) + 255F);
				p_HealthBar_I.color = new Color32 (255, greenChannel, 0, 255);
				p_HealthBar_LifeSymbol.color = new Color32 (255, greenChannel, 0, 255);
			}

			if (curHealth <= 0){
				death = true;
				p_Animator.SetBool ("Death", death);
				dieAudio.Play ();
				string message = "Game Over! Press R to restart";
				StartCoroutine (GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().displayEndText (message));
			}
		}

		// Count down the attack timer
		IEnumerator AttackCountDown(){
			p_attack = true;
			yield return new WaitForSeconds (0.5f);
			swordSwish.Play ();
			yield return new WaitForSeconds (0.5f);
			p_attack = false;
		}

		// Play dying sound for the player
		IEnumerator playDieAudio(){
			dieAudio.Play ();
			yield return new WaitForSeconds (1);
			p_attack = false;
		}

		// Increase the power of the sword in the sword bar
		public void HandleSwordPower(){
			if(swordPower.fillAmount < 1){
				swordPower.fillAmount += 0.2f;
				swordBar.color = new Color32 (0, 255, 255, 255);
			}
		}

		// Activate the swordpower particles on the player's sword, increase in the damage amount on the evils
		// Reset the swordpower and the damage amount of the evils after 1 minute
		IEnumerator useSwordPower(){
			if(swordPower.fillAmount == 1f){
				for (int i = 0; i < evils.Length; ++i) {
					if(evils [i]){
						evils [i].GetComponent<EvilAI> ().damageAmount = 50;
					}
				}
				p_SwordParticle.Play ();
				swordPower.fillAmount = 0f;
				swordBar.color = new Color32 (208, 208, 208, 255);
				yield return new WaitForSeconds (60);

				for (int i = 0; i < evils.Length; ++i) {
					if(evils [i]){
						evils [i].GetComponent<EvilAI> ().damageAmount = 25;
					}
				}
				usingSwordPower = false;
				p_SwordParticle.Stop ();
			}
		}
	}
}
