using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour {

	private Vector3 startPos;
	private Quaternion startRot;

	public bool isDead = false;

	private GameObject rayHit;
	private AI ai;

	PlayerInventory PI;
	// Use this for initialization
	void Start () {
		rayHit = GameObject.Find ("RayHit");
		startPos = transform.position;
		startRot = transform.rotation;
		PI = GetComponent<PlayerInventory>();
		//print(PI.maxHealth);

		StartCoroutine(regeneManaHp());
	}

	public float regeneMana;
	public float regeneHp;

	// Update is called once per frame
	private bool isAttacking = false;
	private float attackCooldown = 0.5f;
	private float currentCooldown = 0.5f;
	private float attackRange = 2.3f;

	public GameObject lightningSpell;
	public float lightSpellCost;
	public float lightSpellSpeed;

	public GameObject healSpell;
	public float healSpellCost;
	public float healPower;

	void Update () {
		

		
		/*if(Input.anyKeyDown){

			print(Input.inputString);
		
		}*/
		if (isAttacking) {
			currentCooldown -= Time.deltaTime;
		}
		if (currentCooldown <= 0) {
			currentCooldown = attackCooldown;
			isAttacking = false;
			//print ("fin attack");
		}

		// a change dans action.cs
		if (Input.GetKeyDown("a")) {
			//print("Appui sur a");
			if (!isAttacking) {
				isAttacking = true;	

				RaycastHit hit;

				if (Physics.Raycast(rayHit.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
				{
					//Debug.DrawLine (rayHit.transform.position, hit.point, Color.red);
					if (hit.transform.tag == "Enemy") {
						hit.transform.GetComponent<AI> ().ApplyDammage (PI.currentDamage);
						//print ("ATTACK LA PUTE avc" + PI.currentDamage);
					} 
				}
			}
		}



		//spell a change dans action.cs
		if (Input.GetKeyDown("n")) {
			//print("Appui sur n");

			if (!isAttacking && PI.currentMana >= lightSpellCost) {
				GetComponent<Animator> ().Play ("Attack1");
				isAttacking = true;	
				GameObject spell = Instantiate (lightningSpell, rayHit.transform.position, transform.rotation) as GameObject; 
				spell.GetComponent<Rigidbody>(). AddForce (transform.forward * lightSpellSpeed);
				PI.currentMana -= lightSpellCost;
			}
		}

		//spell a change dans action.cs
		if (Input.GetKeyDown("b")) {
			//print("Appui sur n");

			if (!isAttacking && PI.currentMana >= healSpellCost) {
				GetComponent<Animator> ().Play ("Crouching");
				isAttacking = true;	
				GameObject spell = Instantiate (healSpell, rayHit.transform.position, transform.rotation) as GameObject; 
				Destroy(spell, 0.7f);
				PI.currentMana -= healSpellCost;
				PI.currentHealth += healPower;
			}
		}


		if (Input.GetKeyDown("m")) {
			isDead = true;
		}

		if (isDead == true) {
			StartCoroutine(WaitDie());
		}
	}

	IEnumerator regeneManaHp() {
		while (true) {
			yield return new WaitForSeconds(1);
			if (PI.currentMana < PI.maxMana) {
				PI.currentMana += regeneMana;
			}
			if (PI.currentHealth < PI.maxHealth) {
				PI.currentHealth += regeneHp;
			}
		}
	}

	private bool inRes = false;
	IEnumerator WaitDie()
	{
		if (inRes == false) {
			PI.currentHealth = 0;
			inRes = true;
			//print ("DIE BITCH");
			GetComponent<Animator> ().Play ("LOSE00");
			yield return new WaitForSeconds(3);
			transform.position = startPos;
			transform.rotation = startRot;
			GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
			//GetComponent<Animator> ().Play ("LOSE00");
			isDead = false;
			PI.currentHealth = PI.maxHealth;
			inRes = false;
		}
	}

	void Hit() {
		//print ("find ?");
	}

	void OnTriggerEnter(Collider col) {
//		print ("t'es mort");
		if (col.tag == "death") {
			transform.position = startPos;
			transform.rotation = startRot;
			GetComponent<Animator> ().Play ("LOSE00");
			GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
		}
	}
}
