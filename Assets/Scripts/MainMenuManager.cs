using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MainMenuManager : MonoBehaviour {

	public Canvas mainMenuCanvas;
	public Canvas infoTutorCanvas;

	public Button prevButton;
	public Button nextButton;

	public GameObject[] infoTutorImages;

	public int currentTutorPage;

	public void OpenInfo()
	{
		infoTutorCanvas.gameObject.SetActive(true);
	}

	public void CloseInfo()
	{
		infoTutorCanvas.gameObject.SetActive(false);
	}

	public void StartGame()
	{
		ApplicationManager.Instance.LoadScene(1);
	}

	public void QuitGame()
	{
		ApplicationManager.Instance.Quit();
	}

	public void NextTutorPage()
	{
		if(currentTutorPage < infoTutorImages.Length - 1)
			currentTutorPage++;
		else
			CloseInfo();

		RefreshTutorPage();
	}

	public void PrevTutorPage()
	{
		if(currentTutorPage > 0)
			currentTutorPage--;
		else
			CloseInfo();

		RefreshTutorPage();
	}

	void RefreshTutorPage()
	{
		if(currentTutorPage < 0 && currentTutorPage >= infoTutorImages.Length - 1)
			return;

		for(int i = 0; i < infoTutorImages.Length; i++)
		{
			infoTutorImages[i].SetActive(currentTutorPage == i);
		}
	}
}
