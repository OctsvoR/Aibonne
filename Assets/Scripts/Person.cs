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

// todo:
// 1. bikin orang jalan pergi
// 2. bikin timer game

public class Person : MonoBehaviour {

	public bool isBeingDragged;
	public bool doApproach;
	public bool doLeave;
	public bool isProximity;
	public bool isProximity_last;
	public bool canBeDragged;
	public bool canBeDragged_last;

	float speed;
	public float standpoint;

	public float timeToLeave = 5f;
	public float timeToLeave_current = 0f;

	public List<GameObject> sprites;

	public List<Person> proximities = new List<Person> ();
	public List<Person> proximities_last = new List<Person> ();

	void Start () {
		speed = Random.Range (0.9f, 1f);

		int skin = Random.Range (1, 1 + sprites.Count);
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
		//////////////
		// Dragging //
		//////////////
		canBeDragged = !doApproach && !doLeave;

		if (canBeDragged && isBeingDragged) {
			Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			transform.position = new Vector3 (
				worldMousePosition.x,
				transform.position.y,
				transform.position.z
			);
		}

		////////////////////////
		// Screen Restriction //
		////////////////////////
		if (canBeDragged) {
			transform.position = new Vector3 (
				Mathf.Clamp (transform.position.x, -5.107f, 5.107f),
				transform.position.y,
				transform.position.z
			);
		}

		//////////////////////////////////////
		// Approaching Standpoint Behaviour //
		//////////////////////////////////////
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

		//////////////////////////////////
		// Leaving Standpoint Behaviour //
		//////////////////////////////////
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

		///////////
		// Timer //
		///////////
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

		////////////
		// Events //
		////////////
		
		if (isProximity != isProximity_last) {
			if (isProximity)
				PersonManager.Instance.onBecameProximity.Invoke ();
		}

		////////////
		// Others //
		////////////

		//proximities.Clear ();

		for (int i = 0; i < PersonManager.Instance.slotList.Count; i++) {
			Person otherPerson = PersonManager.Instance.slotList[i].person;

			if (otherPerson && otherPerson != this) {
				float dist = Vector2.Distance (
					otherPerson.transform.position,
					transform.position
				);

				if (dist <= 1f) {
					if (canBeDragged && otherPerson.canBeDragged &&
						!isBeingDragged && !otherPerson.isBeingDragged
					) {
						proximities.Add (PersonManager.Instance.slotList[i].person);
						isProximity = true;
					}
				} else {
					proximities.Remove (PersonManager.Instance.slotList[i].person);
				}
			}
		}

		isProximity_last = isProximity;
		canBeDragged_last = canBeDragged;
		proximities_last = new List<Person> (proximities);
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
