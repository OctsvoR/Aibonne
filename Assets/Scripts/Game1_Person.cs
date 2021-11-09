using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Linq;

public class Game1_Person : MonoBehaviour
{
	public Warning warning;

	[Space]
	public List<GameObject> sprites;

	bool isBeingDragged;
	bool doApproach = true;
	bool doLeave;

	[HideInInspector]
	public bool isProximity;
	bool canBeDragged;
	bool canBeDragged_last;

	float speed;

	[HideInInspector]
	public float standpoint;

	[Space]
	public float timeToLeave = 5f;
	float timeToLeave_current = 0f;

	void Start()
	{
		Init();
	}

	void Update()
	{
		if(GameManager.Instance.doPause)
			return;

		canBeDragged = !doApproach && !doLeave;

		UpdateDragInput();
		UpdateScreenRestriction();
		UpdateBehaviour();
		UpdateBehaviourTimer();
		UpdateProximityDetection();
		UpdateWarningVisibility();

		canBeDragged_last = canBeDragged;
	}

	void OnMouseDown()
	{
		if(GameManager.Instance.doPause)
			return;

		if(EventSystem.current.IsPointerOverGameObject()) return;

		if(canBeDragged)
			isBeingDragged = true;
	}

	void OnMouseUp()
	{
		if(GameManager.Instance.doPause)
			return;

		isBeingDragged = false;
	}

	void Init()
	{
		speed = Random.Range(0.9f, 1f);

		int skin = Random.Range(1, sprites.Count);
		for(int i = 0; i < sprites.Count; i++)
		{
			sprites[i].SetActive(i == skin);
		}

		timeToLeave_current = timeToLeave;
	}

	void UpdateWarningVisibility()
	{
		warning.gameObject.SetActive(isProximity);
	}

	void UpdateProximityDetection()
	{
		isProximity = false;
		for(int i = 0; i < Game1_PersonManager.Instance.slots.Count; i++)
		{
			var otherPerson = Game1_PersonManager.Instance.slots[i].person;

			if(otherPerson && otherPerson != this)
			{
				var dist = Vector2.Distance(
					transform.position,
					otherPerson.transform.position
				);

				if(dist <= 1f)
				{
					if(
						canBeDragged && otherPerson.canBeDragged &&
						!isBeingDragged && !otherPerson.isBeingDragged
					)
					{
						isProximity = true;
					}
				}
			}
		}
	}

	void UpdateDragInput()
	{
		if(canBeDragged && isBeingDragged)
		{
			Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			transform.position = new Vector3(
				worldMousePosition.x,
				transform.position.y,
				transform.position.z
			);
		}
	}

	void UpdateScreenRestriction()
	{
		if(canBeDragged)
		{
			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, -5.107f, 5.107f),
				transform.position.y,
				transform.position.z
			);
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
					5.605f,
					transform.position.y
				), Time.deltaTime * speed
			);

			if(transform.position.x == 5.605f)
			{
				Destroy(gameObject);
			}

			isBeingDragged = false;
		}

		if(canBeDragged != canBeDragged_last && canBeDragged == true)
		{
			StartCoroutine(AlternateHeadingRoutine());
		}
	}

	void UpdateBehaviourTimer()
	{
		if(isProximity || isBeingDragged)
			timeToLeave_current = timeToLeave;

		if(!doApproach && !isProximity)
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
