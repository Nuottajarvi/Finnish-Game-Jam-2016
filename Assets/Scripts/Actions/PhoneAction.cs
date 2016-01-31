using UnityEngine;
using System.Collections;

public class PhoneAction : MonoBehaviour {

    public ClientNetworker clientNetworker;

    protected int swipeTreshold = 100;

    //Variable to hold start position of a touch: used by tap and swipe
    protected Vector2 startPos;

	//Send timer is used to limit the amount of actions sent
	protected float sendTimer;
	protected float sendLimit;

	// Use this for initialization
	/*void Start () {
		sendTimer = 0;
		//time between shortest send in seconds
		sendLimit = 1.0f;
	}*/
}
