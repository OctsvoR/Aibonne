using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public Canvas endGameCanvas;

	[HideInInspector]
	public Text timerText;

	public enum GameMode
	{
		Distance,
		Mask
	}

	//[Space]
	//public GameMode gameMode;

	[Space]
	public float timer = 60f;
	float timer_current;

	void Awake()
	{
		Instance = this;
	}

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
