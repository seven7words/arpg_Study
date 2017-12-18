using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;

namespace LOLServer.logic
{
    public interface IHandleInterface
    {
         void ClientClose(UserToken token, string error);


         void ClientConnect(UserToken token);

        void MessageReceive(UserToken token, SocketModel message);
    }
}
