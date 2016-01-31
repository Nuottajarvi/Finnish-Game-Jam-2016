using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton that controls the game state
/// </summary>
public class GameController : MonoBehaviour {
    static GameController instance;
    public static GameController Instance {
        get {
            return instance;
        }
    }

	public static System.Random jamRandomer = new System.Random(666);

	Dictionary<string, List<Word>> wordsByClient;

	const float StartingHealth = 1.0f;

    public float Health {
        get; private set;
    }

    void Awake() {
        instance = this;
        Health = StartingHealth;
    }

	// Use this for initialization
	void Start () {
		WordGenerator.CreateWordPool();

		RitualHandler.Instance.DistributeWords();
        RitualHandler.Instance.NewRitual();

		CreateRitualists();
	}

	// Update is called once per frame
	void Update () {
		ChangeHealth(Time.deltaTime * 0.01f);

		if(EnemySpawner.Instance.IsBossWave) {
			if(Health >= 1.0f) {
				EnemySpawner.Instance.StopBoss();
				Health = 0.0f;
			}
		}
	}

    public void ChangeHealth(float amount) {
        Health += amount;

		Health = Mathf.Clamp(Health, 0f, 1f);

		GameUI.Instance.SetHealthBarFill(Health);
     }

    public void LoseGame() {
		if(ServerNetworker.Instance != null)
			ServerNetworker.Instance.GameLostOut();

		SceneManager.LoadScene("selection");
    }

	void CreateRitualists() {
		if(ServerNetworker.Instance == null) return;
		int ritualistCount = ServerNetworker.Instance.connectedPlayers.Count;

		List<float> rads = new List<float> { 0, Mathf.PI / 4, Mathf.PI / 2, 3/4 * Mathf.PI, 5/4 * Mathf.PI, 225, 3/2 * Mathf.PI, 7/4 * Mathf.PI };

		Transform ritualistParent = GameObject.Find("SummoningCircle/Ritualists").transform;

		GameObject firstRitualist = ritualistParent.GetChild(0).gameObject;

		for(int i = 1; i < ritualistCount; i++) {
			if(rads.Count == 0) break;

			Transform newRitualist;

			if(i < ritualistCount - 1) {
				newRitualist = ((GameObject)Instantiate(firstRitualist)).transform;
			} else {
				newRitualist = firstRitualist.transform;
			}

			int rad = GameController.jamRandomer.Next(rads.Count);

			newRitualist.position = Vector3.zero;

			Vector3 spawnOffset = new Vector3(Mathf.Cos(rad),
								  Mathf.Sin(rad),
								  0f).normalized;

			newRitualist.position += spawnOffset * 3f + new Vector3(0f, 1.5f, 0f);

			newRitualist.parent = ritualistParent;
			newRitualist.localScale = Vector3.one;

			Vector3 pos = newRitualist.localPosition;
			pos.z = 0f;

			newRitualist.localPosition = pos;
			
			rads.Remove(rad);
		}
	}
}
