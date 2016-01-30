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

			switch(values["function"].Value){
                //case "broadcastServer": SetServer(values); break;
                case "SendWord": SetWordIn(values); break;
            }
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

    //Send completed action
    public void WordOut(WordActionGenerator.WordAction action) {
        JSONNode data = new JSONClass();

        //Get script containing words and actions
        UI_phonescreen_script uiPhoneScreenScript = GameObject.Find("UI_phonescreen").GetComponent<UI_phonescreen_script>();
        JSONArray arrayToSend = new JSONArray();

        //Get words that match performed action
        for (int i = 0; i < uiPhoneScreenScript.words.Length; i++) {
            if (uiPhoneScreenScript.wordActions[i] == action) {
				JSONNode wordActionPair = new JSONClass();
				wordActionPair["word"] = uiPhoneScreenScript.words[i];
				wordActionPair["action"] = action.ToString();

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

        //Interpret the received results
        string[] wordArray = data["word"].Value.Split(';');
        string[] actionArray = data["wordAction"].Value.Split(';');

        //get uiphonesscreen script where the word and actions are stored
        UI_phonescreen_script uiPhoneScreenScript = GameObject.Find("UI_phonescreen").GetComponent<UI_phonescreen_script>();
        uiPhoneScreenScript.words = wordArray;

        WordActionGenerator.WordAction[] mappedActionArray = new WordActionGenerator.WordAction[actionArray.Length];
        //Map received values to Enums
        for (int i = 0; i < actionArray.Length; i++) {
            mappedActionArray[i] = (WordActionGenerator.WordAction)Enum.Parse(typeof(WordActionGenerator.WordAction) , actionArray[i]);
        }
        uiPhoneScreenScript.wordActions = mappedActionArray;

    }

}
