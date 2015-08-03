using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	public GameObject target;
	public float attackTime;
	public float coolDown;
	
	public AudioClip enemysfxClip;
	public AudioSource enemyaudioSFX;
	
	void Start () {
		attackTime = 0;
		coolDown = 0.0f;

		enemyaudioSFX = (AudioSource)gameObject.AddComponent<AudioSource> ();
		enemyaudioSFX.clip = enemysfxClip;
		enemyaudioSFX.loop = false;
		enemyaudioSFX.playOnAwake = false;
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
		enemyaudioSFX.PlayOneShot (enemysfxClip);
		Health eh = other.gameObject.GetComponent<Health>();
		if (other.gameObject.tag == "SelectableUnits") {
			eh.AddjustCurrentHealth(Random.Range(-800,-100)); //* Time.DeltaTime;
		}
	}
	
	private void Attacker() {
		float distance = Vector3.Distance(target.transform.position, transform.position);
		
		
		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot(dir, transform.forward);
		
		
		if(distance < 2.5f && GameObject.FindGameObjectWithTag("SelectableUnits")) {
			if(direction > 0) { 
				Health eh = (Health)target.GetComponent("Cube(Clone)");
				//Health eh = target.GetComponent<Health>();
				eh.AddjustCurrentHealth(Random.Range(-800,-100));
			}
		}
	}
}