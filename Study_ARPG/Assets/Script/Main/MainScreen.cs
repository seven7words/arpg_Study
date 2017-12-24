using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject mask;//防止用户误操作，顶层遮罩

    [SerializeField]
    private CreatePanel createPanel;

    #region 数显示UI组件，刷新调用
    [SerializeField]
    private Text userName;//等级显示e

    [SerializeField] private Slider expBar;//经验条
    [SerializeField] private Text matchText;//匹配按钮文字
    #endregion
    // Use this for initialization
    void Start () {
	    if (GameData.user == null)
	    {
	        mask.SetActive(true);
            //向服务器请求用户数据、
            this.WriteMessage(GameProtocol.Protocol.TYPE_USER,0,UserProtocol.INFO_CREQ,null);

	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Match()
    {
        if (matchText.text == "开始排队")
        {
            matchText.text = "取消排队";
            this.WriteMessage(Protocol.TYPE_MATCH,0,MatchProtocol.ENTER_CREQ,null);
        }
        else
        {
            matchText.text = "开始排队";
            this.WriteMessage(Protocol.TYPE_MATCH, 0, MatchProtocol.LEAVE_CREQ, null);

        }
    }
    public void RefreshView()
    {
        userName.text = GameData.user.name + "  " + GameData.user.level;
        expBar.value = GameData.user.exp / 100;
    }
    /// <summary>
    /// 打开创建面板
    /// </summary>
    public void OpenCreate()
    {
        createPanel.Open();
    }
    /// <summary>
    /// 关闭创建面板
    /// </summary>
    public void CloseCreate()
    {
        createPanel.Close();
    }

    public void CloseMask()
    {
        mask.SetActive(false);
    }
}
