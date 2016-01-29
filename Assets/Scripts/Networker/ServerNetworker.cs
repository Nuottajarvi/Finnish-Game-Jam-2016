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

    private void SetWordActionIn(JSONNode data)
    {


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
