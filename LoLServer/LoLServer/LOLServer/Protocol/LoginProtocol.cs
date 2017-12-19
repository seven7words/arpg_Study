/* ==============================================================================
2  * 功能描述：LoginProtocol  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/18 20:25:38
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    /// <summary>
    /// 登陆协议
    /// </summary>
    public class LoginProtocol
    {
        public const int LOGIN_CREQ = 0;//客户端申请登陆
        public const int LOGIN_SRES = 1;//服务器反馈给客户端，登陆成功
        public const int REG_CREQ = 2;//客户端申请注册
        public const int REG_SRES = 3;//服务器反馈给客户端 注册结果
    }
}