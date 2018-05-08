using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour {

//	public Transform Target;
//	public UnityEngine.AI.NavMeshAgent agent;
//
//	// Use this for initialization
//	void Start () {
//		agent = gameObject.GetComponent < UnityEngine.AI.NavMeshAgent > ();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		agent.destination = Target.position;	
//	}

	public GameObject myPrefab;

	// Distance entre le joueur et l'ennemi
	private float Distance;

	// Distance entre l'ennemi et sa position de base
	private float DistanceBase;
	private Vector3 basePositions;

	// Cible de l'ennemi
	public Transform Target;

	//Distance de poursuite
	public float chaseRange = 10;

	// Portée des attaques
	public float attackRange = 2.5f;

	// Cooldown des attaques
	public float attackRepeatTime = 1;
	private float attackTime;

	// Montant des dégâts infligés
	public float TheDammage;

	// Agent de navigation
	private UnityEngine.AI.NavMeshAgent agent;

	// Animations de l'ennemi
	private Animation animations;

	// Vie de l'ennemi
	public float enemyHealth;
	private bool isDead = false;

	// loots de l'ennemi
	public GameObject[] loots;


	void Start () {

		agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		animations = gameObject.GetComponent<Animation>();
		attackTime = Time.time;
		basePositions = transform.position;

		textDmgUser = GameObject.Find ("Dmg_user").GetComponent<Text>();
		textDmgMonster = GameObject.Find ("Dmg_monster").GetComponent<Text>();
		//textDmgUser.enabled = false;
	}



	void Update () {

		if (!isDead)
		{

			// On cherche le joueur en permanence
			Target = GameObject.Find("Player").transform;

			// On calcule la distance entre le joueur et l'ennemi, en fonction de cette distance on effectue diverses actions
			Distance = Vector3.Distance(Target.position, transform.position);

			// On calcule la distance entre l'ennemi et sa position de base
			DistanceBase = Vector3.Distance(basePositions, transform.position);

			// Quand l'ennemi est loin = idle
			if (Distance > chaseRange && DistanceBase <= 1)
			{
				idle();
			}

			// Quand l'ennemi est proche mais pas assez pour attaquer
			if (Distance < chaseRange && Distance > attackRange)
			{
//				print ("chase");
				chase();
			}

			// Quand l'ennemi est assez proche pour attaquer
			if (Distance < attackRange)
			{
				attack();
			}

			// Quand le joueur s'est échappé
			if (Distance > chaseRange && DistanceBase > 1) {
				BackBase();
			}

		}
	}

	// poursuite
	void chase()
	{
		animations.Play("walk");
		agent.destination = Target.position;
	}


	Text textDmgUser;
	Text textDmgMonster;

	// Combat
	void attack()
	{
		// empeche l'ennemi de traverser le joueur
		agent.destination = transform.position;

		//Si pas de cooldown
		if (Time.time > attackTime) {
			animations.Play("hit");
			Target.GetComponent<PlayerInventory>().ApplyDamage(TheDammage);
//			showDmg (TheDammage);
			StartCoroutine(showDmg(TheDammage));
			//Debug.Log("L'ennemi a envoyé " + TheDammage + " points de dégâts");
			attackTime = Time.time + attackRepeatTime;
		}
	}

	IEnumerator showDmg(float dmg) {
		
		Text clone = Instantiate (textDmgUser);
		//clone.renderer.enabled = true;
		clone.enabled = true;
		clone.transform.parent = GameObject.Find ("Canvas - Inventory(Clone)").transform;
		clone.text = "-" + dmg;
		clone.fontSize = 40;

		clone.transform.position = textDmgUser.transform.position;

		int a = 0;
		while (a < 10) {
			yield return new WaitForSeconds(0.03f);
			var tmp = clone.transform.position;
			tmp.y += 6;
			clone.transform.position = tmp;
			a++;
		}
		clone.enabled = false;
		Destroy (clone, 3);
	}

	// idle
	void idle()
	{
		animations.Play("idle");
	}

	public void ApplyDammage(float TheDammage)
	{
		if (!isDead)
		{
			enemyHealth = enemyHealth - TheDammage;
			//print(gameObject.name + "a subit " + TheDammage + " points de dégâts.");
			StartCoroutine(showDmgMonster(TheDammage));
			if(enemyHealth <= 0)
			{
				Dead();
			}
		}
	}

	IEnumerator showDmgMonster(float dmg) {

		Text clone = Instantiate (textDmgMonster);
		//clone.renderer.enabled = true;
		clone.enabled = true;
		clone.transform.parent = GameObject.Find ("Canvas - Inventory(Clone)").transform;
		clone.text = ""+dmg;
		clone.fontSize = 40;

		clone.transform.position = textDmgMonster.transform.position;

		int a = 0;
		while (a < 10) {
			yield return new WaitForSeconds(0.03f);
			var tmp = clone.transform.position;
			tmp.y += 6;
			clone.transform.position = tmp;
			a++;
		}
		clone.enabled = false;
		Destroy (clone, 3);
	}

	// Retour a la base si le joueur s'est échappé et si notre ennemi est trop loin de sa base
	public void BackBase() {
		animations.Play("walk");
		agent.destination = basePositions;
	}

	public void Dead()
	{
		gameObject.GetComponent<CapsuleCollider>().enabled = false;
		isDead = true;
		animations.Play("die");

		// apparition du loot
		int randomNumber = Random.Range(0, loots.Length);
		GameObject finalLoot = loots[randomNumber];
		Instantiate(finalLoot, transform.position, transform.rotation);

		//StartCoroutine(respawnMonster());

		Destroy(transform.gameObject, 5);


	}
		
	IEnumerator respawnMonster() {
		print ("Respawn lance");
		yield return new WaitForSeconds(15f);
		Instantiate(myPrefab); 
		print ("Respawn fini");
	}
}
