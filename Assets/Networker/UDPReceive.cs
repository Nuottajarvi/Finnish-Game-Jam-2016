/*
 
    -----------------------
    UDP-Receive (send to)
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
   
    // > receive
    // 127.0.0.1 : 8051
   
    // send
    // nc -u 127.0.0.1 8051
 
*/
using UnityEngine;
using System.Collections.Generic;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour {

	Thread receiveThread;
	UdpClient client;

	public int port;

	public bool log;

	List<string> receivedUDPPackets;

	void Awake(){
		receivedUDPPackets = new List<string>();
		receiveThread = new Thread(
			new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}
		
	private void ReceiveData(){

		client = new UdpClient(port); //Binds udp client to random port
		while (true)
		{

			try
			{
				
				IPEndPoint IPEndPoint = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref IPEndPoint);

				string text = Encoding.UTF8.GetString(data);

				if(log){
					Debug.Log(text);
				}

				lock(receivedUDPPackets){
					receivedUDPPackets.Add(text);
				}
			}
			catch (Exception err)
			{
				print(err.ToString());
			}
		}
	}
		
	public string[] GetLatestUDPPackets(){
		lock(receivedUDPPackets){
			string[] udpPacketArray = receivedUDPPackets.ToArray();
			receivedUDPPackets.Clear();
			return udpPacketArray;	
		}
	}

	void OnDisable(){ 
		if (receiveThread!= null) 
			receiveThread.Abort();
		client.Close(); 
	} 
}