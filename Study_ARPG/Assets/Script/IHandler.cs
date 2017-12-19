/* ==============================================================================
2  * 功能描述：IHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/19 19:15:42
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    /// <summary>
    /// IHandler
    /// </summary>
    public interface IHandler
    {
        void MessageReceive(SocketModel model);
    }
