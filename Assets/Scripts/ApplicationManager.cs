using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ApplicationManager : MonoBehaviour {

	public static ApplicationManager Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	void Start ()
	{
		Time.timeScale = 1;
	}
	
	void Update () {
		
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void LoadScene(int index)
	{
		SceneManager.LoadScene(index);
	}
}
