using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WarningWindow : MonoBehaviour {
    #region 常量
    #endregion
    #region  属性
    #endregion
    #region 字段
    [SerializeField]
    private Text text; 
    private WarningResult result;
    #endregion
    #region 事件
    #endregion
    #region 方法
    public void Active(WarningModel model){
        text.text = model.value;
        this.result = model.result;
        if (model.delay > 0)
        {
            Invoke("Close", model.delay);
        }
        gameObject.SetActive(true); 
    }
    public void Close(){
        if(IsInvoking("Close")) CancelInvoke("Close");
        gameObject.SetActive(false);
        if(result!=null){
            result();
        }
    }
    #endregion
    #region Unity回调
        // Use this for initialization
        void Start () {
            
        }
        
        // Update is called once per frame
        void Update () {
            
        }
    #endregion
    #region  事件回调
    #endregion
    #region 帮助方法
    #endregion
	
}
