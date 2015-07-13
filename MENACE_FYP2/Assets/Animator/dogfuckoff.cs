using UnityEngine;
using System.Collections;

public class dogfuckoff : MonoBehaviour {

	Animator anim;
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Space))
		   anim.SetBool("fly",true);
		else
			anim.SetBool("fly",false);

	}
}
