/* ==============================================================================
2  * 功能描述：MatchRoom  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 22:17:09
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLServer.logic.match
{
    /// <summary>
    /// 战斗匹配房间模型
    /// </summary>
    public class MatchRoom
    {
        public int id;//房间唯一id
        public int teamMax = 1;//每只队伍需要匹配到的人数
        public List<int> teamOne = new List<int>();//队伍一 人员id
        public List<int> teamTwo = new List<int>();//队伍二 人员id

    }
}