using UnityEngine;
using System.Collections;

public class PhoneAction : MonoBehaviour {

    public ClientNetworker clientNetworker;

    public int swipeTreshold = 100;

    //Variable to hold start position of a touch: used by tap and swipe
    protected Vector2 startPos;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
