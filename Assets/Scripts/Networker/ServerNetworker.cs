﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class ServerNetworker : MonoBehaviour {
	static ServerNetworker instance;
	public static ServerNetworker Instance {
		get {
			return instance;
		}
	}

	private string id;

	UDPReceive udpReceive;
	UDPSend udpSend;

	List<string> connectedPlayers;

	void Awake() {
		if(instance == null)
			instance = this;
	}

	void Start(){

		connectedPlayers = new List<string>();
		Application.runInBackground = true;
		udpReceive = GetComponent<UDPReceive>();
		udpSend = GetComponent<UDPSend>();
	}

	void Update(){
		string[] UDPPackets = udpReceive.GetLatestUDPPackets();

		foreach(string UDPPacket in UDPPackets){
			var values = JSON.Parse(UDPPacket);

			switch(values["function"].Value){
				case "Connect": PlayerIn(values); break;
				case "move": MoveIn(values); break;
				case "wordOut": SetWordIn(values); break;
			}
		}
	}
		
	private void PlayerIn(JSONNode data){
		if(!connectedPlayers.Contains(data["id"])){
			connectedPlayers.Add(data["id"]);
			LobbyController lc = GameObject.Find("LobbyPanel").GetComponent<LobbyController>();
			lc.AddPlayer(data["id"]);
		}

		ConfirmConnectionOut();
	}

	private void ConfirmConnectionOut(){
		JSONNode data = new JSONClass();
		data["function"] = "Confirm";

		udpSend.Send(data);
	}

	//Function to read incoming action and see if it corresponds to currently active word in the sentence
	private void SetWordIn(JSONNode data){
		JSONArray wordArray = data["words"].AsArray;

		//TODO: Send to ritualhandler
	}

	private void MoveIn(JSONNode data){
		GameObject cube = GameObject.Find("Cube");
		cube.transform.position += new Vector3(data["x"].AsFloat, data["y"].AsFloat, 0);
	}

	//Function to send list of words and actions to client
	public void SendWordOut(List<Word> words, string clientId) {
		JSONNode data = new JSONClass();

		//Set the function
		data["function"] = "SendWord";

		data["words"] = new JSONArray();

		for(int i = 0; i < words.Count; i++) {
			data["words"][i] = new JSONClass();

			data["words"][i]["word"] = words[i].word;
			data["words"][i]["action"].AsInt = (int)words[i].action;
		}

		data["id"] = words[0].clientId;

		udpSend.Send(data);
	}
}
