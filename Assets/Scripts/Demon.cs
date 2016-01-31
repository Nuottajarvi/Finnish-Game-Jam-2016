using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;
using System;

public class Demon : MonoBehaviour {
	void Update(){
		if(Time.frameCount == 60) StartRise();
	}

	public void StartRise() {
		StartCoroutine("Raise");
	}

	public IEnumerator Raise(){
		//CHANGE RING COLOR
		Image healthbar = GameObject.Find("Bar").GetComponent<Image>();
		healthbar.color = Color.red;


		//CHANGE PARTICLE COLORS
		ParticleSystem ringParticles = GameObject.Find("RitualCircleParticles").GetComponent<ParticleSystem>();
		ringParticles.startColor = Color.red;
		ringParticles.emissionRate = 300;

		//DECREASE BLOOM
		BloomOptimized bloom = Camera.main.GetComponent<BloomOptimized>();
		bloom.intensity = 0.3f;

		yield return new WaitForSeconds(1f);

		ringParticles.emissionRate = 30;

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		while(spriteRenderer.color.a < 0.99){
			spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(1,1,1,1), Time.deltaTime * 2);
			yield return new WaitForFixedUpdate();
			//Debug.Log(spriteRenderer.color.a);
		}

		GameObject fireBall = transform.GetChild(0).gameObject;
		SpriteRenderer fireBallSprite = fireBall.GetComponent<SpriteRenderer>();
		fireBallSprite.color = new Color(1, 1, 1, 1);

		float time = 0;
		while(fireBall.transform.position.x < 50) {
			time += Time.deltaTime;
			if(time > 0.25f) {
				fireBallSprite.flipY = !fireBallSprite.flipY;
				time = 0;
			}

			if(fireBall.transform.position.x > EnemySpawner.Instance.GetBossPosition().x) {
				EnemySpawner.Instance.KillBoss();
			}

			fireBall.transform.position+=new Vector3(3 + fireBall.transform.position.x,0,0) * Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}

		yield return StartCoroutine("Reset");
	}

	public IEnumerator Reset(){
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		GameObject fireBall = transform.GetChild(0).gameObject;
		SpriteRenderer fireBallSprite = fireBall.GetComponent<SpriteRenderer>();
		fireBallSprite.color = new Color(0, 0, 0, 0);
		fireBall.transform.position = new Vector3(0, -0.5f, 0);

		while(spriteRenderer.color.a > 0.01f){
			spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(1,1,1,0), Time.deltaTime * 2);
			yield return new WaitForFixedUpdate();
		}

		//CHANGE RING COLOR
		Image healthbar = GameObject.Find("Bar").GetComponent<Image>();
		healthbar.color = Color.cyan;


		//CHANGE PARTICLE COLORS
		ParticleSystem ringParticles = GameObject.Find("RitualCircleParticles").GetComponent<ParticleSystem>();
		ringParticles.startColor = Color.cyan;

		//DECREASE BLOOM
		BloomOptimized bloom = GameObject.Find("Main Camera").GetComponent<BloomOptimized>();
		bloom.intensity = 0.7f;
	}
}
