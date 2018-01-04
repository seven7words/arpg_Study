
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using GameProtocol.DTO;
using GameProtocol.DTO.fight;
using UnityEngine;

public class FightHandler : MonoBehaviour,IHandler
{
    private FightRoomModel room;
    [SerializeField]
    private Transform[] position1;//队伍一建筑
    [SerializeField]
    private Transform start1;//队伍1的复活（出生点）点
    [SerializeField]
    private Transform start2;//队伍2的复活（出生点）点
    [SerializeField]
    private Transform[] position2;//队伍二建筑
    private Dictionary<int,GameObject> teamOne = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> teamTwo = new Dictionary<int, GameObject>();

    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case FightProtocol.START_BRO:
                Debug.Log("????");
                StartFight(model.GetMessage<FightRoomModel>());
                break;
        }
    }

    private void StartFight(FightRoomModel value)
    {
        room = value;
        foreach (AbsFightModel item in value.teamOne)
        {
            GameObject go;
            Debug.Log(item.code);
            if (item.type == ModelType.HUMAN)
            {
                go = Instantiate(Resources.Load<GameObject>("prefab/Player/" + item.code),start1.position+new Vector3(Random.Range(0.5f, 1.5f), 0, Random.Range(1, 7)),start1.rotation);
               
            }
            else
            {
                go = Instantiate(Resources.Load<GameObject>("prefab/build/1_" + item.code),position1[item.code-1].position,Quaternion.identity);

            }
            this.teamOne.Add(item.id, go);
        }
        foreach (AbsFightModel item in value.teamTwo)
        {
            GameObject go;
            Debug.Log(item.code);
            if (item.type == ModelType.HUMAN)
            {
                go = Instantiate(Resources.Load<GameObject>("prefab/Player/" + item.code), start2.position + new Vector3(Random.Range(0.5f, 1.5f), 0, Random.Range(1, 7)), start2.rotation);

            }
            else
            {
                go = Instantiate(Resources.Load<GameObject>("prefab/build/2_" + item.code), position2[item.code - 1].position, Quaternion.identity);



            }
            this.teamTwo.Add(item.id, go);
        }
    }
}
