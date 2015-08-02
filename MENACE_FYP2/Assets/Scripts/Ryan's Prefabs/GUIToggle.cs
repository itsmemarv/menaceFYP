using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIToggle : MonoBehaviour {

	Toggle musicToggle;
	// Use this for initialization
	void Start () {
	
	}

	/*public void MuteToggle (bool muteToggle) {
		if(muteToggle == false)
		{
			AudioListener.volume = 1;
		}
		if(muteToggle == true)
		{
			AudioListener.volume = 0;
		}
	}*/

	public void SetMute()
	{
		if(musicToggle.isOn)
		{
			AudioListener.volume = 1;
		}
		else
		{
			AudioListener.volume = 0;
		}
	}
}
