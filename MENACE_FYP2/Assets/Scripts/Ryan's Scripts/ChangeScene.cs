using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {
	
	public void ChangeToScene (string sceneToChangeTo) {
		//Fade(); 
		Application.LoadLevel(sceneToChangeTo);
	}

	public void QuitGame ()
	{
		Application.Quit();
	}

	IEnumerator Fade()
	{
		float fadeTime = GameObject.Find("_GM").GetComponent<Fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
	}
}
