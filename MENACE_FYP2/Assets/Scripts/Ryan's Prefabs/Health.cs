using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 1000;
	public int curHealth = 1000;

	public float healthBarLength;
	private GameObject[] GoingToDestroy;
	
	void Start () {
		healthBarLength = Screen.width / 8;
		this.GoingToDestroy = GameObject.FindGameObjectsWithTag("SelectableUnits");
		this.GoingToDestroy = GameObject.FindGameObjectsWithTag("Eney");
	}
	
	
	void Update () {
		if (GameObject.FindGameObjectWithTag("Eney"))
		AddjustCurrentHealth(0);
	}
	
	
	void OnGUI(){
		Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
		GUI.Box(new Rect(screenPos.x, Screen.height - screenPos.y, healthBarLength, 20), curHealth + "/" + maxHealth);	

	}
	
	
	public void AddjustCurrentHealth(int adj) {
		curHealth += adj;

		//foreach(GameObject stuff in GoingToDestroy)
		//{
			if(curHealth < 0)
			{
				curHealth = 0;
				//GameObject.Destroy(stuff);
				Destroy(this.gameObject);
				//for(int i = 0; i < GoingToDestroy.Length; i++)
				//{
					//Destroy(GoingToDestroy[i].gameObject);
				//}
			}
			
			if(curHealth > maxHealth)
				curHealth = maxHealth;
			
			if(maxHealth < 1)
				maxHealth = 1;
			
			healthBarLength = (Screen.width / 8) * (curHealth / (float)maxHealth);
		//}
	}
}
