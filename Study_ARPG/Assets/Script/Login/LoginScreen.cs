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

        public void openLoginBtn()
        {
            passwordInput.text = string.Empty;
            loginBtn.interactable = true;
        }
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
            NetIO.Instance.Write(Protocol.TYPE_LOGIN,0,LoginProtocol.LOGIN_CREQ,accountInfoDTO);
            loginBtn.interactable = false;
        }
        public void RegOnClick(){
            regPanel.SetActive(true);  
        }
        public void RegCloseOnClick()
        {
            regAccountInput.text = string.Empty;
            regpw1Input.text = string.Empty;
            regpwInput.text = string.Empty;

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
            AccountInfoDTO dto = new AccountInfoDTO();
            dto.account = regAccountInput.text;
            dto.password = regpw1Input.text;
            //TODO:验证通过申请注册并关闭注册面板
            Debug.Log("我点击了咩？");
            NetIO.Instance.Write(Protocol.TYPE_LOGIN,0,LoginProtocol.REG_CREQ,dto);
            RegCloseOnClick();
    }
    #endregion
    #region Unity回调
    #endregion
    #region  事件回调
    #endregion
    #region 帮助方法
    #endregion
	
}
