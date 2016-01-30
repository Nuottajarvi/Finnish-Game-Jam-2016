using UnityEngine;
using System.Collections;

public class Rituals {
	public static void DestructionAreaRitual() {
		GameObject.Find("DestroyArea").GetComponent<DestroyArea>().Activate();
	}

	public static void DestroyAllRitual() {
		EnemySpawner.Instance.DestroyAllEnemies();
	}

	public static void PushbackRitual() {
		EnemySpawner.Instance.Pushback();
	}
}
