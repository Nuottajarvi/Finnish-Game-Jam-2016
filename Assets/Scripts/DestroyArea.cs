using UnityEngine;
using System.Collections;

public class DestroyArea : MonoBehaviour {
	Material destroyAreaMaterial;

	public static bool active;

	// Use this for initialization
	void Start () {
		destroyAreaMaterial = GetComponent<MeshRenderer>().materials[0];
	}
	
	// Update is called once per frame
	void Update () {
		if(destroyAreaMaterial.color.a > 0f) {
			Color ogColor = destroyAreaMaterial.color;

			ogColor.a -= 0.666f * Time.deltaTime;
			ogColor.a = Mathf.Max(0, ogColor.a);

			destroyAreaMaterial.color = ogColor;
		} else {
			active = false;
		}
	}

	void OnTriggerStay(Collider collider) {
		if(collider.name.Contains("Enemy") && active) {
			collider.gameObject.SetActive(false);
		}
	}

	public void Activate() {
		active = true;

		Color ogColor = destroyAreaMaterial.color;
		ogColor.a = 1f;

		destroyAreaMaterial.color = ogColor;
	}
}
