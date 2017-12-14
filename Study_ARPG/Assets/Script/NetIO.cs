using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
public class NetIO  {
	private static NetIO instance;
	private Socket socket;
	private string ip="127.0.0.1";
	private int port = 6650;
	public static NetIO Instance{
		get{
			if(instance==null){
				instance = new NetIO();
			}
			return instance;
		}
	}

	private NetIO(){
		try{
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(ip,port);
		}catch(Exception e){
			Debug.Log(e.Message);
		}
		

	}
}
