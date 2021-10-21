using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Edge {

	public Person from;
	public Person to;

	public Edge (Person from, Person to) {
		this.from = from;
		this.to = to;
	}
}