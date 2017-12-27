using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroGrid : MonoBehaviour
{
    [SerializeField]
    private Button btn;
    [SerializeField]
    private int id = -1;
    [SerializeField]
    private Image img;
    public void Init(int id)
    {
        this.id = id;
        img.sprite = Resources.Load<Sprite>("HeroIcon/"+id);
    }

    public void Active()
    {
        btn.interactable = true;
    }

    public void Deactive()
    {
        btn.interactable = false;
    }

    public void OnClick()
    {
        if (id != -1)
        {
            //处理角色选择事件
            SelectEventUtil.selectHero(id);
        }
    }
}
