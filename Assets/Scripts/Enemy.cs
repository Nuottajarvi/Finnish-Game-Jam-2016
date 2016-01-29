using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    static Transform summoningCircle;

    Transform enemyTransform;

    const int SpawnDistanceMin = 22;
    const int SpawnDistanceMax = 38;


    // How far away enemy has to be to center of summoning circle
    // when enemy has 'won'
    const float PlayerReachDistance = 5f;

    // Current move speed
    public float MoveSpeed {
        get; set;
    }

    void Awake() {
        enemyTransform = transform;
        if(summoningCircle == null) summoningCircle = GameObject.Find("SummoningCircle").transform;
    }

    void OnEnable() {
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
	void Update() {
        Vector3 targetDir = summoningCircle.position - enemyTransform.position;
        float distanceToCircle = targetDir.magnitude;

        if(distanceToCircle <= PlayerReachDistance) {
            ReachedCircle();
        }

        targetDir = targetDir.normalized;
        
        enemyTransform.Translate(targetDir * MoveSpeed * Time.deltaTime);
	}

    void ReachedCircle() {
        gameObject.SetActive(false);
    }
}
