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

public class QuizManager : MonoBehaviour
{
	public Canvas questionAnswerCanvas;
	public Text questionText;

	[Space]
	public Canvas popupCanvas;

	[Space]
	public Canvas endQuizCanvas;
	public Text endTextCanvas;

	int currentQuizPage;

	int score;

	[Serializable]
	public class QuestionAnswer
	{
		public string question;
		public int rightAnswer;
	}

	[Space]
	public List<QuestionAnswer> questionAnswersBank;
	List<QuestionAnswer> questionAnswers = new List<QuestionAnswer>();

	public enum Result
	{
		None,
		Right,
		Wrong
	}

	public void AnswerYes()
	{
		Answer(1);
	}

	public void AnswerNo()
	{
		Answer(0);
	}

	private void Start()
	{
		PrepareQuestionAnswers();
	}

	private void Update()
	{
		UpdateTextUI();
	}

	void ShowPopup(string message)
	{
		popupCanvas.gameObject.SetActive(true);
	}

	void HidePopup()
	{
		popupCanvas.gameObject.SetActive(false);
	}

	void UpdateTextUI()
	{
		if(currentQuizPage < 5)
			questionText.text = "" + questionAnswers[currentQuizPage].question;
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

	bool CheckAnswer(int answer)
	{
		return questionAnswers[currentQuizPage].rightAnswer == answer;
	}

	Result Answer(int answer)
	{
		Result result = Result.None;

		if(currentQuizPage < 5)
		{
			result = CheckAnswer(answer) ? Result.Right : Result.Wrong;

			if(result == Result.Right)
				score++;

			StartCoroutine(NextQuizPageRoutine(result));
		}

		return result;
	}

	string GetResultMessage (Result result)
	{
		string message = string.Empty;

		switch(result)
		{
			case Result.None:
				break;
			case Result.Right:
				message = "Benar!";
				break;
			case Result.Wrong:
				message = "Salah!";
				break;
		}

		return message;
	}

	IEnumerator NextQuizPageRoutine(Result result)
	{
		if(result == Result.Wrong)
		{
			ShowPopup(GetResultMessage(result));

			yield return new WaitForSeconds(1f);
		}

		currentQuizPage++;
		if(currentQuizPage >= 5)
		{
			EndQuiz();
		}

		if(result == Result.Wrong)
			HidePopup();
	}

	void EndQuiz()
	{
		questionAnswerCanvas.gameObject.SetActive(false);
		endQuizCanvas.gameObject.SetActive(true);
		endTextCanvas.text = $"<size=56>kuis selesai</size>\nbenar {score} / 5";
	}
}