using UnityEngine;
using System.Collections;

public class Tilt : MonoBehaviour {

	ClientNetworker networker;

	void Start () {
		networker = GameObject.Find("Networker").GetComponent<ClientNetworker>();
	}

	void Update () {
		float x = Input.acceleration.x;
		float y = Input.acceleration.y;
		networker.MoveOut(x, y);
	}
}
