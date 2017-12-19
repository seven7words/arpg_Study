using System;
using System.Collections.Generic;
using System.IO.Pipes;
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
        List<byte> cache = new List<byte>();
        public LengthEncode LE;
        public LengthDecode LD;
        public encode encode;
        public decode decode;
        private bool isReading = false;
        private bool isWriting = false;
        public AbsHandlerCenter center;
        Queue<byte[]> writeQueue = new Queue<byte[]>();
        public delegate void SendProcess(SocketAsyncEventArgs e);
        public delegate void ClientProcess(UserToken token,string error);

        public ClientProcess closeProcess;
        public SendProcess sendProcess;
        public UserToken() {
           receiveSAEA = new SocketAsyncEventArgs();
           sendSAEA = new SocketAsyncEventArgs();
           receiveSAEA.UserToken = this;
           sendSAEA.UserToken = this;
            //设置接收对象的缓冲区大小
            receiveSAEA.SetBuffer(new byte[1024], 0, 1024);
        }
       //网络消息到达
       public void receive(byte[] buff) {
            //将消息写入缓存
            cache.AddRange(buff);
           if (!isReading)
           {
               isReading = true;
               OnData();
               
           }
       }
        /// <summary>
        /// 缓存中有数据处理
        /// </summary>
        void OnData()
        {
            //解码消息存储对象
            byte[] buff = null;
            //当粘包解码器存在的时候进行粘包处理
            if (LD != null)
            {
                buff = LD(ref cache);
                //消息未接受全，退出数据处理，等待下次消息到达
                if (buff == null)
                {
                    isReading = false;
                    return;
                }
            }
            else
            {
                //缓存区中没有数据，直接跳出数据处理，等待下次消息到达
                if (cache.Count == 0)
                {
                    isReading = false;
                    return;
                }
                buff = cache.ToArray();
                cache.Clear();
                isReading = true;
            }

            //反序列化方法是否存在
            if (decode == null)
            {
                throw new Exception("message decode process is null");
            }
            //进行消息反序列化
            object message = decode(buff);
            //TODO:通知应用层有消息到达
            center.MessageReceive(this,message);
            //尾递归，防止在消息处理过程中，有其他消息到达而没有接受处理
            OnData();
        }
    
       public void write(byte[] value) {
           if (conn == null)
           {
                //此链接已经断开了
               closeProcess(this, "调用已经断开的链接");
               return;
           }
           writeQueue.Enqueue(value);
           if (!isWriting)
           {
               isWriting = true;
                onWrite();
           }
       }

        public void onWrite()
        {
            //判断发送消息队列是否有消息
            if (writeQueue.Count == 0)
            {
                isWriting = false;
                return;
            }
            //取出第一条待发消息
            byte[] buff = writeQueue.Dequeue();
            //设置消息发送异步对象的发送数据缓冲区数据
            sendSAEA.SetBuffer(buff,0,buff.Length);
            //开启异步发送
            bool result = conn.SendAsync(sendSAEA);
            //是否挂起
            if (!result)
            {
                sendProcess(sendSAEA);
            }
        }

       public void writed() {
            //与ondata尾递归同理
           onWrite();
       }
       public void Close() {
           try
           {
              writeQueue.Clear();
                cache.Clear();
               isReading = false;
               isWriting = false;
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
