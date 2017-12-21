/* ==============================================================================
2  * 功能描述：EventUtil  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 23:23:17
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 创建选人模块事件
/// </summary>
/// <param name="teamOne"></param>
/// <param name="teamTwo"></param>
public delegate void CreateSelect(List<int> teamOne,List<int>teamTwo);
/// <summary>
/// 移除选人模块事件 选人房间关闭
/// </summary>
/// <param name="roomId"></param>
public delegate void DestroySelect(int roomId);
namespace LOLServer.tool
{
    /// <summary>
    /// 模块通知别的模块
    /// </summary>
    public class EventUtil
    {
        public static CreateSelect createSelect;
        public static DestroySelect destroySelect;
    }
}