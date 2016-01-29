using UnityEngine;
using System.Collections;

/***********************/
// This class is used for keeping track if player completes any wordactions
/***********************/

public class ClientWordActionTracker : MonoBehaviour {

    public bool tapping;
    public bool shaking;
    public bool swiping;

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

    /* Start swipe detection variables */
    public float minSwipeDist = 1;
    private Vector2 startPos;
    /* end swipe detectoion variables */

    //Pre-made function for detecting the shakes
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


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //Detect the shaking
        ShakeDetector();
        if (counting)
        {
            shaking = true;
        }
        else {
            shaking = false;
        }

        //Detect the swipe and tap
        //A swipe that doesn't move enough is considered as a tap on screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    swiping = false;
                    tapping = false;
                    break;
                case TouchPhase.Ended:
                    float swipeDist = (new Vector3(touch.position.x, touch.position.y, 0) - new Vector3(startPos.x, startPos.y, 0)).magnitude;
                    if (swipeDist > minSwipeDist)
                    {
                        swiping = true;
                    }
                    else {
                        tapping = true;
                    }
                    break;
            }
        }
        else {
            swiping = false;
            tapping = false;
        }



    }
}
