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

	Dictionary<string, List<Word>> wordsByClient;

    private RitualHandler() {
        random = new System.Random();
		currentWords = new List<Word>();
    }

    public void NewRitual() {
		int wordCount = 4 + EnemySpawner.Instance.CurrentWave / 2;

        currentRitual = new string[wordCount];

        for(int i = 0; i < wordCount; i++) {
			string word = WordGenerator.GetWord();

			while(System.Array.IndexOf(currentRitual, word) >= 0) {
				word = WordGenerator.GetWord();
			}

			currentRitual[i] = word;
        }

        GameUI.Instance.SetNewRitualText(currentRitual, string.Empty);
    }

	public void DistributeWords() {
		wordsByClient = new Dictionary<string, List<Word>>();

		List<string> clientIds = ServerNetworker.Instance.connectedPlayers;

		if(clientIds.Count == 0) return;

		for(int i = 0; i < clientIds.Count; i++) {
			wordsByClient[clientIds[i]] = new List<Word>();
		}

		System.Random rand = new System.Random();

		List<string> wordPool = new List<string>(WordGenerator.CurrentWordPool);
		

		while(wordPool.Count > 0) {
			Word word = new Word();

			word.word = wordPool[rand.Next(0, wordPool.Count)];
			word.action = WordActionGenerator.GetWordAction();
			word.clientId = clientIds[wordPool.Count % clientIds.Count];

			wordsByClient[word.clientId].Add(word);

			wordPool.Remove(word.word);
		}

		foreach(KeyValuePair<string, List<Word>> pair in wordsByClient) {
			SendWordDataToClient(pair.Key, pair.Value);
		}
	}

	public void SendWordDataToClient(string clientId, List<Word> wordsToSend) {
		Debug.Log("Send word data to " + clientId);

		for(int i = 0; i < currentWords.Count; i++) {
			Word word = currentWords[i];

			if(word.clientId == clientId) {
				wordsToSend.Add(word);
			}
		}

		ServerNetworker.Instance.SendWordOut(wordsToSend, clientId);
	}

	public void OnNewAction(JSONArray json) {
		for(int i = 0; i < json.Count; i++) {
			if(json[i]["word"].Value.ToLower() == currentRitual[currentWordIndex].ToLower()) {
				currentWordIndex++;

				if(currentWordIndex >= currentRitual.Length) {
					RitualComplete();
					NewRitual();
				}

				return;
			}
		}
		
		currentWordIndex = 0;

		GameUI.Instance.SetNewRitualText(currentRitual, "red");
	}

	void RitualComplete() {
		Rituals.DestructionAreaRitual();
	}
}
