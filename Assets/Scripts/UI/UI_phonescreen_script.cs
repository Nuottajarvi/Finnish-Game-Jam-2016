using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_phonescreen_script : MonoBehaviour {

    public Text wordList;
	//public string[] words;
	//public WordActionGenerator.WordAction[] wordActions; 
	public List<Word> words;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Empty text
        wordList.text = "";

        //Set text with all the contents from the lists
        for (int i = 0; i < words.Count; i++) {
            wordList.text = wordList.text + "\n<b>" + words[i].word + "</b>\n" + words[i].action + "\n";
        }
	}
}
