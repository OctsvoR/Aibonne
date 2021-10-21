using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public float timer = 60f;
	public float timer_current;

	public Text timerText;

	public Canvas endGameCanvas;

	void Start () {
		timer_current = timer;
	}
	
	void Update () {
		UpdateGameTimer ();
		UpdateUI ();
	}

	void EndGame () {
		Time.timeScale = 0f;
		endGameCanvas.gameObject.SetActive (true);
	}

	void UpdateGameTimer () {
		timer_current -= Time.deltaTime;

		if (timer_current <= 0f) {
			EndGame ();

			timer_current = 0f;
		}
	}

	void UpdateUI () {
		timerText.text = (int)timer_current + "s";
	}
}
