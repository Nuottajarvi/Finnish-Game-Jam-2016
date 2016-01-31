using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_phonescreen_script : MonoBehaviour {

    public Text wordList;
	public Text idText;
	//public string[] words;
	//public WordActionGenerator.WordAction[] wordActions; 
	public List<Word> words;
	public string id;

	[SerializeField]
	Text logText;

	string logString;

	void Start() {
		id = "Not registered yet";
	}

	// Update is called once per frame
	void Update () {
        //Empty text
        wordList.text = "";

		if(words != null && words.Count > 0) {
			//Set text with all the contents from the lists
			for(int i = 0; i < words.Count; i++) {
				wordList.text = wordList.text + "\n<b>" + words[i].word + "</b>\n" + words[i].action + "\n";
			}
		}


		idText.text = "ID: " + id;

	}

	void OnEnable() {
		Application.logMessageReceived += OnLog;
	}

	void OnLog(string condition, string stackTrace, LogType logType) {
		if(logType == LogType.Log) return;

		logString += "\n" + condition;

		logText.text = logString;
	}
}
