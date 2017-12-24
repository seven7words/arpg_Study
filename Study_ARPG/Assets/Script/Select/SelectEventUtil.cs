using System.Collections;
using System.Collections.Generic;
using GameProtocol.DTO;
using UnityEngine;

public delegate void CallBack();
public delegate void Refresh(SelectRoomDTO room);
public class SelectEventUtil
{

    public static CallBack selected;
    public static Refresh refreshView;
}
