using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    static Transform summoningCircle;

    Transform enemyTransform;

    const int SpawnDistanceMin = 25;
    const int SpawnDistanceMax = 32;

    // Speed at easiest difficulty when game starts
    const float StartMoveSpeed = 1.5f;

    // How much move speed is increased each time
    const float MoveSpeedIncrease = 0.1f;

    // Max speed after which speed isn't increased
    const float MaxMoveSpeed = 2.5f;

    // How far away enemy has to be to center of summoning circle
    // when enemy has 'won'
    const float PlayerReachDistance = 5f;

    // Current move speed
    float moveSpeed;

	// Use this for initialization
	void Start () {
        enemyTransform = transform;

        moveSpeed = StartMoveSpeed;

        if(summoningCircle == null) summoningCircle = GameObject.Find("SummoningCircle").transform;

        ChooseSpawnPosition();
	}

    void ChooseSpawnPosition() {
        System.Random rand = new System.Random();

        float spawnDirectionRad = (float)rand.NextDouble() * 2 * Mathf.PI;
        int spawnDistance = rand.Next(SpawnDistanceMin, SpawnDistanceMax);

        Vector3 spawnOffset = new Vector3(Mathf.Cos(spawnDirectionRad) * spawnDistance,
                                          Mathf.Sin(spawnDirectionRad) * spawnDistance,
                                          0f);

        enemyTransform.Translate(spawnOffset);
    }

	// Update is called once per frame
	void Update () {
        Vector3 targetDir = summoningCircle.position - enemyTransform.position;
        float distanceToCircle = targetDir.magnitude;

        if(distanceToCircle <= PlayerReachDistance) {
            ReachedCircle();
        }

        targetDir = targetDir.normalized;
        
        enemyTransform.Translate(targetDir * moveSpeed * Time.deltaTime);
	}

    void ReachedCircle() {
        Debug.Log("Enemy reached to the circle");
        Destroy(gameObject);
    }
}
