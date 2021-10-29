using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Warning : MonoBehaviour {

	public Edge edge;
	
	void Start () {
	}
	
	void Update () {
		transform.position = Camera.main.WorldToScreenPoint (
			new Vector2 (
				edge.from.transform.position.x + (edge.to.transform.position.x - edge.from.transform.position.x) * .5f,
				edge.from.transform.position.y + 0.6f
			)
		);
		
	}
}
