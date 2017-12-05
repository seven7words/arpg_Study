using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningManager : MonoBehaviour {
    #region 常量
    #endregion
    #region  属性
    #endregion
    #region 字段
    public static List<WarningModel> errors = new List<WarningModel>();
    [SerializeField]
    private WarningWindow window;
    #endregion
    #region 事件
    #endregion
    #region 方法
    #endregion
    #region Unity回调
        // Use this for initialization
        void Start () {
            
        }
        
        // Update is called once per frame
        void Update () {
            if(errors.Count>0){
            WarningModel err = errors[0];
            errors.RemoveAt(0);
            window.Active(err);
            }
        }
    #endregion
    #region  事件回调
    #endregion
    #region 帮助方法
    #endregion
	
}
