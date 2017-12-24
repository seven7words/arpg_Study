using System;
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchHandler : MonoBehaviour, IHandler
{
    public void MessageReceive(SocketModel model)
    {
        if (model.command == MatchProtocol.ENTER_SELECT_BRO)
        {
            SceneManager.LoadScene(2);
        }
    }
}
