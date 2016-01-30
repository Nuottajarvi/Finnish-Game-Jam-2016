using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

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

	List<Word> currentWords;

	// Which part of the ritual is the current action
	int currentWordIndex;

    private RitualHandler() {
        random = new System.Random();
		currentWords = new List<Word>();
    }

    public void NewRitual() {
        int wordCount = random.Next(4, 8);

        currentRitual = new string[wordCount];

        for(int i = 0; i < wordCount; i++) {
            currentRitual[i] = WordGenerator.GetWord();
        }

        GameUI.Instance.SetNewRitualText(currentRitual, string.Empty);

		DistributeWords();
    }

	void DistributeWords() {
		// TODO: Get client ids
		Debug.Log("Distributing words");
		string[] clientIds = /*new string[] { "2", "5", "6", "7" }*/ ServerNetworker.Instance.connectedPlayers.ToArray();

		currentWords.Clear();

		List<string> idsLeft = /*new List<string>(clientIds)*/ ServerNetworker.Instance.connectedPlayers;

		WordActionGenerator.WordAction[] possibleActions = System.Enum.GetValues(typeof(WordActionGenerator.WordAction)) as WordActionGenerator.WordAction[];

		for(int i = 0; i < currentRitual.Length; i++) {
			Word newWord = new Word();

			newWord.word = currentRitual[i];
			newWord.action = WordActionGenerator.GetWordAction();

			string chosenId = idsLeft[random.Next(0, idsLeft.Count)];

			newWord.clientId = chosenId;

			idsLeft.Remove(chosenId);

			if(idsLeft.Count == 0) {
				idsLeft = new List<string>(clientIds);
			}

			currentWords.Add(newWord);

		}

		for (int i = 0; i < clientIds.Length; i++) {
			SendWordDataToClient(clientIds[i]);
		}

	}

	public void SendWordDataToClient(string clientId) {
		List<Word> wordsToSend = new List<Word>();

		for(int i = 0; i < currentWords.Count; i++) {
			Word word = currentWords[i];

			if(word.clientId == clientId) {
				wordsToSend.Add(word);
			}
		}

		ServerNetworker.Instance.SendWordOut(wordsToSend, clientId);
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
		Rituals.DestructionAreaRitual();
	}
}
