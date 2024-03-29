﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {
    static EnemySpawner instance;

	public delegate void NewWaveDelegate(int wave);
	public static event NewWaveDelegate onNewWave;

    public static EnemySpawner Instance {
        get { return instance; }
    }

    bool spawning;

    List<Enemy> enemies;

    public int CurrentWave {
        get; private set;
    }

	[SerializeField]
	GameObject destroyArea;

    // Time between spawning new enemy
    float singleSpawnDeltaTime = 1.5f;
    float waveSpawnDeltaTime = 1f;

    WaitForSeconds spawnWaitSingle;
    WaitForSeconds spawnWaitWave;

    // How many enemies still have to be spawned for the current wave
    int waveEnemiesLeft;

    Transform enemyParent;

	const float BossSpeedModifier = 0.3f;

    // Speed at easiest difficulty when game starts
    const float StartMoveSpeed = 1.0f;

    // How much move speed is increased each time
    const float MoveSpeedIncrease = 0.1f;

    // Max speed after which speed isn't increased
    const float MaxMoveSpeed = 3.5f;

    float currentMoveSpeed;

	// Every x wave is a boss wave
	const int bossWaveDelta = 4;

	const float pushbackTime = 0.5f;
	float lastPushback;

	public bool IsBossWave {
		get {
			return (CurrentWave != 0 && CurrentWave % bossWaveDelta == 0);
		}
	}
    void Awake() {
        instance = this;

        currentMoveSpeed = StartMoveSpeed;
    }

	// Use this for initialization
	void Start () {
        spawning = true;

        enemies = new List<Enemy>();
        enemyParent = GameObject.Find("Enemies").transform;

        spawnWaitSingle = new WaitForSeconds(singleSpawnDeltaTime);
        spawnWaitWave = new WaitForSeconds(waveSpawnDeltaTime);

        StartCoroutine("SpawnRoutine");
	}

	void FixedUpdate() {
		if(lastPushback > 0f && Time.time - lastPushback > pushbackTime) {
			RestoreSpeeds();
			lastPushback = 0f;
		}

	}

	void Update() {
		if(IsBossWave) {
			GameUI.Instance.IncreaseHealthBarFill(Time.deltaTime * 0.04f);
		}
	}

    IEnumerator SpawnRoutine() {
		CurrentWave = 0;
		NextWave();

        while(spawning) {
            while(waveEnemiesLeft > 0) {
                SpawnEnemy();
                yield return spawnWaitSingle;

                waveEnemiesLeft--;
            }

            // Wait until all enemies have reached circle/died
            while(Enemy.ActiveCount > 0) {
                yield return null;
            }

            NextWave();
            yield return spawnWaitWave; // Wait before starting to spawn enemies to this wave
        }
    }

    void NextWave() {
        CurrentWave++;

		if(onNewWave != null)
			onNewWave(CurrentWave);

        GameUI.Instance.SetWaveText(CurrentWave);

        IncreaseMoveSpeed();

		if(IsBossWave) {
			waveEnemiesLeft = 1;
			GameUI.Instance.SetHealthBarFill(0f);
		} else {
			waveEnemiesLeft = 3 + CurrentWave / 2;
		}  
    }

    Enemy SpawnEnemy() {
		Enemy.Type enemyType;

		if(IsBossWave) {
			enemyType = Enemy.Type.Boss;
		} else {
			enemyType = Enemy.Type.Normal;
		}

        Enemy enemy = GetFreeEnemy(enemyType);

        if(enemy == null) {
            enemy = InstantiateEnemy(enemyType);
        }

		enemy.MoveSpeed = currentMoveSpeed;

		if(enemy.type == Enemy.Type.Boss) {
			enemy.MoveSpeed *= BossSpeedModifier;
		}

        enemy.gameObject.SetActive(true);

        return enemy;
    }

    Enemy GetFreeEnemy(Enemy.Type enemyType) {
        for(int i = 0; i < enemies.Count; i++) {
            if(!enemies[i].gameObject.activeInHierarchy &&
				enemies[i].type == enemyType) return enemies[i];
        }

        return null;
    }

    Enemy InstantiateEnemy(Enemy.Type enemyType) {
		string prefabPath = string.Empty;

		if(enemyType == Enemy.Type.Normal) {
			prefabPath = "Prefabs/Enemy";
		} else if(enemyType == Enemy.Type.Boss) {
			prefabPath = "Prefabs/Boss";
		}

        GameObject enemyObject = Instantiate(Resources.Load(prefabPath)) as GameObject;
        enemyObject.transform.parent = enemyParent;
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemies.Add(enemyObject.GetComponent<Enemy>());

        return enemy;
    }

    void IncreaseMoveSpeed() {
        currentMoveSpeed += MoveSpeedIncrease;

        currentMoveSpeed = Mathf.Min(currentMoveSpeed, MaxMoveSpeed);
    }

	public void DestroyAllEnemies() {
		foreach(Enemy e in enemies) {
			e.gameObject.SetActive(false);
		}
	}

	public void Pushback() {
		lastPushback = Time.time;

		foreach(Enemy enemy in enemies) {
			enemy.MoveSpeed = -currentMoveSpeed * 3f;
		}
		// TODO: Push all enemies away from circle
	}

	public void StopBoss() {
		foreach(Enemy enemy in enemies) {
			if(enemy.type == Enemy.Type.Boss) {
				enemy.MoveSpeed = 0f;
				break;
			}
		}

		GameObject.Find("AudioPlayer").transform.FindChild("DemonSummon").GetComponent<AudioSource>();

		GameObject.Find("demon").GetComponent<Demon>().StartRise();
	}

	public void KillBoss() {
		foreach(Enemy enemy in enemies) {
			if(enemy.type == Enemy.Type.Boss) {
				enemy.gameObject.SetActive(false);
				return;
			}
		}
	}

	public void RestoreSpeeds() {
		foreach(Enemy enemy in enemies) {
			if(enemy.type == Enemy.Type.Normal) {
				enemy.MoveSpeed = currentMoveSpeed;
			} else {
				enemy.MoveSpeed = currentMoveSpeed * BossSpeedModifier;
			}
		}
	}

	public Vector3 GetBossPosition() {
		foreach(Enemy enemy in enemies) {
			if(enemy.type == Enemy.Type.Boss) {
				return enemy.transform.position;
			}
		}

		return Vector3.zero;
	}
}
