using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    static EnemySpawner instance;

    public static EnemySpawner Instance {
        get { return instance; }
    }

    bool spawning;

    List<Enemy> enemies;

    // Time between spawning new enemy
    float spawnDeltaTime = 4f;

    WaitForSeconds spawnWait;

    Transform enemyParent;

    // Speed at easiest difficulty when game starts
    const float StartMoveSpeed = 0.5f;

    // How much move speed is increased each time
    const float MoveSpeedIncrease = 0.1f;

    // Max speed after which speed isn't increased
    const float MaxMoveSpeed = 1.8f;

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

        spawnWait = new WaitForSeconds(spawnDeltaTime);

        StartCoroutine("SpawnRoutine");
	}

    IEnumerator SpawnRoutine() {
        lastDifficultyIncreaseTime = Time.time;

        while(spawning) {
            yield return spawnWait;

            if(Time.time - lastDifficultyIncreaseTime > DifficultyIncreaseDeltaTime) {
                IncreaseDifficulty();
                lastDifficultyIncreaseTime = Time.time;
            }

            Enemy enemy = SpawnEnemy();
            enemies.Add(enemy);
        }
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

    void IncreaseDifficulty() {
        currentMoveSpeed += MoveSpeedIncrease;

        currentMoveSpeed = Mathf.Min(currentMoveSpeed, MaxMoveSpeed);
    }
}
