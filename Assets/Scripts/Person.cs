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

public class Person : MonoBehaviour {

	public bool isBeingDragged;
	public bool doApproach;
	public bool doLeave;
	public bool isProximity;
	public bool canBeDragged;
	public bool canBeDragged_last;

	float speed;
	public float standpoint;

	public float timeToLeave = 5f;
	public float timeToLeave_current = 0f;

	public List<GameObject> sprites;

	public Warning warning;

	void Start () {
		speed = Random.Range (0.9f, 1f);

		int skin = Random.Range (1, sprites.Count);
		for (int i = 0; i < sprites.Count; i++) {
			sprites[i].SetActive (i == skin);
		}

		timeToLeave_current = timeToLeave;
	}

	void OnMouseDown () {
		if (canBeDragged)
			isBeingDragged = true;
	}

	void OnMouseUp () {
		isBeingDragged = false;
	}

	void Update () {
		canBeDragged = !doApproach && !doLeave;

		UpdateDragInput ();
		UpdateScreenRestriction ();
		UpdateBehaviour ();
		UpdateBehaviourTimer ();

		isProximity = false;
		for(int i = 0; i < PersonManager.Instance.slots.Count; i++)
		{
			var otherPerson = PersonManager.Instance.slots[i].person;
				
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

		warning.gameObject.SetActive(isProximity);

		canBeDragged_last = canBeDragged;
	}

	void UpdateDragInput () {
		if (canBeDragged && isBeingDragged) {
			Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			transform.position = new Vector3 (
				worldMousePosition.x,
				transform.position.y,
				transform.position.z
			);
		}
	}

	void UpdateScreenRestriction () {
		if (canBeDragged) {
			transform.position = new Vector3 (
				Mathf.Clamp (transform.position.x, -5.107f, 5.107f),
				transform.position.y,
				transform.position.z
			);
		}
	}

	void UpdateBehaviour () {
		if (doApproach) {
			transform.position = Vector2.MoveTowards (
				transform.position,
				new Vector2 (
					standpoint,
					transform.position.y
				), Time.deltaTime * speed
			);

			if (Mathf.Abs (transform.position.x - standpoint) == 0f)
				doApproach = false;
		}

		if (doLeave) {
			transform.position = Vector2.MoveTowards (
				transform.position,
				new Vector2 (
					5.605f,
					transform.position.y
				), Time.deltaTime * speed
			);

			if (transform.position.x == 5.605f) {
				Destroy (gameObject);
			}

			isBeingDragged = false;
		}
	}

	void UpdateBehaviourTimer () {
		if (isProximity || isBeingDragged) 
			timeToLeave_current = timeToLeave;

		if (!doApproach && !isProximity) {
			timeToLeave_current -= Time.deltaTime;

			if (timeToLeave_current <= 0f) {
				doLeave = true;
				timeToLeave_current = 0f;
			}
		}

		if (canBeDragged != canBeDragged_last && canBeDragged == true) {
			StartCoroutine (AlternateHeadingRoutine ());
		}
	}

	IEnumerator AlternateHeadingRoutine () {
		while (canBeDragged) {
			yield return new WaitForSeconds (Random.Range (0.5f, 1.5f));
			transform.localScale = new Vector3 (
				-transform.localScale.x, 
				transform.localScale.y, 
				transform.localScale.z
			);
		}

		transform.localScale = new Vector3 (
			1f,
			transform.localScale.y,
			transform.localScale.z
		);
	}
}
