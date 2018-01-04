using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using UnityEngine;

public class FightScreen : MonoBehaviour
{

    // Use this for initialization
    void Start () {
		this.WriteMessage(GameProtocol.Protocol.TYPE_FIGHT,0,FightProtocol.ENTER_CREQ,null);
	}

    
}
