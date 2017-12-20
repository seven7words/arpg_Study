using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 站还是创建面板处理
/// </summary>
public class CreatePanel : MonoBehaviour
{
    [SerializeField]
    private InputField nameField;
    [SerializeField]
    private Button btn;

    public void Open()
    {
        btn.enabled = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);

    }

    public void OnClick()
    {
        if (nameField.text.Length > 6 || nameField.text == string.Empty)
        {
            WarningManager.errors.Add(new WarningModel("请输入正确的昵称"));
            return;
        }
        btn.enabled = false;
        this.WriteMessage(Protocol.TYPE_USER,0,UserProtocol.CREATE_CREQ,nameField.text);
    }
}
