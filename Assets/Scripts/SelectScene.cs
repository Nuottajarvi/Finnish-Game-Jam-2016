using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectScene : MonoBehaviour {

	public void LoadClientScene()  {
		SceneManager.LoadScene("client");
	}

	public void LoadServerScene() {
		SceneManager.LoadScene("lobby");
	}
}
