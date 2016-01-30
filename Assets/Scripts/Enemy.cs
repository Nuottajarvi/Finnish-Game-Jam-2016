using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    static Transform summoningCircle;

    Transform enemyTransform;

    const int SpawnDistanceMin = 20;
    const int SpawnDistanceMax = 22;

    public static int ActiveCount;

    // How far away enemy has to be to center of summoning circle
    // when enemy has 'won'
    const float PlayerReachDistance = 2f;

	float distanceToCircle;
	public float DistanceToCircle {
		get {
			return distanceToCircle;
		}
	}

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

        ActiveCount++;
    }

    void OnDisable() {
        ActiveCount--;
    }

    void ChooseSpawnPosition() {
        System.Random rand = new System.Random();

        float spawnDirectionRad = (float)rand.NextDouble() * 2 * Mathf.PI;
        float spawnDirectionDeg = (spawnDirectionRad / (2 * Mathf.PI)) * 360;

        int spawnDistance = rand.Next(SpawnDistanceMin, SpawnDistanceMax);

        enemyTransform.position = Vector3.zero;

        Vector3 spawnOffset = new Vector3(Mathf.Cos(spawnDirectionRad),
                                          Mathf.Sin(spawnDirectionRad),
                                          0f).normalized;

        spawnOffset *= spawnDistance;
        spawnOffset.z = -5f;
        enemyTransform.Translate(spawnOffset);

        enemyTransform.Rotate(-Vector3.forward * (180 - spawnDirectionDeg));

        GetComponent<SpriteRenderer>().flipY = enemyTransform.position.x > 0;
    }

	// Update is called once per frame
	void Update() {
        Vector3 targetDir = summoningCircle.position - enemyTransform.position;
        targetDir.z = 0;

        distanceToCircle = targetDir.magnitude;

        if(distanceToCircle <= PlayerReachDistance) {
            ReachedCircle();
        }

        targetDir = targetDir.normalized;

        enemyTransform.position += targetDir * MoveSpeed * Time.deltaTime;
	}

    void ReachedCircle() {
        gameObject.SetActive(false);

        GameController.Instance.ReduceHealth();
    }
}
