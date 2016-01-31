using UnityEngine;
using System.Collections;

public class Shake : PhoneAction {

 
static float accelerometerUpdateInterval = 1.0f / 60.0f;
 
// The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa).
static float lowPassKernelWidthInSeconds  = 1.0f;
 
// This next parameter is initialized to 2.0 per Apple's recommendation, or at least according to Brady! <img draggable="false" class="emoji" alt="😉" src="http://s.w.org/images/core/emoji/72x72/1f609.png">
float shakeDetectionThreshold = 1.2f;
 
private float lowPassFilterFactor  = accelerometerUpdateInterval / lowPassKernelWidthInSeconds; 
private Vector3 lowPassValue = Vector3.zero;
private Vector3 acceleration;
	private Vector3 deltaAcceleration;
 
 
void Start()

	{
		shakeDetectionThreshold *= shakeDetectionThreshold;
		lowPassValue = Input.acceleration;

		sendTimer = 0;
		//time between shortest send in seconds
		sendLimit = 1.0f;

	}


	void Update()
	{
		sendTimer = sendTimer + Time.deltaTime;

		acceleration = Input.acceleration;
		lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
		deltaAcceleration = acceleration - lowPassValue;
		if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
		{
			if (sendTimer > sendLimit)
			{
				sendTimer = 0;
				clientNetworker.WordOut(WordActionGenerator.WordAction.Shake);

			}
		}

	}





}
