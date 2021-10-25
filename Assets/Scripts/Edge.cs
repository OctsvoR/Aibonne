using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Edge {

	public Person from;
	public Person to;

	//public Button warning;

	public Edge (Person from, Person to) {
		this.from = from;
		this.to = to;
	}
}