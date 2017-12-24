﻿using System;
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using GameProtocol.DTO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectHandler : MonoBehaviour, IHandler
{
    private SelectRoomDTO room;
    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case SelectProtocol.DESTROY_BRO:
                SceneManager.LoadScene(1);
                break;
            case SelectProtocol.ENTER_SRES:
                enter(model.GetMessage<SelectRoomDTO>());
                break;
            case SelectProtocol.ENTER_EXBRO:
                enter(model.GetMessage<int>());
                break;
            case SelectProtocol.FIGHT_BRO:
                SceneManager.LoadScene(3);
                break;
            case SelectProtocol.READY_BRO:
                ready(model.GetMessage<SelectModel>());
                break;
            case SelectProtocol.SELECT_BRO:
                select(model.GetMessage<SelectModel>());
                break;
            case SelectProtocol.SELECT_SRES:
                WarningManager.errors.Add(new WarningModel("角色选择失败，清重新选择"));
                break;
            case SelectProtocol.TALK_BRO:
                talk(model.GetMessage<string>());
                break;        
        }
    }

    private void ready(SelectModel model)
    {
        if (model.userId == GameData.user.id)
        {
            //是自己准备了，进行状态处理不准点击操作了
            SelectEventUtil.selected();
        }
        foreach (SelectModel item in room.teamOne)
        {
            if (item.userId == model.userId)
            {
                item.hero = model.hero;
                item.ready = true;
                //刷新UI界面
                SelectEventUtil.refreshView(room);
                return;
            }
        }
        foreach (SelectModel item in room.teamTwo)
        {
            if (item.userId == model.userId)
            {
                item.hero = model.hero;
                item.ready = true;
                //刷新UI界面
                SelectEventUtil.refreshView(room);
                return;
            }
        }
    }
    private void select(SelectModel model)
    {
        foreach (SelectModel item in room.teamOne)
        {
            if (item.userId == model.userId)
            {
                item.hero = model.hero;
                //刷新UI界面
                SelectEventUtil.refreshView(room);
                return;
            }
        }
        foreach (SelectModel item in room.teamTwo)
        {
            if (item.userId == model.userId)
            {
                item.hero = model.hero;
                //刷新UI界面
                SelectEventUtil.refreshView(room);
                return;
            }
        }
    }
    private void talk(string value)
    {
        //向聊天面板添加信息
        transform.GetComponent<SelectScreen>().TalkShow(value);
    }
    private void enter(SelectRoomDTO dto)
    {
        transform.GetComponent<SelectScreen>().CloseMask();
        room = dto;
        //刷新界面UI
        SelectEventUtil.refreshView(room);
    }

    private void enter(int id)
    {
        if (room == null)
            return;
        foreach (SelectModel item in room.teamOne)
        {
            if (item.userId == id)
            {
                item.enter = true;
                //刷新UI界面
                SelectEventUtil.refreshView(room);
                return;
            }
        }
        foreach (SelectModel item in room.teamTwo)
        {
            if (item.userId == id)
            {
                item.enter = true;
                //刷新UI界面
                SelectEventUtil.refreshView(room);
                return;
            }
        }
    }
}
