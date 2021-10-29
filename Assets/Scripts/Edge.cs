using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Edge : MonoBehaviour{

	public Person from;
	public Person to;

	public Warning warning;

	public Edge (Person from, Person to) {
		this.from = from;
		this.to = to;
	}
}