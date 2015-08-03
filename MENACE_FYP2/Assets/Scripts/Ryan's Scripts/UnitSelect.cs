using UnityEngine;
using System.Collections;

public class UnitSelect : MonoBehaviour {

	public Color originalColor;
	public bool isSelected;
	
	private void OnSelected()
	{
		isSelected = true;
		GetComponent<Renderer>().material.color = Color.red;
	}
	
	private void OnUnselected()
	{
		isSelected = false;
		GetComponent<Renderer>().material.color = originalColor;
	}
}
