/* ==============================================================================
2  * 功能描述：SerializeUtil  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/14 20:09:23
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NetFrame
{
    /// <summary>
    /// SerializeUtil
    /// </summary>
    public class SerializeUtil
    {
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] encode(object value)
        {
            MemoryStream ms = new MemoryStream();//创建编码解码的内存刘对象 
            BinaryFormatter bw = new BinaryFormatter();//二进制序列化对象
            //将obj对象序列化成二进制数据写入到内存刘
            bw.Serialize(ms,value);
            byte[] result = new byte[ms.Length];
            //将流数据拷贝到结果数组
            Buffer.BlockCopy(ms.GetBuffer(),0,result,0,(int)ms.Length);
            ms.Close();
            
            return result;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object decode(byte[] value)
        {
            MemoryStream ms = new MemoryStream(value);//创建编码解码的内存刘对象     并将需要反序列化的数据写入其中
            BinaryFormatter bw = new BinaryFormatter();//二进制序列化对象
            //将obj对象序列化成二进制数据写入到内存刘
            object result = bw.Deserialize(ms);
            ms.Close();


            return result;
        }
    }
}