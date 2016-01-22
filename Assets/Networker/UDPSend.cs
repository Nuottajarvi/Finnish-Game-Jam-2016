using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SimpleJSON;

public class UDPSend : MonoBehaviour{
	// prefs
	public int port;
	public bool log;

	IPEndPoint remoteEndPoint;
	UdpClient client;

	public void Awake(){
		client = new UdpClient();
	}

	void Start(){
		remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);
	}
		
	//Optional parameter index
	private void SendString(string message){
		try
		{
			byte[] data = Encoding.UTF8.GetBytes(message);
			client.Send(data, data.Length, remoteEndPoint);

			Debug.Log(remoteEndPoint);

			if(log) Debug.Log(message);
		}
		catch (Exception err)
		{
			print(err.ToString());
		}
	}

	public void Send(JSONNode data){
		SendString(data.ToString());
	}

	void onDispose(){
		client.Close();
	}
}