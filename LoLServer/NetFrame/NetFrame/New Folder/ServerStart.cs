using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;
namespace NetFrame{
    public class ServerStart{
        Socket server;//服务器socket监听对象
        int maxClient;//最大客户端连接数
        Semaphore acceptClients;
        UserTokenPool pool;
        ///<summary>
        ///初始化通信监听
        ///<summary>
        ///<param name="port">监听端口</param>
        public ServerStart(int max){
            //实例化监听对象
            server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            //设定服务器最大连接人数
            maxClient = max;
            //创建连接池
            pool = new UserTokenPool(max);
            //连接信号量
            acceptClients = new Semaphore(max, max);
            for (int i = 0; i < max;i++){
                UserToken token = new UserToken();
                //初始化token信息

                token.receiveSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

				token.sendSAEA.Completed +=new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                pool.Push(token);
            }

        }
        public void Start(int port){
            //监听当前服务器网卡所有可用ip地址的port端口
            //外网ip 内网ip192.168.x.x 本机ip127.0.0.1
            server.Bind(new IPEndPoint(IPAddress.Any,port));
            //置于监听状态
            server.Listen(10);
            StartAccept((null));
        }
        /// <summary>
        /// 开始客户端连接监听
        /// </summary>
        public void StartAccept(SocketAsyncEventArgs e){
            //如果当前传入为空，说明调用新的客户端连接监听事件，否则的话移除当前客户端连接
            if(e==null){
                e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);

            }else{
                e.AcceptSocket = null;
            }
            //信号量-1
            acceptClients.WaitOne();
            bool result = server.AcceptAsync(e);
			//判断异步事件是否挂起 没挂起说明立刻执行完成，直接处理事件
			//否则会在处理完成后出发Accept_Completed
			if(!result){
                ProcessAccept(e);
            }
        }
		public void ProcessAccept(SocketAsyncEventArgs e)
		{
            //从连接对象池取出连接对象 供新用户使用
            UserToken token = pool.Pop();
            token.conn = e.AcceptSocket;
            //TODO:通知应用层，有客户端连接
            //开启消息到达监听
            StartReceive(token);
            //释放当前异步对象
            StartAccept(e);
		}
        public void Accept_Completed(object sender,SocketAsyncEventArgs e){
            ProcessAccept(e);
        }
        public void StartReceive(UserToken token){
            //用户连接对象开启异步数据接受
            bool result=token.conn.ReceiveAsync(token.receiveSAEA);
            //异步事件是否挂起
            if(!result){
                ProcessReceive(token.receiveSAEA); 
            }
        }

		public void IO_Completed(object sender, SocketAsyncEventArgs e)
		{
            if(e.LastOperation==SocketAsyncOperation.Receive){
                ProcessReceive(e);
            }else{
                ProcessAccept(e);
            }
			ProcessAccept(e);
		}
        public void ProcessReceive(SocketAsyncEventArgs e){
            UserToken token = e.UserToken as UserToken;
            if(token.receiveSAEA.BytesTransferred>0&&token.receiveSAEA.SocketError==SocketError.Success){
                byte[] message =new byte[token.receiveSAEA.BytesTransferred];
                Buffer.BlockCopy(token.receiveSAEA.Buffer,0,message,0,token.receiveSAEA.BytesTransferred);
                //处理接收到的消息
                token.Receive(message);
                StartReceive(token);

            }else{
                if(token.receiveSAEA.SocketError!=SocketError.Success){
                    ClientClose(token,token.receiveSAEA.SocketError.ToString());
                }else{
                    ClientClose(token,"客户端主动断开连接");
                }
            }
        }
		public void ProcessSend(SocketAsyncEventArgs e)
		{
            UserToken token = e.UserToken as UserToken;
            if(e.SocketError!=SocketError.Success){
                ClientClose(token, e.SocketError.ToString());
            }else{
                //消息发送成功，回调成功
                token.Writed();
            }
		}
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="token">Token.断开连接的用户对象</param>
        /// <param name="error">Error.断开连接的错误编码</param>
        public void ClientClose(UserToken token,string error){
            if(token.conn!=null){
                lock(token){
                    //通知应用层面客户端断开连接
                    token.Close();
                    //加回一个信号量，供其他用户使用
                    pool.Push(token);
                    acceptClients.Release();
                }
            }
        }
    }   
}
 