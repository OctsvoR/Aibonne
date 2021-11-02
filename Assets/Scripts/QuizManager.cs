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

public class QuizManager : MonoBehaviour {

	public Text questionText;

	public Popup popup;

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
	public string message;

	public enum Result
	{
		None,
		Right,
		Wrong,
		End
	}

	[Button(enabledMode: EButtonEnableMode.Playmode)]
	public void AnswerYes()
	{
		message = Answer(1).ToString();
	}

	[Button(enabledMode: EButtonEnableMode.Playmode)]
	public void AnswerNo()
	{
		message = Answer(0).ToString();
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
		popup.gameObject.SetActive(true);
		popup.text.text = message;
	}

	void HidePopup()
	{
		popup.gameObject.SetActive(false);
		popup.text.text = string.Empty;
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
			//next page
		}
		else
		{
			result = Result.End;
		}

		return result;
	}

	IEnumerator NextQuizPage()
	{
		ShowPopup("jawaban");
		yield return new WaitForSeconds(3f);
		currentQuizPage++;
	}
}