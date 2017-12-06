using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;
namespace NetFrame
{
    ///<summary>
    ///用户连接对象
    ///<summary>
    
    public class UserToken{
        /// <summary>
        /// 用户连接
        /// </summary>
        public Socket conn;
        /// <summary>
        /// 用户异步接受网络数据对象
        /// </summary>
        public SocketAsyncEventArgs receiveSAEA;
		/// <summary>
		/// 用户异步发送网络数据对象
		/// </summary>
		public SocketAsyncEventArgs sendSAEA;
        public UserToken(){
            this.sendSAEA = new SocketAsyncEventArgs();
            this.receiveSAEA = new SocketAsyncEventArgs();
            receiveSAEA.UserToken = this;
            sendSAEA.UserToken = this;
        }
        public void Receive(byte[] buff){
            
        }
        public void Close(){
            try{
				conn.Shutdown(SocketShutdown.Both);
				conn.Close();
				conn = null;
            }catch(Exception e){
                Console.WriteLine(e.Message);
            }

        }
        public void Writed(){
            
        }
    }   
}
 