using UnityEngine;
using System.Collections;

public class Swipe : PhoneAction {

	// Use this for initialization
	void Start()
	{
		sendTimer = 0.0f;
	}

	// Update is called once per frame
	void Update () {

		sendTimer = sendTimer + Time.deltaTime;


        //A swipe that doesn't move enough is considered as a tap on screen
        if (Input.touchCount > 0)
        {
            //Use the first swipe
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                //Start tracking a swipe
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                //End swipe, check if swipe had enough distance
                //If not, consider the event as a tap
                case TouchPhase.Ended:
                    float swipeDist = (new Vector3(touch.position.x, touch.position.y, 0) - new Vector3(startPos.x, startPos.y, 0)).magnitude;
                    if (swipeDist > swipeTreshold)
                    {
						//SEND SWIPE
						if (sendTimer > sendLimit)
						{
							sendTimer = 0;
							clientNetworker.WordOut(WordActionGenerator.WordAction.Swipe);
						}
                        
                    }
                    break;
            }
        }
    }
}
