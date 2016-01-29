using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_phonescreen_script : MonoBehaviour {

    public Text wordList;
    public string[] words;
    public WordActionGenerator.WordAction[] wordActions; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Empty text
        wordList.text = "";

        //Set text with all the contents from the lists
        for (int i = 0; i < words.Length; i++) {
            wordList.text = wordList.text + "\n<b>" + words[i] + "</b>\n" + wordActions[i] + "\n";
        }
	}
}
