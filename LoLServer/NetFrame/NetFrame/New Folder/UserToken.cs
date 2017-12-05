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
    }   
}
 