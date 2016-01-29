using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    bool spawning;

    List<Enemy> enemies;

    // Time between spawning new enemy
    float spawnDeltaTime = 4f;

    WaitForSeconds spawnWait;

    Transform enemyParent;

	// Use this for initialization
	void Start () {
        spawning = true;

        enemies = new List<Enemy>();
        enemyParent = GameObject.Find("Enemies").transform;

        GameObject firstEnemyObject = Instantiate(Resources.Load("Prefabs/Enemy")) as GameObject;
        enemies.Add(firstEnemyObject.GetComponent<Enemy>());

        spawnWait = new WaitForSeconds(spawnDeltaTime);

        StartCoroutine("SpawnRoutine");
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
        Enemy enemy = GetFreeEnemy();

        if(enemy == null) {
            GameObject enemyObject = Instantiate(Resources.Load("Prefabs/Enemy")) as GameObject;
            enemyObject.transform.parent = enemyParent;
            enemy = enemyObject.GetComponent<Enemy>();
            enemies.Add(enemyObject.GetComponent<Enemy>());
        }

        enemy.gameObject.SetActive(true);

        return enemy;
    }

    Enemy GetFreeEnemy() {
        for(int i = 0; i < enemies.Count; i++) {
            if(!enemies[i].gameObject.activeInHierarchy) return enemies[i];
        }

        return null;
    }
}
