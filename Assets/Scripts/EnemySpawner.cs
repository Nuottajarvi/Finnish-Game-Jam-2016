using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    bool spawning;

    List<Enemy> enemies;

    // Time between spawning new enemy
    float spawnDeltaTime = 4f;

    WaitForSeconds spawnWait;

	// Use this for initialization
	void Start () {
        enemies.Add(CreateEnemy());

        GameObject firstEnemyObject = Instantiate(Resources.Load("Prefabs/Enemy")) as GameObject;
        enemies.Add(firstEnemyObject.GetComponent<Enemy>());

        spawnWait = new WaitForSeconds(spawnDeltaTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator SpawnRoutine() {
        while(spawning) {
            yield return spawnWait;

            Enemy enemy = CreateEnemy();
            enemies.Add(enemy);
        }
    }

    Enemy CreateEnemy() {
        GameObject enemyObject = Instantiate(enemies[0].gameObject) as GameObject;

        Enemy enemy = enemyObject.GetComponent<Enemy>();

        return enemy;
    }
}
