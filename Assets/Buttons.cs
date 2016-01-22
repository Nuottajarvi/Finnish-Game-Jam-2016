using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

	ClientNetworker networker;

	void Start () {
		networker = GameObject.Find("Networker").GetComponent<ClientNetworker>();
	}

	public void ToLeft () {
		networker.MoveOut(-5, 0);
	}

	public void ToRight(){
		networker.MoveOut(5, 0);
	}
}
