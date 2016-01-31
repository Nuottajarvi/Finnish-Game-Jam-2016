using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
	Transform[] wordElements;

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
				if(wordElements.Length <= i) break;

				Transform wordParent = wordElements[i];

				wordParent.FindChild("Name").GetComponent<Text>().text = words[i].word;
				wordParent.FindChild("ActionName").GetComponent<Text>().text = words[i].action.ToString();

				Transform actionImageParent = wordParent.FindChild("ActionImage");

				foreach(Transform child in actionImageParent) {
					child.gameObject.SetActive(false);
				}

				for(int j = 0; j < actionImageParent.childCount; j++) {
					GameObject imageObject = actionImageParent.GetChild(j).gameObject;

					if(imageObject.name.Equals(words[i].action.ToString())) {
						imageObject.SetActive(true);
					}
				}
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

	public static void LoseGame() {
		SceneManager.LoadScene("selection");
	}
}
