using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {
	// Use this for initialization
	private static AudioScript instance;
	public static AudioScript GetInstance() {
			return instance;
	}

	void Awake(){
		if (instance != null && instance != this) {
				Destroy (this.gameObject);
				return;
		} else {
				instance = this;
			Debug.Log("the Instance " + instance.GetComponent<AudioSource>().name);
		}
		DontDestroyOnLoad (this.gameObject);
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
