/* ==============================================================================
2  * 功能描述：AbsFightModel  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/28 21:17:02
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.DTO.fight
{
    /// <summary>
    /// AbsFightModel
    /// </summary>
    [Serializable]
    public class AbsFightModel
    {
        public int id;//战斗区域中唯一识别码
        public ModelType type;//标识当前生命体是属于什么类别的
        public int code;//模型唯一识别码，战斗中会有多个相同的兵种出现，只用于表示形象及对应的数组
        public int hp;//当前血量
        public int maxHp;//最大血量
        public int atk;//攻击
        public int def;//防御
        public string name;//名称
        public float speed;//移动速度
        public float atkSpeed;//攻击速度
        public float atkRange;//攻击范围
        public float eyeRange;//视野范围

    }
}