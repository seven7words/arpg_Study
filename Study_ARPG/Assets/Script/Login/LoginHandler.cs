using System;
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using UnityEngine;

public class LoginHandler : MonoBehaviour, IHandler
{
    public void MessageReceive(SocketModel model)
    {
        WarningManager.errors.Add(new WarningModel(model.message+""));
        switch (model.command)
        {
            case LoginProtocol.LOGIN_SRES:
                //SendMessage("openLoginBtn");
                transform.parent.GetComponent<LoginScreen>().openLoginBtn();
                break;
        }
    }
}
