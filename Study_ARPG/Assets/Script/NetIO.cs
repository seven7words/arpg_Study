using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
public class NetIO  {
	//单例对象
	private static NetIO instance;
	private Socket socket;
	private string ip="127.0.0.1";
	private int port = 6650;
	private byte[] readbuff = new byte[1024];
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
			//创建客户端连接对象
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//连接到服务器
			socket.Connect(ip,port);
			//开启异步消息接受
			socket.BeginReceive(readbuff,0,1024,SocketFlags.None,ReceiveCallBack,readbuff);
		}catch(Exception e){
			Debug.Log(e.Message);
		}
		
	}
	//收到消息回调
	private void ReceiveCallBack(IAsyncResult ar){
		
	}
}
