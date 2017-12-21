/* ==============================================================================
2  * 功能描述：SelectModel  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/21 9:15:03
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.DTO
{
    /// <summary>
    /// SelectModel
    /// </summary>
    [Serializable]
    public class SelectModel
    {
        public int userId;//用户id
        public string name;//用户名称
        public int hero;//所选英雄
        public bool enter;//是否进入
        public bool ready;//是否已准备
    }
}