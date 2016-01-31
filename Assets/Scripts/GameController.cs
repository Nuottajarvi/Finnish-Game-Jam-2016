using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
	}

	// Update is called once per frame
	void Update () {
		ChangeHealth(Time.deltaTime * 0.01f);

		if(EnemySpawner.Instance.IsBossWave) {
			if(Health >= 1.0f) {
				EnemySpawner.Instance.KillBoss();
			}
		}
	}

    public void ChangeHealth(float amount) {
        Health += amount;

		Health = Mathf.Clamp(Health, 0f, 1f);

		GameUI.Instance.SetHealthBarFill(Health);
     }

    void LoseGame() {
        Debug.Log("Game lost");
    }
}
