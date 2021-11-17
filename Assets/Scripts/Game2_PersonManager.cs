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

public class Game2_PersonManager : MonoBehaviour {

	public static Game2_PersonManager Instance { private set; get; }

	public Game2_Person personPrefab;

	[Serializable]
	public class Slot
	{
		public Game2_Person person;
		public float standpoint;
	}

	public List<Slot> slots;

	int personNumbering;

	[HideInInspector]
	public int maskedPersonCount;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		GenerateSlots(5);
		StartCoroutine(SpawnPersonRoutine());
	}

	void GenerateSlots(int amount)
	{
		for(int i = 0; i < amount; i++)
		{
			Slot slot = new Slot();
			slot.standpoint = -amount / 2 + i * 1.2f;
			slots.Add(slot);
		}
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
			Game2_Person person = Instantiate(
				personPrefab,
				new Vector3(-3.8f, -1.595f + Random.Range(-0.5f, 0.2f)),
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

	IEnumerator SpawnPersonRoutine()
	{
		for(; ; )
		{
			if(maskedPersonCount < 10)
			{
				SpawnPerson();
			}
			yield return new WaitForSeconds(Random.Range(2, 5));
		}
	}
}
