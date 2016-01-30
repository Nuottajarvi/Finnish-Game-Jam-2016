using UnityEngine;
using System.Collections;
using SimpleJSON;

public class RitualHandler {
    static RitualHandler instance;
    public static RitualHandler Instance {
        get {
            if(instance == null) {
                instance = new RitualHandler();
            }
            return instance;
        }
    }

    System.Random random;

    static string[] currentRitual;
    public static string[] CurrentRitual {
        get {
			return currentRitual;
		}
    }

	// Which part of the ritual is the current action
	int currentWordIndex;

    private RitualHandler() {
        random = new System.Random();
    }

    public void NewRitual() {
        int wordCount = random.Next(4, 8);

        currentRitual = new string[wordCount];

        for(int i = 0; i < wordCount; i++) {
            currentRitual[i] = WordGenerator.GetWord();
        }

        GameUI.Instance.SetNewRitualText(currentRitual, string.Empty);
    }

	public void OnNewAction(JSONArray json) {
		if(currentRitual[currentWordIndex] == json["word"]) {
			currentWordIndex++;

			if(currentWordIndex >= currentRitual.Length) {
				RitualComplete();
				NewRitual();
			}
		} else {
			currentWordIndex = 0;

			GameUI.Instance.SetNewRitualText(currentRitual, "red");
		}
	}

	void RitualComplete() {
		EnemySpawner.Instance.DestructionRitual();
	}
}
