﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    static GameUI instance;
    public static GameUI Instance {
        get {
            return instance;
        }
    }

    [SerializeField]
    Text ritualText;

    [SerializeField]
    Text waveText;

    [SerializeField]
    Image healthBar;

    void Awake() {
        if(instance == null)
            instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetWaveText(int wave) {
        waveText.text = "Wave " + wave.ToString();
    }

    public void SetHealthBarFill(float fill) {
        healthBar.fillAmount = fill;
    }

    public void SetNewRitualText(string[] words, string color) {
        string ritualString = string.Empty;

        for(int i = 0; i < words.Length; i++) {
			// Start of rich text color tag <color=red> </color>
			string colorStartString = string.Empty;

			ritualString += colorStartString + words[i];

			if(colorStartString.Length > 0) {
				ritualString += "</color>";
			}

			if(i < words.Length - 1) ritualString += " ";
        }

        ritualText.text = ritualString;
    }
}