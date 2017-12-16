using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Demo : Editor {

	[MenuItem("Demo/GetLabels")]
	public  static void GetLabels(){
		UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath("Assets/Script/NetIO.cs");
		Debug.Log(objs.Length);
		foreach (var obj in objs)
		{
			string[] mylabels =	AssetDatabase.GetLabels(obj);
			foreach (var item in mylabels)
			{
				Debug.Log(item);
			}
		}
	 	
	}
}
