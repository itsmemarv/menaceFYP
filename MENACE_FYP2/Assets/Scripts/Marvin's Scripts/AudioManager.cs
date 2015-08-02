using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioClip newMusic;
	void Awake(){
		GameObject go = GameObject.Find ("MenuSound");

		if (go.name != newMusic.name) {
			go.GetComponent<AudioSource>().clip = newMusic;
			Debug.Log ("Music Changed!");
		}
		go.GetComponent<AudioSource>().Play ();
		Debug.Log ("AMVolume " + go.GetComponent<AudioSource>().volume);
		Debug.Log ("Music Name " + go.GetComponent<AudioSource>().name);
		}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
