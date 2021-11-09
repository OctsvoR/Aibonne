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

public class Game1_PersonManager : MonoBehaviour {

	public static Game1_PersonManager Instance { private set; get; }

	public Game1_Person personPrefab;

	[Space]
	public int numberOfSlots = 9;

	[Serializable]
	public class Slot
	{
		public Game1_Person person;
		public float standpoint;
	}

	public List<Slot> slots;

	int personNumbering;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		GenerateSlots(9);

		StartCoroutine(SpawnPersonRoutine());
	}

	private void Update()
	{
		if(GameManager.Instance.doPause)
			return;

		GameManager.Instance.doUpdateTimer = !IsProximityDetected();
	}

	bool IsProximityDetected()
	{
		bool result = false;

		for(int i = 0; i < slots.Count; i++)
		{
			if(slots[i].person && slots[i].person.isProximity)
			{
				result = true;
				break;
			}
		}

		return result;
	}

	void SpawnPerson()
	{
		if(GameManager.Instance.doPause)
			return;

		int filledSlotAmount = 0;

		for(int i = 0; i < slots.Count; i++)
		{
			if(slots[i].person != null)
			{
				filledSlotAmount++;
			}
		}

		bool slotIsFull = filledSlotAmount == slots.Count;

		if(!slotIsFull)
		{
			int floor = Random.Range(0, 2);

			Game1_Person person = Instantiate(
				personPrefab,
				new Vector3(-5.605f, floor == 0 ? -1.595f : 0.845f, 0f),
				Quaternion.identity
			);

			person.gameObject.name = "Person " + personNumbering;

			int r = FindEmptySlotId();
			slots[r].person = person;
			person.standpoint = slots[r].standpoint /*+ Random.Range(-0.2f, 0.2f)*/;

			personNumbering++;
		}
	}

	int FindEmptySlotId()
	{
		int slotID = Random.Range(0, slots.Count);

		while(slots[slotID].person != null)
		{
			slotID = Random.Range(0, slots.Count);
		}

		return slotID;
	}

	void GenerateSlots(int amount)
	{
		for(int i = 0; i < amount; i++)
		{
			Slot slot = new Slot();
			slot.standpoint = -amount / 2 + i * 0.8f;
			slots.Add(slot);
		}
	}

	IEnumerator SpawnPersonRoutine()
	{
		for(; ; )
		{
			SpawnPerson();
			yield return new WaitForSeconds(Random.Range(2, 5));
		}
	}
}
