using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actions : MonoBehaviour {

	public string rool_forward;
	public string attack1;
	public string spell;
	public string heal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(rool_forward)) {
			GetComponent<Animator> ().Play ("Roll-Forward");
		}
		if (Input.GetKeyDown(attack1)) {
			GetComponent<Animator> ().Play ("Attack1");
		}
		if (Input.GetKeyDown(spell)) {
			// spell
		}
		if (Input.GetKeyDown(heal)) {
			// heal
		}
	}
}
