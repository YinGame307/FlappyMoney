using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GameControl : MonoBehaviour 
{
	public static GameControl instance;			//A reference to our game control script so we can access it statically.
	public Text scoreText;						//A reference to the UI text component that displays the player's score.
	public GameObject gameOvertext;				//A reference to the object that displays the text which appears when the player dies.

	private int score = 0;						//The player's score.
	public bool gameOver = false;				//Is the game over?
	public float scrollSpeed = -2f;

	public bool isStarted;
	public GameObject menu;
	public GameObject[] heros;
	int activeHero;
	void Awake()
	{
		PlayerPrefs.SetInt ("SHOWADS", 0);
		try{
			heros[Player.user.userLevel - 1].SetActive(true);
			activeHero = Player.user.userLevel - 1;
		}catch(System.Exception ex){
			heros [0].SetActive (true);
			activeHero = 0;
		}
		isStarted = false;
		//If we don't currently have a game control...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
	}

	void Update()
	{
		if (isStarted) {
//			//If the game is over and the player has pressed some input...
//			if (gameOver && Input.GetMouseButtonDown (0) && !IsPointerOverUIObject) {
//				//...reload the current scene.
//				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
//			}
		}
	}

	public void BirdScored()
	{
		//The bird can't score if the game is over.
		if (gameOver)	
			return;
		//If the game is not over, increase the score...
		score++;
		//...and adjust the score text.
		scoreText.text = "Score: " + score.ToString();
	}
	public Text scoreLabel;
	public Text hiScoreLabel;

	public void BirdDied()
	{
		scoreLabel.text = "" + score.ToString();
		if (score > PlayerPrefs.GetInt ("HISCORE")) {
			PlayerPrefs.SetInt ("HISCORE", score);
		}
		hiScoreLabel.text = "Hight score: " + PlayerPrefs.GetInt ("HISCORE").ToString ();
		//Activate the game over text.
		gameOvertext.SetActive (true);
		//Set the game to be over.
		if (!gameOver) {			
			gameOver = true;
		}

	}

	public void startGame(){
		isStarted = true;
		menu.SetActive (false);
		heros [activeHero].GetComponent<Animator> ().enabled = true;
	}
	public void pause(){
		Time.timeScale = 0;
	}

	public void unPause(){
		Time.timeScale = 1;
	}

	public void backToHome(){		
		SceneManager.LoadScene (0);
	}

	public void restart(){
		PlayerPrefs.SetInt ("SHOWADS", 1);
		SceneManager.LoadScene (1);
	}
}
