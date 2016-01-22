using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class ServerNetworker : MonoBehaviour {

	private string id;

	UDPReceive udpReceive;
	UDPSend udpSend;

	void Start(){

		Application.runInBackground = true;
		udpReceive = GetComponent<UDPReceive>();
		udpSend = GetComponent<UDPSend>();
	}

	void Update(){
		string[] UDPPackets = udpReceive.GetLatestUDPPackets();

		foreach(string UDPPacket in UDPPackets){
			var values = JSON.Parse(UDPPacket);

			switch(values["function"].Value){
				case "move": MoveIn(values); break;
			}
		}
	}

	private void MoveIn(JSONNode data){
		GameObject cube = GameObject.Find("Cube");
		cube.transform.position += new Vector3(data["x"].AsFloat, data["y"].AsFloat, 0);
	}
}
