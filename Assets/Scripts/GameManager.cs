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

public class GameManager : MonoBehaviour {

	public float timer = 60f;
	public float timer_current;

	public Text timerText;

	public Canvas endGameCanvas;

	[HorizontalLine(color: EColor.Gray)]
	public bool isQuiz;
	public int currentQuizPage;

	[Serializable]
	public class QuestionAnswer
	{
		public string question;
		public int rightAnswer;
	}

	public List<QuestionAnswer> questionAnswersBank;
	public List<QuestionAnswer> questionAnswers;

	[HorizontalLine(color: EColor.Gray)]
	[ReadOnly]
	public string result;

	[Button(enabledMode: EButtonEnableMode.Playmode)]
	void AnswerYes() 
	{
		int rightAnswer = questionAnswers[currentQuizPage].rightAnswer;

		result = rightAnswer == 0 ? "You're right!" : "You're wrong!";
		NextQuizPage();
	}

	[Button(enabledMode: EButtonEnableMode.Playmode)]
	void AnswerNo() 
	{
		int rightAnswer = questionAnswers[currentQuizPage].rightAnswer;

		result = rightAnswer == 0 ? "You're wrong!" : "You're right!";
		NextQuizPage();
	}

	void Start () {
		timer_current = timer;

		PrepareQuestionAnswers();
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
	
	void PrepareQuestionAnswers()
	{
		while(questionAnswers.Count < 5)
		{
			int r = Random.Range(0, questionAnswersBank.Count);
			questionAnswers.Add(questionAnswersBank[r]);
			questionAnswersBank.RemoveAt(r);
		}
	}

	void NextQuizPage()
	{
		if(currentQuizPage < 5 - 1)
		{
			currentQuizPage++;
		}
	}
}
