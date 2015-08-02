using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {

	//public bool isMuted = false;
	private static AudioScript instance;
	new AudioSource audio;
	// Use this for initialization
	void Awake () {
		if (instance != null && instance != this)
		{
			Destroy (this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
	{
		audio = GetComponent<AudioSource>();
		if (!instance.audio.isPlaying)
		{
			//isMuted = false;
			audio.Play();
		}

	}

	void OnApplicationQuit()
	{
		//isMuted = false;
		instance = null;
	}

	// Update is called once per frame
	public void ToggleMute (bool isMuted = false) {
		if (instance.audio.isPlaying && isMuted == false)
		{
			audio.mute.Equals(true);
			isMuted = true;
		}
		if (isMuted == true)
		{
			audio.mute.Equals(false);
			isMuted = false;
		}
	}
}
