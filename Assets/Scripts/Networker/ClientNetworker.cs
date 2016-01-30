using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;


public class ClientNetworker : MonoBehaviour {

	private string id;
	public bool connected = false;

	float time;

	UDPReceive udpReceive;
	UDPSend udpSend;

	void Start(){
		System.Random random = new System.Random();
		id = random.Next(1000000).ToString();
		Application.runInBackground = true;
		udpReceive = GetComponent<UDPReceive>();
		udpSend = GetComponent<UDPSend>();
	}

	void Update(){
		//TODO: Mitä jos tulee useampi?
		string[] UDPPackets = udpReceive.GetLatestUDPPackets();

		foreach(string UDPPacket in UDPPackets){
			var values = JSON.Parse(UDPPacket);

			switch(values["function"].Value){
				case "Confirm": ConfirmIn(values); break;
		        	case "SendWord": SetWordIn(values); break;
		        }
		}

		time += Time.deltaTime;
		if(time > 1f) {
			if(!connected) {
				ConnectToServerOut();
			}
			time = 0;
		}
	}

	public void ConnectToServerOut(){
		JSONNode data = new JSONClass();

		data["function"] = "Connect";
		data["id"] = id;

		udpSend.Send(data);
	}

	public void ConfirmIn(JSONNode data){
		connected = true;
	}

	public void MoveOut(float x, float y){
		JSONNode data = new JSONClass();

		data["function"] = "move";
		data["id"] = id;
		data["x"].AsFloat = x;
		data["y"].AsFloat = y;
	
		udpSend.Send(data);
	}

    //Send completed action
    public void WordOut(WordActionGenerator.WordAction action) {
        JSONNode data = new JSONClass();

        //Get script containing words and actions
        UI_phonescreen_script uiPhoneScreenScript = GameObject.Find("UI_phonescreen").GetComponent<UI_phonescreen_script>();
        JSONArray arrayToSend = new JSONArray();

        //Get words that match performed action
        for (int i = 0; i < uiPhoneScreenScript.words.Count; i++) {
            if (uiPhoneScreenScript.words[i].action == action) {
				JSONNode wordActionPair = new JSONClass();
				wordActionPair["word"] = uiPhoneScreenScript.words[i].word;
				wordActionPair["action"].AsInt = (int)uiPhoneScreenScript.words[i].action;

				arrayToSend.Add(wordActionPair);
            }
        }

		data["id"] = id;
		data["function"] = "wordOut";
        data["words"] = arrayToSend;

        udpSend.Send(data);

    }

    //Set current words
    public void SetWordIn(JSONNode data) {
		List<Word> wordsIn = new List<Word>();
		
		JSONArray wordArray = data["words"].AsArray;

		for(int i = 0; i < wordArray.Count; i++) {
			Word word = new Word();

			word.action = (WordActionGenerator.WordAction)wordArray[i]["action"].AsInt;
			word.word = wordArray[i]["word"];

			wordsIn.Add(word);
		}

		UI_phonescreen_script uiPhoneScreenScript = GameObject.Find("UI_phonescreen").GetComponent<UI_phonescreen_script>();
		uiPhoneScreenScript.words = wordsIn;

    }

}
