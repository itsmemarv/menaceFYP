using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	public GameObject target;
	public float attackTime;
	public float coolDown;
	
	public AudioClip sfxClip;
	public AudioSource audioSFX;
	
	void Start () {
		attackTime = 0;
		coolDown = 0.0f;

		audioSFX = (AudioSource)gameObject.AddComponent<AudioSource> ();
		audioSFX.clip = sfxClip;
		audioSFX.loop = false;
		audioSFX.playOnAwake = false;
		
	}
	
	
	void Update () {
		if(attackTime > 0)
			attackTime -= Time.deltaTime;
		
		if(attackTime < 0)
			attackTime = 0;
		
		
		if(attackTime == 0) {
			Attacker();
			attackTime = coolDown;
		}
		
	}

	void OnTriggerEnter(Collider other)
	{
		audioSFX.PlayOneShot (sfxClip);
		Health eh = other.gameObject.GetComponent<Health>();
		if (other.gameObject.tag == "Eney") {
			eh.AddjustCurrentHealth(Random.Range(-800,-100)); //* Time.DeltaTime;
		}
	}

	private void Attacker() {
		float distance = Vector3.Distance(target.transform.position, transform.position);


		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot(dir, transform.forward);


		if(distance < 2.5f && GameObject.FindGameObjectWithTag("Eney")) {
			//if(direction > 0) { 
			Health eh = (Health)target.GetComponent("Enemy");
			eh.AddjustCurrentHealth(Random.Range(-800,-100));
			//}
		}
	}
}