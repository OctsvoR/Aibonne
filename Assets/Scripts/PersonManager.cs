using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PersonManager : MonoBehaviour {

	public static PersonManager Instance { private set; get; }

	[Serializable]
	public class Slot {
		public Person person;
		public float standpoint;
	}

	public Person personPrefab;

	public int personAmount = 6;

	public List<Slot> slotList;

	public Canvas warningCanvas;
	public Button warningPrefab;

	public UnityEvent onBecameProximity = new UnityEvent ();

	public class Edge {

		public Person from;
		public Person to;

		public Edge (Person from, Person to) {
			this.from = from;
			this.to = to;
		}
	}

	int personNumbering;

	void Awake () {
		Instance = this;
	}

	void Start () {
		GenerateSlots (9);

		StartCoroutine (ResetProximityFlagRoutine ());
		StartCoroutine (SpawnPersonRoutine ());

		//onBecameProximity.AddListener (() => { print ("x"); });
	}
	
	void Update () {
		CalculateProximities ();
	}

	void SpawnPerson () {
		int filledSlotAmount = 0;

		for (int i = 0; i < slotList.Count; i++) {
			if (slotList[i].person != null) {
				filledSlotAmount++;
			}
		}

		bool slotIsFull = filledSlotAmount == slotList.Count;

		if (!slotIsFull) {
			int floor = Random.Range (0, 2);

			Person person = Instantiate (
				personPrefab,
				new Vector3 (-5.605f, floor == 0 ? -1.595f : 0.845f, 0f),
				Quaternion.identity
			);

			person.gameObject.name = "Person " + personNumbering;

			int r = FindEmptySlotId ();
			slotList[r].person = person;
			person.standpoint = slotList[r].standpoint /*+ Random.Range (-0.2f, 0.2f)*/;

			personNumbering++;
		}
	}

	int FindEmptySlotId () {
		int slotID = Random.Range (0, slotList.Count);

		while (slotList[slotID].person != null) {
			slotID = Random.Range (0, slotList.Count);
		}

		return slotID;
	}

	void GenerateSlots (int amount) {
		for (int i = 0; i < amount; i++) {
			Slot slot = new Slot ();
			slot.standpoint = -amount / 2 + i * 0.8f;
			slotList.Add (slot);
		}
	}

	void CalculateProximities () {
		//for (int i = 0; i < slotList.Count; i++) {
		//	for (int j = i; j < slotList.Count; j++) {
		//		if (slotList[i].person && slotList[j].person) {
		//			if (slotList[i].person != slotList[j].person) {
		//				float dist = Vector3.Distance (
		//					slotList[i].person.transform.position,
		//					slotList[j].person.transform.position
		//				);

		//				if (dist <= 1f) {
		//					if (
		//						slotList[i].person.canBeDragged && slotList[j].person.canBeDragged &&
		//						!slotList[i].person.isBeingDragged && !slotList[j].person.isBeingDragged
		//					) {
		//						Debug.DrawLine (slotList[i].person.transform.position, slotList[j].person.transform.position, Color.yellow);
		//						slotList[i].person.isProximity = true;
		//						slotList[j].person.isProximity = true;
		//					}
		//				}
		//			}
		//		}
		//	}
		//}
	}

	IEnumerator ResetProximityFlagRoutine () {
		for (; ; ) {
			for (int i = 0; i < slotList.Count; i++) {
				if (slotList[i].person) {
					slotList[i].person.isProximity = false;
				}
			}
			yield return new WaitForSeconds (0.1f);
		}
	}

	IEnumerator SpawnPersonRoutine () {
		for (; ; ) {
			SpawnPerson ();
			yield return new WaitForSeconds (Random.Range (2, 5));
		}
	}
}
