using UnityEngine;
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

	[SerializeField]
	Button areaRitualTestButton;

	string[] currentWords;

	float redFlashTime;

    void Awake() {
        if(instance == null)
            instance = this;

		areaRitualTestButton.onClick.AddListener(() => {
			Rituals.CompleteRitual();
		});
    }

	void Update() {
		if(redFlashTime > 0f && Time.time - redFlashTime > 0.5f) {
			redFlashTime = 0f;

			SetNewRitualText(currentWords, string.Empty);
		}
	}

    public void SetWaveText(int wave) {
        waveText.text = "Wave " + wave.ToString();
    }

    public void SetHealthBarFill(float fill) {
        healthBar.fillAmount = fill;
    }

	public void IncreaseHealthBarFill(float amount) {
		healthBar.fillAmount += amount;
	}

	public float GetHealthBarFill() {
		return healthBar.fillAmount;
	}

    public void SetNewRitualText(string[] words, string color) {
		this.currentWords = words;
		string ritualString = string.Empty;

		if(color.Length > 0) {
			ritualString += "<color=" + color + ">";
		}

        for(int i = 0; i < words.Length; i++) {
			ritualString += words[i];

			if(i < words.Length - 1) ritualString += " ";
        }

		if(color.Length > 0) {
			ritualString += "</color>";
		}

        ritualText.text = ritualString;
    }

	public void FlashRitualText(string color) {
		SetNewRitualText(currentWords, color);
	}
}
