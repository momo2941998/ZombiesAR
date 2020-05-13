using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadNewGame : MonoBehaviour {


	public Button newGame;

	// Use this for initialization
	void Start () {
		newGame.onClick.AddListener (LoadGame);
		
	}
	
	void LoadGame()
	{
		SceneManager.LoadScene ("My scene");
	}
}
