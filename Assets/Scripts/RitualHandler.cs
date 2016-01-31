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
		currentWords = new List<Word>();

		EnemySpawner.onNewWave += OnNewWave;
    }

	void OnNewWave(int wave) {
		if(wave > 4 && (wave - 1) % 4 == 0) {
			// New words after each boss wave
			DistributeWords();
			NewRitual();
		}
	}

    public void NewRitual() {
		int wordCount = 4 + EnemySpawner.Instance.CurrentWave / 4;

		wordCount = Mathf.Min(wordCount, WordGenerator.CurrentWordPool.Length);

        currentRitual = new string[wordCount];

        for(int i = 0; i < wordCount; i++) {
			string word = WordGenerator.GetWord();

			while(System.Array.IndexOf(currentRitual, word) >= 0) {
				word = WordGenerator.GetWord();
			}

			currentRitual[i] = word;
        }

		currentWordIndex = 0;

        GameUI.Instance.SetNewRitualText(currentRitual, string.Empty);
    }

	public void DistributeWords() {
		wordsByClient = new Dictionary<string, List<Word>>();

		if(ServerNetworker.Instance == null) return;

		List<string> clientIds = ServerNetworker.Instance.connectedPlayers;

		if(clientIds.Count == 0) return;

		WordGenerator.CreateWordPool();

		for(int i = 0; i < clientIds.Count; i++) {
			wordsByClient[clientIds[i]] = new List<Word>();
		}

		List<string> wordPool = new List<string>(WordGenerator.CurrentWordPool);
		

		while(wordPool.Count > 0) {
			Word word = new Word();

			word.word = wordPool[GameController.jamRandomer.Next(0, wordPool.Count)];
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

				GameObject.Find("AudioPlayer").transform.FindChild("Succesful").GetComponent<AudioSource>().Play();

				currentWordIndex++;

				if(currentWordIndex >= currentRitual.Length) {
					currentWordIndex = 0;
					RitualComplete();
					NewRitual();
				} else {
					GameUI.Instance.FlashRitualText("green");
				}

				return;
			}
		}

		GameObject.Find("AudioPlayer").transform.FindChild("Unsuccesful").GetComponent<AudioSource>().Play();

		currentWordIndex = 0;

		GameUI.Instance.FlashRitualText("red");
	}

	void RitualComplete() {
		Rituals.CompleteRitual();
	}
}
