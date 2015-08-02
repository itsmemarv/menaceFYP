using UnityEngine;
using System.Collections;


enum E_TEXTURE{
	E_HOWTOPLAY = 0,
	E_ACQUIRE,
	E_DEPLOY,
	E_REINFORCE,
	E_ATTACK,
	E_FORTIFY,
	E_TOTAL
}

public class TutorialScript : MonoBehaviour {

	public int currentTexture;
	E_TEXTURE theTexture;
	SpriteRenderer _spriteRenderer;

	public Sprite HowToPlay;
	public Sprite AcquireTexture;
	public Sprite DeployTexture;
	public Sprite ReinforceTexture;
	public Sprite AttackTexture;
	public Sprite FortifyTexture;

	// Use this for initialization
	void Start () {
		currentTexture = 0;
		_spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		_spriteRenderer.sprite = HowToPlay;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RightButtonChange(){
		currentTexture++;
		if (currentTexture > 5) {
			currentTexture = 0;
		}
		ChangeTexture (currentTexture);


	}

	public void LeftButtonChange(){
		currentTexture--;
		if (currentTexture < 0) {
			currentTexture = 5;
		}
		ChangeTexture (currentTexture);
	}


	void ChangeTexture(int myTexture){
		switch (myTexture) {
		case 0:
			_spriteRenderer.sprite =  HowToPlay;
			break;

		case 1:
			_spriteRenderer.sprite =  AcquireTexture;
			break;

		case 2:
			_spriteRenderer.sprite =  DeployTexture;
			break;

		case 3:
			_spriteRenderer.sprite =  ReinforceTexture;
			break;

		case 4:
			_spriteRenderer.sprite =  AttackTexture;
			break;

		case 5:
			_spriteRenderer.sprite =  FortifyTexture;
			break;

		}


	}
}
