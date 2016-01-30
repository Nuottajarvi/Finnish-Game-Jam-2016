using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {
    static EnemySpawner instance;

    public static EnemySpawner Instance {
        get { return instance; }
    }

    bool spawning;

    List<Enemy> enemies;

    public int CurrentWave {
        get; private set;
    }

    // Time between spawning new enemy
    float singleSpawnDeltaTime = 4f;
    float waveSpawnDeltaTime = 5f;

    WaitForSeconds spawnWaitSingle;
    WaitForSeconds spawnWaitWave;

    // How many enemies still have to be spawned for the current wave
    int waveEnemiesLeft;

    Transform enemyParent;

    // Speed at easiest difficulty when game starts
    const float StartMoveSpeed = 0.9f;

    // How much move speed is increased each time
    const float MoveSpeedIncrease = 0.1f;

    // Max speed after which speed isn't increased
    const float MaxMoveSpeed = 2.5f;

    float currentMoveSpeed;

    const float DifficultyIncreaseDeltaTime = 15f;
    float lastDifficultyIncreaseTime;

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

    IEnumerator SpawnRoutine() {
        lastDifficultyIncreaseTime = Time.time;

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
        GameUI.Instance.SetWaveText(CurrentWave);

        IncreaseMoveSpeed();

        waveEnemiesLeft = 3 + CurrentWave;
    }

    Enemy SpawnEnemy() {
        Enemy enemy = GetFreeEnemy();

        if(enemy == null) {
            enemy = InstantiateEnemy();
        }

        enemy.MoveSpeed = currentMoveSpeed;
        enemy.gameObject.SetActive(true);

        return enemy;
    }

    Enemy GetFreeEnemy() {
        for(int i = 0; i < enemies.Count; i++) {
            if(!enemies[i].gameObject.activeInHierarchy) return enemies[i];
        }

        return null;
    }

    Enemy InstantiateEnemy() {
        GameObject enemyObject = Instantiate(Resources.Load("Prefabs/Enemy")) as GameObject;
        enemyObject.transform.parent = enemyParent;
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemies.Add(enemyObject.GetComponent<Enemy>());

        return enemy;
    }

    void IncreaseMoveSpeed() {
        currentMoveSpeed += MoveSpeedIncrease;

        currentMoveSpeed = Mathf.Min(currentMoveSpeed, MaxMoveSpeed);
    }
}
