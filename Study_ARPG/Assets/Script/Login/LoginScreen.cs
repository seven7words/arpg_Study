using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameProtocol;
using GameProtocol.DTO;
public class LoginScreen : MonoBehaviour {
    #region 常量
    #endregion
    #region  属性
    #endregion
    #region 字段
        #region 登录面板
            [SerializeField]
            private InputField accountInput;
            [SerializeField]
            private InputField passwordInput;
            [SerializeField]
            private Button loginBtn;
        #endregion
        #region 注册面板
            [SerializeField]
            private GameObject regPanel;
            [SerializeField]
            private InputField regAccountInput;
            [SerializeField]
            private InputField regpwInput;
            [SerializeField]
            private InputField regpw1Input;
        #endregion
       
    #endregion
    #region 事件
    #endregion
    #region 方法
        public void LoginOnClick(){
            if(accountInput.text.Length==0||accountInput.text.Length>6){
                WarningManager.errors.Add(new WarningModel("账号不合法",delegate{
                    Debug.Log("回调测试");
                }));
                return;
            }
            if(passwordInput.text.Length==0||passwordInput.text.Length>6){
                Debug.Log("密码不合法");
                return;
            }
            //TODO:验证通过 申请登录
            //loginBtn.enabled = false;
            AccountInfoDTO accountInfoDTO = new AccountInfoDTO();
            accountInfoDTO.account = accountInput.text;
            accountInfoDTO.password = passwordInput.text;
            loginBtn.interactable = false;
            NetIO.Instance.Write(Protocol.TYPE_LOGIN,0,LoginProtocol.LOGIN_CREQ,accountInfoDTO);

        }
        public void RegOnClick(){
            regPanel.SetActive(true);  
        }
        public void RegCloseOnClick(){
            regPanel.SetActive(false); 
        }
        public void RegPanelRegOnClick(){
            if(regAccountInput.text.Length==0||regAccountInput.text.Length>6){
                Debug.Log("账号不合法");
                return;
            }
            if(regpwInput.text.Length==0||regpwInput.text.Length>6){
                Debug.Log("密码不合法");
                return;
            }
            if(regpw1Input.text.Length==0||regpw1Input.text.Length>6||!string.Equals(regpwInput.text,regpw1Input.text)){
                Debug.Log("两次输入密码不一致");
                return;
            }
            //TODO:验证通过申请注册并关闭注册面板
        }
    #endregion
    #region Unity回调
        // Use this for initialization
        void Start () {
            NetIO io =    NetIO.Instance;
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
