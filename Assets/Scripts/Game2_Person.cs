using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Game2_Person : MonoBehaviour {

	public Warning warning;

	[Space]
	public List<GameObject> sprites;
	public List<GameObject> unmaskedSprites;

	public bool doApproach = true;
	bool doLeave;
	bool canBeDragged;
	bool canBeDragged_last;

	bool hasMask;
	bool alreadyMasked;

	float speed;

	[HideInInspector]
	public float standpoint;

	int activeSkin = 0;

	[Space]
	public float timeToLeave = 5f;
	float timeToLeave_current = 0f;

	void Start () {
		Init();
	}
	
	void Update ()
	{
		if(GameManager.Instance.doPause)
			return;

		canBeDragged = !doApproach && !doLeave;

		if(!alreadyMasked)
			warning.gameObject.SetActive(canBeDragged);

		UpdateBehaviour();
		UpdateBehaviourTimer();

		canBeDragged_last = canBeDragged;
	}

	private void OnMouseDown()
	{
		if(GameManager.Instance.doPause)
			return;

		if(alreadyMasked)
			return;

		if(EventSystem.current.IsPointerOverGameObject()) return;

		if(!hasMask) GiveMask();
	}

	void Init()
	{
		speed = Random.Range(0.9f, 1f);

		activeSkin = Random.Range(0, sprites.Count);

		int maskedRandom = Random.Range(0, 2);
		alreadyMasked = maskedRandom == 1;

		switch(maskedRandom)
		{
			case 0:
				sprites[activeSkin].SetActive(false);
				unmaskedSprites[activeSkin].SetActive(true);
				break;
			case 1:
				sprites[activeSkin].SetActive(true);
				unmaskedSprites[activeSkin].SetActive(false);
				break;
		}

		timeToLeave_current = timeToLeave;
	}

	void GiveMask()
	{
		if(canBeDragged)
		{
			unmaskedSprites[activeSkin].SetActive(false);
			sprites[activeSkin].SetActive(true);

			hasMask = true;
			doLeave = true;

			Game2_PersonManager.Instance.maskedPersonCount++;

			if(Game2_PersonManager.Instance.maskedPersonCount >= 10)
				GameManager.Instance.EndGame();
		}
	}

	void UpdateBehaviour()
	{
		if(doApproach)
		{
			transform.position = Vector2.MoveTowards(
				transform.position,
				new Vector2(
					standpoint,
					transform.position.y
				), Time.deltaTime * speed
			);

			if(Mathf.Abs(transform.position.x - standpoint) == 0f)
				doApproach = false;
		}

		if(doLeave)
		{
			transform.localScale = new Vector3(
				1f,
				transform.localScale.y,
				transform.localScale.z
			);

			transform.position = Vector2.MoveTowards(
				transform.position,
				new Vector2(
					3.8f,
					transform.position.y
				), Time.deltaTime * speed
			);

			if(transform.position.x == 3.8f)
			{
				Destroy(gameObject);
			}
		}

		if(canBeDragged != canBeDragged_last && canBeDragged == true)
		{
			StartCoroutine(AlternateHeadingRoutine());
		}

		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
	}

	void UpdateBehaviourTimer()
	{
		if(alreadyMasked && !doApproach)
		{
			timeToLeave_current -= Time.deltaTime;

			if(timeToLeave_current <= 0f)
			{
				doLeave = true;
				timeToLeave_current = 0f;
			}
		}
	}

	IEnumerator AlternateHeadingRoutine()
	{
		while(canBeDragged)
		{
			yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
			transform.localScale = new Vector3(
				-transform.localScale.x,
				transform.localScale.y,
				transform.localScale.z
			);
		}

		transform.localScale = new Vector3(
			1f,
			transform.localScale.y,
			transform.localScale.z
		);
	}
}
