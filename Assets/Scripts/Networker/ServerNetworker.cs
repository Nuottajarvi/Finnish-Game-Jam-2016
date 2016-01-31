using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.SceneManagement;
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

	public List<string> connectedPlayers;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(this.gameObject);
	}

	void Start(){

		DontDestroyOnLoad(this.gameObject);

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
		// Only allow new players to connect in lobby
		if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "lobby") return;

		if(!connectedPlayers.Contains(data["id"]) && Application.loadedLevel != 0){
			connectedPlayers.Add(data["id"]);
			LobbyController lc = GameObject.Find("PlayerText").GetComponent<LobbyController>();
			lc.AddPlayer(data["id"]);
		}

		ConfirmConnectionOut(data["id"]);
	}

	private void ConfirmConnectionOut(string id){
		JSONNode data = new JSONClass();
		data["function"] = "Confirm";
		data["id"] = id;

		udpSend.Send(data);
	}

	//Function to read incoming action and see if it corresponds to currently active word in the sentence
	private void SetWordIn(JSONNode data){
		JSONArray wordArray = data["words"].AsArray;

		RitualHandler.Instance.OnNewAction(wordArray);
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

		StartCoroutine(sendWordsOverAndOverAgain(data));
	}

	IEnumerator sendWordsOverAndOverAgain(JSONData data){

		int i = 0;

		while(i < 5){
			udpSend.Send(data);
			yield return new WaitForSeconds(0.5f);
			i++;
		}
	}

	public void GameLostOut() {
		JSONNode data = new JSONClass();

		data["function"] = "GameStatus";
		data["status"] = "lost";

		udpSend.Send(data);
	}
}
