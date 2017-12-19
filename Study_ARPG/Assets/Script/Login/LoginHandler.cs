using System;
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour, IHandler
{
    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case LoginProtocol.LOGIN_SRES:
                //SendMessage("openLoginBtn");
                login(model.GetMessage<int>());
                 break;
            case LoginProtocol.REG_SRES:
                reg(model.GetMessage<int>());
                break;
        }
    }
    /// <summary>
    /// 登陆返回处理
    /// </summary>
    private void login(int value)
    {
        transform.parent.GetComponent<LoginScreen>().openLoginBtn();
        switch (value)
        {
            case 0:
                ////加载游戏主场景
                SceneManager.LoadScene(1);
                break;
            case -1:
                WarningManager.errors.Add(new WarningModel("账号不存在"));

                break;
            case -2:
                WarningManager.errors.Add(new WarningModel("账号在线"));
                break;
            case -3:
                WarningManager.errors.Add(new WarningModel("密码错误"));
                break;
            case -4:
                WarningManager.errors.Add(new WarningModel("输入不合法"));
                break;
        }
    }
    /// <summary>
    /// 注册返回处理
    /// </summary>
    private void reg(int value)
    {
        switch (value)
        {
            case 0:
                ////加载游戏主场景
                 WarningManager.errors.Add(new WarningModel("注册成功"));
                break;
            case 1:
                WarningManager.errors.Add(new WarningModel("注册失败，账号重复"));

                break;

        }
    }
}
