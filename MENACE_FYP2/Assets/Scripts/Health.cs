using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 100;
	public int curHealth = 100;
	
	public float healthBarLength;
	
	
	void Start () {
		healthBarLength = Screen.width / 8;
	}
	
	
	void Update () {
		AddjustCurrentHealth(0);
		
	}
	
	
	void OnGUI(){
		Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
		GUI.Box(new Rect(screenPos.x, Screen.height - screenPos.y, healthBarLength, 20), curHealth + "/" + maxHealth);	

	}
	
	
	public void AddjustCurrentHealth(int adj) {
		curHealth += adj;	
		
		if(curHealth < 0)
			curHealth = 0;
		
		if(curHealth > maxHealth)
			curHealth = maxHealth;
		
		if(maxHealth < 1)
			maxHealth = 1;
		
		healthBarLength = (Screen.width / 8) * (curHealth / (float)maxHealth);
	}
}
