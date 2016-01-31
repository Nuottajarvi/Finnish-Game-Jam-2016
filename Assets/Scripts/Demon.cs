using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

public class Demon : MonoBehaviour {

	float time = 0f;

	void Update(){
		time+=Time.deltaTime;
		if(time > 2){
			StartCoroutine("Raise");
			time = -100000;
		}
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
		BloomOptimized bloom = GameObject.Find("Main Camera").GetComponent<BloomOptimized>();
		bloom.intensity = 0.3f;

		yield return new WaitForSeconds(1f);

		ringParticles.emissionRate = 30;

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		while(spriteRenderer.color.a < 250){
			spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(1,1,1,1), Time.deltaTime * 2);
			yield return new WaitForFixedUpdate();
		}
	}

	public IEnumerator Reset(){
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		while(spriteRenderer.color.a > 1){
			spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(0,0,0,0), Time.deltaTime * 2);
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
