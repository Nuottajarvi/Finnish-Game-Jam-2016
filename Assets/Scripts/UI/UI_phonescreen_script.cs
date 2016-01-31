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

	[SerializeField]
	Text[] wordTexts;

	string logString;

	void Start() {
		id = "Not registered yet";
	}

	// Update is called once per frame
	void Update () {
		idText.text = "ID: " + id;
	}

	public void UpdateWordTexts() {
		if(words != null && words.Count > 0) {
			for(int i = 0; i < words.Count; i++) {
				if(wordTexts.Length <= i) break;

				wordTexts[i].text = words[i].word + "\n" + words[i].action;
			}
		}
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
