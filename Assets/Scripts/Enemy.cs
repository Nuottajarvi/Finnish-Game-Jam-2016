using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public enum Type { Normal, Boss }
	[SerializeField] public Type type;

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
		float spawnDirectionRad;
		float spawnDirectionDeg;

		if(type == Type.Normal) {
			spawnDirectionRad = (float)GameController.jamRandomer.NextDouble() * 2 * Mathf.PI;
			spawnDirectionDeg = (spawnDirectionRad / (2 * Mathf.PI)) * 360;
		} else {
			spawnDirectionRad = 0;
			spawnDirectionDeg = 0;
		}

        int spawnDistance = GameController.jamRandomer.Next(SpawnDistanceMin, SpawnDistanceMax);

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
		if(GameObject.Find("AudioPlayer").transform.FindChild("EnemyHits").GetComponent<AudioSource>().isPlaying == false)
		{
			GameObject.Find("AudioPlayer").transform.FindChild("EnemyHits").GetComponent<AudioSource>().Play();
		}
		

		gameObject.SetActive(false);

		if(type == Type.Normal) {
			GameController.Instance.ChangeHealth(-0.1f);
		} else {
			GameController.Instance.ChangeHealth(-1f);
			GameController.Instance.LoseGame();
		}
    }
}
