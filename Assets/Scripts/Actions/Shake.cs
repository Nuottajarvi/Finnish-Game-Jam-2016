using UnityEngine;
using System.Collections;

public class Shake : PhoneAction {

	/* Start shake detection variables */
	public float avrgTime = 0.5f;
	public float peakLevel = 0.6f;
	public float endCountTime = 0.6f;
	public int shakeDir;
	public int shakeCount;

	Vector3 avrgAcc = Vector3.zero;
	int countPos;
	int countNeg;
	int lastPeak;
	int firstPeak;
	bool counting;
	float timer;


	/* End shake detection variables */




	// Use this for initialization
	void Start () {
		sendTimer = 0.0f;
	}

	bool ShakeDetector()
	{
		// read acceleration:
		Vector3 curAcc = Input.acceleration;
		// update average value:
		avrgAcc = Vector3.Lerp(avrgAcc, curAcc, avrgTime * Time.deltaTime);
		// calculate peak size:
		curAcc -= avrgAcc;
		// variable peak is zero when no peak detected...
		int peak = 0;
		// or +/- 1 according to the peak polarity:
		if (curAcc.y > peakLevel) peak = 1;
		if (curAcc.y < -peakLevel) peak = -1;
		// do nothing if peak is the same of previous frame:
		if (peak == lastPeak)
			return false;
		// peak changed state: process it
		lastPeak = peak; // update lastPeak
		if (peak != 0)
		{ // if a peak was detected...
			timer = 0; // clear end count timer...
			if (peak > 0) // and increment corresponding count
				countPos++;
			else
				countNeg++;
			if (!counting)
			{ // if it's the first peak...
				counting = true; // start shake counting
				firstPeak = peak; // save the first peak direction
			}
		}
		else // but if no peak detected...
		if (counting)
		{ // and it was counting...
			timer += Time.deltaTime; // increment timer
			if (timer > endCountTime)
			{ // if endCountTime reached...
				counting = false; // finish counting...
				shakeDir = firstPeak; // inform direction of first shake...
				if (countPos > countNeg) // and return the higher count
					shakeCount = countPos;
				else
					shakeCount = countNeg;
				// zero counters and become ready for next shake count
				countPos = 0;
				countNeg = 0;
				return true; // count finished
			}
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		sendTimer = sendTimer + Time.deltaTime;

		ShakeDetector();
		if (counting && sendTimer > sendLimit)
		{
			clientNetworker.WordOut(WordActionGenerator.WordAction.Shake);
			sendTimer = 0;
		}

		if (Input.GetKeyUp("space")) {
			clientNetworker.WordOut(WordActionGenerator.WordAction.Shake);
		}

	}
}
