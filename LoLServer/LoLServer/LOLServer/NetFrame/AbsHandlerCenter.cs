using System;
using System.Collections.Generic;
using NetFrame;

namespace NetFrame
{

    public abstract class AbsHandlerCenter
    {
        /// <summary>
        /// �ͻ�������
        /// </summary>
        /// <param name="token">���ӵĿͻ��˶���</param>
        public abstract void ClientConnect(UserToken token);
        /// <summary>
        /// �յ��ͻ�����Ϣ
        /// </summary>
        /// <param name="token">������Ϣ�ĵĿͻ��˶���</param>
        /// <param name="message">��Ϣ����</param>
        public abstract void MessageReceive(UserToken token, object message);
        /// <summary>
        /// �ͻ��˶Ͽ�����
        /// </summary>
        /// <param name="token">�Ͽ��Ŀͻ��˶���</param>
        /// <param name="error">�Ͽ��Ĵ�����Ϣ</param>
        public abstract void ClientClose(UserToken token, string error);
    }
}
