using UnityEngine;
using System.Collections;
using GameProtocol;
using GameProtocol.dto.fight;
using UnityEngine.UI;
using GameProtocol.constans;
public class FightScene : MonoBehaviour {
    public static FightScene instance;
    [SerializeField]
    /// <summary>
    /// 个人头像
    /// </summary>
    /// 
    private Image head;
    [SerializeField]
    /// <summary>
    /// 自身血条
    /// </summary>
    private Slider hpSlider;
    [SerializeField]
    /// <summary>
    /// 角色名
    /// </summary>
    private Text nameText;
    [SerializeField]
    /// <summary>
    /// 技能Icon
    /// </summary>
    private SkillGrid[] skills;
    [SerializeField]
    private Text levelText;

    private GameObject myHero;

    private Camera mainCamera;
	void Start () {
        instance = this;
        mainCamera = Camera.main;
        this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.ENTER_CREQ, null);
	}

    public void initView(FightPlayerModel model,GameObject hero) {
        myHero = hero;
        head.sprite = Resources.Load<Sprite>("HeroIcon/"+model.code);
        hpSlider.value = model.hp / model.maxHp;
        nameText.text = HeroData.heroMap[model.code].name;
        levelText.text = model.level.ToString();
        int i = 0;
        foreach (FightSkill item in model.skills)
	    {
            skills[i].init(item);
            i++;
	    }             
    }

    public void lookAt() {
        mainCamera.transform.position = myHero.transform.position + new Vector3(-6, 8, 0);
    }
}
