using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.IO;
public  class NetIO  {
	
	//单例对象
	private static NetIO instance;
	private Socket socket;
	private string ip="127.0.0.1";
	private int port = 6650;
	private byte[] readbuff = new byte[1024];
	List<byte> cache = new List<byte>();
	private bool isReading =false;
	public static NetIO Instance{
		get{
			if(instance==null){
				instance = new NetIO();
			}
			return instance;
		}
	}
	public List<SocketModel> messages = new List<SocketModel>();

	private NetIO(){
		try{
			//创建客户端连接对象
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//连接到服务器
			socket.Connect(ip,port);
			//开启异步消息接受 消息到达后会直接写入缓冲区readbuff
			socket.BeginReceive(readbuff,0,1024,SocketFlags.None,ReceiveCallBack,readbuff);
		}catch(Exception e){
			Debug.Log(e.Message);
		}
		
	}
	//收到消息回调
	private void ReceiveCallBack(IAsyncResult ar){
		try{
			//获取当前收到的信息长度()
			int length = socket.EndReceive(ar);
			byte[] message = new byte[length];
			Buffer.BlockCopy(readbuff,0,message,0,length);
			cache.AddRange(message);
			if(!isReading){
				isReading = true;
				OnData();
			}
			//尾递归 再次 开启异步消息接受 消息到达后会直接写入缓冲区readbuff
			socket.BeginReceive(readbuff,0,1024,SocketFlags.None,ReceiveCallBack,readbuff);		
			
		}catch(Exception e){
			Debug.Log("远程服务器主动断开连接");
			socket.Close();
		}
		
	}
	public void Write(byte type,int area,int command,object message){
			ByteArray ba = new ByteArray();
            ba.write(type);
            ba.write(area);
            ba.write(command);
            //判断消息体是否为空，不为空则序列化写入
            if (message != null)
            {
                ba.write(SerializeUtil.encode(message));
            }
			ByteArray arr1 = new ByteArray();
			arr1.write(ba.Length);
			arr1.write(ba.getBuff());
			try{
				socket.Send(arr1.getBuff());
			}catch(Exception e){
				Debug.Log("网络错误请重新登陆"+e.Message);
			}

	}
	
	  /// <summary>
        /// 缓存中有数据处理
        /// </summary>
        void OnData()
        {
			//长度解码
			byte[] result = decode(ref cache);
			//长度解码返回空，说明消息体补全，等待下条消息过来补全
			if(result==null){
				isReading = false;
				return;
			}
			SocketModel message = MDecode(result);
			if(message==null){
				isReading = false;
				return;
			}
			//进行消息的处理
			messages.Add(message);
            //尾递归，防止在消息处理过程中，有其他消息到达而没有接受处理
            OnData();
        }
		/// <summary>
        /// 粘包长度解码
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static byte[] decode(ref List<byte> cache)
        {
            if (cache.Count < 4)
            {
                return null;
            }
            //创建内存刘数据，并将缓存数据写入进去
            MemoryStream ms = new MemoryStream(cache.ToArray());
            //二进制读取流
            BinaryReader br = new BinaryReader(ms);
            //从缓存中读取int型消息长度
            int length = br.ReadInt32();
            //如果消息体长度大于缓存中数据长度，说明消息还没有读取完，等待下次消息到达后再次处理
            if (length > ms.Length - ms.Position)
            {
                return null;
            }
            //读取正确长度数据
            byte[] result = br.ReadBytes(length);
            //清空缓存
            cache.Clear();
            //将读取后的剩余数据写入缓存
            cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
            br.Close();
            ms.Close();
            return result;


        }
		 /// <summary>
        /// 消息体反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SocketModel MDecode(byte[] value)
        {
            ByteArray ba = new ByteArray(value);
            SocketModel model = new SocketModel();
            byte type;
            int area;
            int command;
            //从数据流中读取三层协议
            ba.read(out type);
            ba.read(out area);
            ba.read(out command);
            model.type = type;
            model.area = area;
            model.command = command;
            //判断读取完协议后，是否还有数据需要读取，是则说明有消息体。进行读取
            if (ba.Readnable)
            {
                byte[] message;
                //将剩余数据全部读取
                ba.read(out message,ba.Length-ba.Position);
                //反序列化数据
                model.message = SerializeUtil.decode(message);
            }
            ba.Close();
            return model;
        }
		
}
