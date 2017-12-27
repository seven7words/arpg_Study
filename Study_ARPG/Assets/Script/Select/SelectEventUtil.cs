using System.Collections;
using System.Collections.Generic;
using GameProtocol.DTO;
using UnityEngine;

public delegate void CallBack();
public delegate void Refresh(SelectRoomDTO room);

public delegate void SelectHero(int id);
public class SelectEventUtil
{

    public static CallBack selected;
    public static Refresh refreshView;
    public static SelectHero selectHero;
}
