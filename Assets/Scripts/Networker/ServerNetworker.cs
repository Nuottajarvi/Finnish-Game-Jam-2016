using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class ServerNetworker : MonoBehaviour {

	private string id;

	UDPReceive udpReceive;
	UDPSend udpSend;

	List<string> connectedPlayers;

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
	private void SendWordOut(List<string> word, List<WordActionGenerator.WordAction> wordAction) {
		JSONNode data = new JSONClass();

		//Set the function
		data["function"] = "SendWord";

		//Empty container strings
		string wordString = "";
		string actionString = "";

		//Word and wordaction lists are equal length
		for (int i = 0; i < word.Count; i++) {
			//strings are values separated with ;
			wordString = wordString + word[i] + ";";
			actionString = actionString + wordAction[i] + ";";
		}

		//Remove last ; from the end of string
		wordString = wordString.Remove(wordString.Length - 1);
		actionString = actionString.Remove(actionString.Length - 1);

		data["word"] = wordString;
		data["wordAction"] = actionString;

		udpSend.Send(data);
	}
}
