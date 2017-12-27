using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using GameProtocol.DTO;
using UnityEngine;
using UnityEngine.UI;

public class SelectScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject heroBtn;//英雄头像
    [SerializeField]
    private Transform listParent;//英雄头像列表父容器
    [SerializeField]
    private GameObject initMask;//防止误操作遮罩

    private Dictionary<int,HeroGrid> gridMap = new Dictionary<int, HeroGrid>();
    [SerializeField]
    private SelectGrid[] left;//左边
    [SerializeField]
    private SelectGrid[] right;//右边

    [SerializeField] private Button ready;
    [SerializeField]
    private InputField talkInput;//聊天输入框
    [SerializeField]
    private Text talkMessageShow;//聊天信息显示框

    [SerializeField] private Scrollbar talkScroll;//聊天滚动条
    public void SendClick()
    {
        if (talkInput.text != string.Empty)
        {
            this.WriteMessage(Protocol.TYPE_SELECT,0,SelectProtocol.TALK_CREQ,talkInput.text);
            talkInput.text = string.Empty;
        }
    }

    public void TalkShow(string value)
    {
        talkMessageShow.text += "\n" + value;
        talkScroll.value = 0;
    }
    // Use this for initialization
    void Start ()
    {
        SelectEventUtil.selected = Selected;
        SelectEventUtil.refreshView = RefreshView;
        SelectEventUtil.selectHero = SelectHero;
        //显示遮罩防止误操作
        initMask.SetActive(true);
        //初始化英雄列表
        InitHeroList();
        //通知进入场景并加载完成
		this.WriteMessage(Protocol.TYPE_SELECT,0,SelectProtocol.ENTER_CREQ,null);
	}

    private void InitHeroList()
    {
        if(GameData.user==null)
            return;
        //int index = 0;
        foreach (int item in GameData.user.heroList)
        {
            //创建英雄头像 并添加进选择列表
            GameObject btn = Instantiate<GameObject>(heroBtn);
            HeroGrid grid = btn.GetComponent<HeroGrid>();
            grid.Init(item);
            btn.transform.parent = listParent;
            btn.transform.localScale = Vector3.one;
            //btn.transform.localPosition = new Vector3(48+72*(index%7),-42+index/7*-72);
            gridMap.Add(item,grid);
        }
        
    }

    public void CloseMask()
    {
        initMask.SetActive(false);
    }

    public void RefreshView(SelectRoomDTO room)
    {
        int team = room.getTeam(GameData.user.id);
        //自身默认队伍在左侧
        if (team == 1)
        {
            for (int i = 0; i < room.teamOne.Length; i++)
            {
                left[i].Refresh(room.teamOne[i]);
            }
            for (int i = 0; i < room.teamTwo.Length; i++)
            {
                right[i].Refresh(room.teamTwo[i]);
            }
        }
        else
        {
            for (int i = 0; i < room.teamOne.Length; i++)
            {
                right[i].Refresh(room.teamOne[i]);
            }
            for (int i = 0; i < room.teamTwo.Length; i++)
            {
                left[i].Refresh(room.teamTwo[i]);
            }
        }
        RefreshHeroList(room);
    }

    private void RefreshHeroList(SelectRoomDTO room)
    {
        int team = room.getTeam(GameData.user.id);
        List<int> selected = new List<int>();
        //获取自己所在队伍的已经选择了的英雄
        if (team == 1)
        {
            foreach (var item in room.teamOne)
            {
                if (item.hero != -1)
                {
                    selected.Add(item.hero);
                }
            }
        }
        else
        {
            foreach (var item in room.teamTwo)
            {
                if (item.hero != -1)
                {
                    selected.Add(item.hero);
                }
            }
        }
        //将已选择英雄从选择菜单中设置状态为不可选
        foreach (int item in gridMap.Keys)
        {
            if (selected.Contains(item)||!ready.interactable)
            {
                gridMap[item].Deactive();
            }
            else
            {
                gridMap[item].Active();
            }
        }
    }

    public void Selected()
    {
        ready.interactable = false;
    }

    public void SelectHero(int id)
    {
        this.WriteMessage(Protocol.TYPE_SELECT,0,SelectProtocol.SELECT_CREQ,id);        
    }

    public void ReadyClick()
    {
        this.WriteMessage(Protocol.TYPE_SELECT, 0, SelectProtocol.READY_CREQ, null);

    }
}
