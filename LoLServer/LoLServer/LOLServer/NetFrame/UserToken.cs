using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    /// <summary>
    /// 用户连接信息对象
    /// </summary>
   public class UserToken
    {
       /// <summary>
       /// 用户连接
       /// </summary>
       public Socket conn;
       //用户异步接收网络数据对象
       public SocketAsyncEventArgs receiveSAEA;
       //用户异步发送网络数据对象
       public SocketAsyncEventArgs sendSAEA;

       public UserToken() {
           receiveSAEA = new SocketAsyncEventArgs();
           sendSAEA = new SocketAsyncEventArgs();
           receiveSAEA.UserToken = this;
           sendSAEA.UserToken = this;
       }
       //网络消息到达
       public void receive(byte[] buff) {
           //将消息写入缓存
       }


       public void write(byte[] value) {
          
       }


       public void writed() {
           //与onData尾递归同理
       }
       public void Close() {
           try
           {
              
               conn.Shutdown(SocketShutdown.Both);
               conn.Close();
               conn = null;
           }
           catch (Exception e) {
               Console.WriteLine(e.Message);
           }
       }
    }
}
