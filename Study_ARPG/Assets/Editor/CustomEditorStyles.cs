using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class CustomEditorStyles  {

	private static GUIStyle m_titleStyle;
	public static GUIStyle titleStyle{
		get{
			if(m_titleStyle == null){
				#if UNITY_5
					m_titleStyle = new GUIStyle(EditorStyles.helpBox);
				#else
					m_titleStyle = new GUIStyle(EditorStyles.boldLabel);
				#endif
				m_titleStyle.fontSize = 12;
				m_titleStyle.richText = true;
			}
			return m_titleStyle;
		}
	}
	private static GUIStyle m_richText;
	public static GUIStyle richText{
		get{
			if(m_richText==null){
				m_richText = new GUIStyle(EditorStyles.label);
				m_richText.richText = true;
			}
			return m_richText;
		}
	}
	private static GUIStyle m_leftText;
	public static GUIStyle leftText{
		get{
			if(m_leftText == null){
				m_leftText = new GUIStyle(EditorStyles.label);
				m_leftText.alignment = TextAnchor.MiddleLeft;
			}
			return m_leftText;
		}
	}
	private static GUIStyle m_richTextBtn;
	public static GUIStyle richTextBtn{
		get{
			if(m_richTextBtn == null){
				m_richTextBtn = new GUIStyle(EditorStyles.miniButton );
				m_richTextBtn.richText = true;
				m_richTextBtn.fontSize = 12;
			}
			return m_richTextBtn;
		}
	}
	private static GUIStyle m_ToggleTitle;
	public static GUIStyle toggleTitle{
		get{
			if(m_ToggleTitle==null){
				m_ToggleTitle = new GUIStyle(EditorStyles.toggle);
				m_ToggleTitle.fontSize = 12;
			}
			return m_ToggleTitle;
		}
	}
	public static GUIContent
		addContent = new GUIContent("+","添加一个元素");
	public static GUIContent
		rmContent = new GUIContent("-","移除一个元素");
}
