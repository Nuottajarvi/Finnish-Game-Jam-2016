﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyController : MonoBehaviour {

	public GameObject lobbyPlayer;
	public Text connectedPlayersText;

	public void AddPlayer(string name){
		/*GameObject instantiatedPlayer = GameObject.Instantiate(lobbyPlayer, Vector3.zero, Quaternion.identity) as GameObject;
		instantiatedPlayer.transform.parent = transform;

		Text text = instantiatedPlayer.transform.GetChild(0).GetComponent<Text>();
		text.text = name;*/
		connectedPlayersText.text = connectedPlayersText.text + name + "\n";
	}

	public void StartGame()
	{
		Application.LoadLevel("game");

	}

}
