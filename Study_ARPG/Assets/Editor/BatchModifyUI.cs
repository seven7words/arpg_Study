using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

public class BatchModifyUI  {

    public static List<T> GetObjectList<T>(string strDir, string pattern, SearchOption opt) where T : Object
    {
        //先得到资源路径
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath+"/"+strDir);
        //得到资源路径下以pattern为结尾的所有文件
        FileInfo[] files = dir.GetFiles(pattern, opt);
        List<T> objList = new List<T>();
        //得到unity工程的默认路径
        int startIndex = Application.dataPath.Length;
        foreach (FileInfo file in files)
        {
            //得到在Assets目录下的资源路径
            string assetPath = "Assets" + file.FullName.Substring(startIndex);
            //得到该物体
            T obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
            if (obj as T)
            {
                objList.Add(obj);
            }
        }
        //得到物体集合
        return objList;
    }
    /// <summary>
    /// 批量替换物体
    /// </summary>
    /// <param name="init">是否需要初始化</param>
    /// <param name="onAction">对物体的回调函数</param>
    public static void ModifyUIPrefabs(bool init, System.Func<GameObject, bool> onAction)
    {
        var objs = GetObjectList<GameObject>("Prefabs/UI/", "*.prefab", SearchOption.AllDirectories);
        foreach (var o in objs)
        {
            var go = o;
            
            if (init)
            {
                go = Object.Instantiate(o) as GameObject;
                go.name = o.name;
                Debug.Log(o.name);
            }
            if (onAction.Invoke(go) && init)
            {
                AssetDatabase.SaveAssets();
                PrefabUtility.ReplacePrefab(go, o, ReplacePrefabOptions.ReplaceNameBased);
                Debug.LogFormat("预设替换:{0}",o.name);
            }
            if(init) Object.DestroyImmediate(go);
        }
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Demo/界面批处理/按钮格式")]
    private static void ChangeButtonNavigation()
    {
        ModifyUIPrefabs(true, (go) =>
        {
            bool dirty = false;
            var buttons = go.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                if (button.navigation.mode != Navigation.Mode.None)
                {
                    Navigation navigation = new Navigation();
                    navigation.mode = Navigation.Mode.None;
                    button.navigation = navigation;
                    dirty = true;
                }
                if (dirty)
                {
                    if (go)
                    {
                        UnityEditor.EditorUtility.SetDirty(go);
                    }
                }
            }
            return dirty;
        });
    }
}
