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

	public bool doApproach = true;
	bool doLeave;
	bool canBeDragged;
	bool canBeDragged_last;

	bool hasMask;

	float speed;

	[HideInInspector]
	public float standpoint;

	void Start () {
		Init();
	}
	
	void Update ()
	{
		canBeDragged = !doApproach && !doLeave;

		UpdateBehaviour();

		canBeDragged_last = canBeDragged;
	}

	private void OnMouseDown()
	{
		if(!hasMask) GiveMask();
	}

	private void OnMouseUp()
	{

	}

	void Init()
	{
		speed = Random.Range(0.9f, 1f);
	}

	void GiveMask()
	{
		if(canBeDragged)
		{
			hasMask = true;
			doLeave = true;
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
		}

		if(canBeDragged != canBeDragged_last && canBeDragged == true)
		{
			StartCoroutine(AlternateHeadingRoutine());
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
