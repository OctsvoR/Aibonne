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

	[HideInInspector]
	public int personCountOnScreen;

	int personNumbering;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		StartCoroutine(SpawnPersonRoutine());
	}

	void SpawnPerson()
	{
		Game2_Person person = Instantiate(
			personPrefab,
			new Vector2(-3.8f, -1.595f),
			Quaternion.identity
		);

		person.gameObject.name = "Person " + personNumbering;
		person.standpoint = Random.Range (-3f, 3f);

		personNumbering++;
		personCountOnScreen++;
	}

	IEnumerator SpawnPersonRoutine()
	{
		for(; ; )
		{
			if(personNumbering < 10 && personCountOnScreen < 3)
			{
				SpawnPerson();
			}
			yield return new WaitForSeconds(Random.Range(2, 5));
		}
	}
}
