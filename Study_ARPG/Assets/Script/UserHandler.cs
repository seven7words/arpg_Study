/* ==============================================================================
2  * 功能描述：UserHandler  
3  * 创 建 者：seven_words
4  * 创建日期：2017/12/20 19:25:39
5  * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameProtocol;
using GameProtocol.DTO;
using UnityEngine.TestTools;
using UnityEngine;

/// <summary>
/// UserHandler
/// </summary>
public class UserHandler : MonoBehaviour, IHandler
{
    private MainScreen main;

    void Awake()
    {
        main = transform.parent.GetComponent<MainScreen>();
    }
    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case UserProtocol.INFO_SRES:
                info(model.GetMessage<UserDTO>());
                break;
            case UserProtocol.CREATE_SRES:
                create(model.GetMessage<bool>());
                break;
            case UserProtocol.ONLINE_SRES:
                online(model.GetMessage<UserDTO>());
                break;
                
        }
    }

    private void info(UserDTO user)
    {
        if (user == null)
        {
            //显示创建面板
           main.OpenCreate();
        }
        else
        {
            //向服务器申请登陆
            this.WriteMessage(Protocol.TYPE_USER,0,UserProtocol.ONLINE_CREQ,null);

        }
        
    }

    private void online(UserDTO user)
    {
        GameData.user = user;
        //移除遮罩
        main.CloseMask();
        //刷新UI数据
        main.RefreshView();
        WarningManager.errors.Add(new WarningModel("登陆成功"));
    }

    private void create(bool value)
    {
        if (value)
        {
            WarningManager.errors.Add(new WarningModel("创建成功", delegate
            {
                //关闭创建面板
                main.CloseCreate();
                //直接申请登陆
                this.WriteMessage(Protocol.TYPE_USER, 0, UserProtocol.ONLINE_CREQ, null);

            }));
           
        }
        else
        {
            WarningManager.errors.Add(new WarningModel("创建失败", delegate
            {
                //刷新创建面板
                main.OpenCreate();
            })); 
            
        }
    }
}
