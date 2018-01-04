using GameProtocol.dto.fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


   public class PlayerCon:MonoBehaviour
    {
       public FightPlayerModel data;

       protected Animator anim;

       private UnityEngine.AI.NavMeshAgent agent;

       void Start() {
           anim = GetComponent<Animator>();
           agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
       }
       /// <summary>
       /// 攻击动画播放结束回调方法
       /// </summary>
       public void attacked() { 
       
       }
       /// <summary>
       /// 移动
       /// </summary>
       /// <param name="target"></param>
       public void move(Vector3 target) {
           agent.ResetPath();
           agent.SetDestination(target);
           anim.SetInteger("state", AnimState.RUN);
       }
       /// <summary>
       /// 申请攻击，攻击的目标
       /// </summary>
       /// <param name="target"></param>
       public void attack(Transform[] target) { 
            
       }
       /// <summary>
       /// 申请释放技能
       /// </summary>
       /// <param name="code"></param>
       /// <param name="target"></param>
       public void skill(int code, Transform[] target)
       { 
       
       }
    }

