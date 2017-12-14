using System;
using System.Collections.Generic;

namespace NetFrame
{

    public class MessageEncoding
    {
        /// <summary>
        /// ��Ϣ�����л�
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Encode(object value)
        {
            SocketModel model = value as SocketModel;
            ByteArray ba = new ByteArray();
            ba.write(model.type);
            ba.write(model.area);
            ba.write(model.command);
            //�ж���Ϣ���Ƿ�Ϊ�գ���Ϊ�������л�д��
            if (model.message != null)
            {
                ba.write(SerializeUtil.encode(model.message));
            }
            
            byte[] result = ba.getBuff();
            ba.Close();
            return result;

        }
        /// <summary>
        /// ��Ϣ�巴���л�
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Decode(byte[] value)
        {
            ByteArray ba = new ByteArray(value);
            SocketModel model = new SocketModel();
            byte type;
            int area;
            int command;
            //���������ж�ȡ����Э��
            ba.read(out type);
            ba.read(out area);
            ba.read(out command);
            model.type = type;
            model.area = area;
            model.command = command;
            //�ж϶�ȡ��Э����Ƿ���������Ҫ��ȡ������˵������Ϣ�塣���ж�ȡ
            if (ba.Readnable)
            {
                byte[] message;
                //��ʣ������ȫ����ȡ
                ba.read(out message,ba.Length-ba.Position);
                //�����л�����
                model.message = SerializeUtil.decode(message);
            }
            ba.Close();
            return model;
        }
    }
}
