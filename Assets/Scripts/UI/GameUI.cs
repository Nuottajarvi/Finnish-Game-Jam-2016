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

    void Awake() {
        if(instance == null)
            instance = this;

		areaRitualTestButton.onClick.AddListener(() => {
			Rituals.CompleteRitual();
		});
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
