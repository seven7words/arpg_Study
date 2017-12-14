using System;
using System.Collections.Generic;
using System.IO;
namespace NetFrame
{
    public class LengthEncoding
    {
        /// <summary>
        /// ճ�����볤��
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static byte[] encode(byte[] buff)
        {
            MemoryStream ms = new MemoryStream();//�����ڴ�������
            BinaryWriter sw = new BinaryWriter(ms);//д������ƶ�����
            //д����Ϣ����
            sw.Write(buff.Length);
            //д����Ϣ��
            sw.Write(buff);
            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
            sw.Close();
            ms.Close();
            return result;
        }
        /// <summary>
        /// ճ�����Ƚ���
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static byte[] decode(ref List<byte> cache)
        {
            if (cache.Count < 4)
            {
                return null;
            }
            //�����ڴ������ݣ�������������д���ȥ
            MemoryStream ms = new MemoryStream(cache.ToArray());
            //�����ƶ�ȡ��
            BinaryReader br = new BinaryReader(ms);
            //�ӻ����ж�ȡint����Ϣ����
            int length = br.ReadInt32();
            //�����Ϣ�峤�ȴ��ڻ��������ݳ��ȣ�˵����Ϣ��û�ж�ȡ�꣬�ȴ��´���Ϣ������ٴδ���
            if (length > ms.Length - ms.Position)
            {
                return null;
            }
            //��ȡ��ȷ��������
            byte[] result = br.ReadBytes(length);
            //��ջ���
            cache.Clear();
            //����ȡ���ʣ������д�뻺��
            cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
            br.Close();
            ms.Close();
            return result;


        }
    }
}
