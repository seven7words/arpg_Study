/* ==============================================================================
2  * 功能描述：SocketModel  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/14 20:07:09
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFrame
{
    /// <summary>
    /// SocketModel
    /// </summary>
    public class SocketModel
    {
       
            /// <summary>
            /// 一级协议 用于区分所属模块
            /// </summary>
            public byte type
            {
                get;
                set;
            }
            /// <summary>
            /// 二级协议 用于区分模块下所属子模块
            /// </summary>
            public int area { get; set; }
            /// <summary>
            /// 三级协议， 用于区分当前处理逻辑功能
            /// </summary>
            public int command { get; set; }
            /// <summary>
            /// 消息体，当前需要处理的主体数据
            /// </summary>
            public object message { get; set; }

            public SocketModel()
            {

            }

            public SocketModel(byte t, int a, int c, object o)
            {
                this.type = t;
                this.area = a;
                this.command = c;
                this.message = o;
            }

            public T GetMessage<T>()
            {
                return (T)message;
            }
        }
    }
