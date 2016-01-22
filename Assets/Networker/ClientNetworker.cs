using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class ClientNetworker : MonoBehaviour {

	private string id;

	UDPReceive udpReceive;
	UDPSend udpSend;

	void Start(){
		System.Random random = new System.Random();
		id = "" + random.Next(9999);
		Application.runInBackground = true;
		udpReceive = GetComponent<UDPReceive>();
		udpSend = GetComponent<UDPSend>();
	}

	void Update(){
		//TODO: Mitä jos tulee useampi?
		string[] UDPPackets = udpReceive.GetLatestUDPPackets();

		foreach(string UDPPacket in UDPPackets){
			var values = JSON.Parse(UDPPacket);

			/*switch(values["function"].Value){
				case "broadcastServer": SetServer(values); break;
			}*/
		}
	}

	public void MoveOut(float x, float y){
		JSONNode data = new JSONClass();

		data["function"] = "move";
		data["id"] = id;
		data["x"].AsFloat = x;
		data["y"].AsFloat = y;
	
		udpSend.Send(data);
	}
}
