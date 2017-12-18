using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension  {

	//扩展monobehaviour发送消息方法
	public static void Write(this MonoBehaviour mono,byte type,int area,int command,object message){
		NetIO.Instance.Write(type,area,command,message);
	}
}
