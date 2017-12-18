using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class AssetChecker : EditorWindow {

	[MenuItem("Demo/资源检查")]
	private static void Init(){
		GetWindow(typeof(AssetChecker));
	}
	private enum TabType{
		FX,Role,Scene,
	};
	private string[] TabToolStrings = {"特效","角色","场景"};
	private TabType m_Tab;
	private enum AssetType{
		Shader,Asset,Texture,Material,Audio,Model,Animation,
	}
	private string[] AssetToolbarStrings = {"着色器","资源","贴图","材质","音频","模型","动画"};
	private AssetType m_AssetT;
	private static bool ctrlPressed = false;
	private static GUIStyle s_LeftToggle; 
	private static GUIStyle leftToggle{
		get{
			if(s_LeftToggle == null){
				s_LeftToggle = new GUIStyle(EditorStyles.toolbarButton);
				s_LeftToggle.alignment = TextAnchor.MiddleLeft;
			}
			return s_LeftToggle;
		}
	}
	private void OnGUI(){
		ctrlPressed = Event.current.control||Event.current.command;
		if(GUILayout.Button("重新载入")){
			m_AllAB = null;
			m_FxAB = null;
			File.Delete(fxdepends);
			m_FxDepends = null;
			m_FxMats = null;
			m_FXShaders = null;
			m_ShaderMats = null;
			BundleInf.Reset();
		}
		m_Tab = (TabType)GUILayout.Toolbar((int)m_Tab,TabToolStrings);
		switch(m_Tab){
			case TabType.FX:
			ListFxDetail();
			break;
			case TabType.Role:
			break;
			case TabType.Scene:
			break;
			default:break; 
		}
	}
	private static void SelectObject(Object selectedObject,bool append){
		if(append){
			List<Object> currentSelection = new List<Object>(Selection.objects);
			if(currentSelection.Contains(selectedObject))
				currentSelection.Remove(selectedObject);
			else
				currentSelection.Add(selectedObject);
			Selection.objects = currentSelection.ToArray();
		}else{
			Selection.activeObject = selectedObject;
		}
	}
	private static void SelectObjects(Object[] selectedObjects,bool append){
		if(append){
			List<Object> currentSelection = new List<Object>(Selection.objects);
			currentSelection.AddRange(selectedObjects);
			Selection.objects = currentSelection.ToArray();
		}else{
			Selection.objects = selectedObjects;
		}
	}
	private static void SelectObject(string assetPaths,bool append){
		var selectedObject = AssetDatabase.LoadMainAssetAtPath(assetPaths);
		SelectObject(selectedObject,append);
	}
	private static void SelectObjects(string[] assetPaths,bool append){
		var selectedObjects = new Object[assetPaths.Length];
		for (int i = 0; i < assetPaths.Length; i++)
		{
			selectedObjects[i] = AssetDatabase.LoadMainAssetAtPath(assetPaths[i]);
		}
		SelectObjects(selectedObjects,append);
	}
	private static void SelectObjects<T>(AssetsSet<T>.AssetPair[] pairs,bool append) where T:Object{
		var selectedObjects = new Object[pairs.Length];
		for (int i = 0; i < pairs.Length; i++)
		{
			selectedObjects[i] = pairs[i].Obj;
		}
		if(append){
			List<Object> currentSelection = new List<Object>(Selection.objects);
			currentSelection.AddRange(selectedObjects);
			Selection.objects = currentSelection.ToArray();
		}else{
			Selection.objects = selectedObjects;
		}
	}
	private class BundleInf{
		public static Dictionary<string,BundleInf> AllBundles{get;private set;}
		public static void Reset(){AllBundles = null;}
		public static BundleInf Get(string bundleName){
			if(AllBundles == null){
				AllBundles = new Dictionary<string,BundleInf>();
			}
			BundleInf bundle;
			if(!AllBundles.TryGetValue(bundleName,out bundle)){
				bundle = new BundleInf(bundleName);
				AllBundles.Add(bundleName,bundle);
			}
			return bundle;
			
		}
		public string bundle{get;private set;}
		private Dictionary<string,string[]>m_Dict;
		public string[] AllDepends{get;private set;}
		public string[] GetAssets(string type){
			string[] assets;
			m_Dict.TryGetValue(type,out assets);
			return assets;
		}
		public BundleInf(string bundleName){
			this.bundle = bundleName;
			var objs = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
			AllDepends = AssetDatabase.GetDependencies(objs);
			var liMat = new List<string>();
			var liTex = new List<string>();
			var liAud = new List<string>();
			var liFbx = new List<string>();
			var liAni = new List<string>();
			foreach (var d in AllDepends)
			{
				var ext = Path.GetExtension(d).ToLower();
				switch(ext){
					case ".mat":liMat.Add(d);break;
					case ".tga":
					case ".png": liTex.Add(d);break;
					case ".fbx":liFbx.Add(d);break;
					case ".ogg":
					case ".mp3":liAud.Add(d);break;
					case ".controller":
					case ".anim":liAni.Add(d);break;
					default:break;
				}
			}
			liMat.Sort();liTex.Sort();liFbx.Sort();liAud.Sort();liAni.Sort();
			m_Dict = new Dictionary<string, string[]>();
			m_Dict.Add("mat",liMat.ToArray());
			m_Dict.Add("tex",liTex.ToArray());
			m_Dict.Add("fbx",liFbx.ToArray());
			m_Dict.Add("aud",liAud.ToArray());
			m_Dict.Add("ani",liAni.ToArray());
			m_Dict.Add("obj",objs);
		}
		private static void ShowFxesUsingAsset(string type,string asset){
			var strbld = new System.Text.StringBuilder();
			var listInf = new List<BundleInf>();
			foreach (var kv in AllBundles)
			{
				string[] assets;
				if(kv.Value.m_Dict.TryGetValue(type,out assets)){
					foreach (var path in assets)
					{
						if(path == asset){
							listInf.Add(kv.Value);
							break;
						}
					}
				}
				
			}
			var startIdx = "Assets/Prefabs".Length;
			foreach (var inf in listInf)
			{
				foreach (var p in inf.m_Dict["obj"])
				{
					if(p!=asset){
						var depends = AssetDatabase.GetDependencies(p);
						foreach (var d in depends)
						{
							if(d==asset){
								strbld.AppendLine(p.Substring(startIdx+1));
							}
						}
					}
				}
			}
			EditorUtility.DisplayDialog(asset,strbld.Length>0?strbld.ToString():"未被其他引用","确定");
		}
		private Vector2 m_ScrollPos;
		public void DrawList(string type){
			string[] arr;
			if(m_Dict.TryGetValue(type,out arr)){
				var sharedDir = bundle.StartsWith("fx")?"Assets/Artwork/FX/Shared":"Assets/Artwork/UIFX/Shared/";
				var bundleName = bundle.Substring(0,bundle.LastIndexOf('.'));
				var commonName= sharedDir.ToLower();
				var otherName = "assets/artwork/";
				var isObj = type =="obj";
				m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);
				GUILayout.BeginVertical();
				var defColor = GUI.color;
				for (int i = 0; i < arr.Length; i++)
				{
					var path = arr[i];
					GUILayout.BeginHorizontal();
					if(!isObj){
						if(GUILayout.Button("查看",GUILayout.Width(40))){
							ShowFxesUsingAsset(type,path);
						}
						if(GUILayout.Button("共享",GUILayout.Width(40))){
							AssetDatabase.MoveAsset(path,sharedDir+Path.GetFileName(path));
						}
					}
					var pathL = path.ToLower();
					GUI.color = pathL.Contains(bundleName)?defColor:
					pathL.StartsWith(commonName)?Color.green:
					pathL.StartsWith(otherName)?Color.yellow:Color.red;
					if(GUILayout.Button(path,CustomEditorStyles.leftText)){
						SelectObject(AssetDatabase.LoadMainAssetAtPath(path),ctrlPressed);
					}
					GUI.color = defColor;
					GUILayout.EndHorizontal();
				}
				if(arr.Length>0){
					if(GUILayout.Button("全部")){
						SelectObjects(arr,ctrlPressed);
					}
				}else{
					GUILayout.Label(string.Format("未引用{0}资源",type));
				}
				GUILayout.EndVertical();
				GUILayout.EndScrollView();
			}
		}
		
	}
	private class AssetsSet<T> where T:Object{
		public class AssetPair:System.IComparable{
			public string path{get;private set;}
			private T m_Obj;
			public T Obj{
				get{
					if(m_Obj==null){
						m_Obj = AssetDatabase.LoadAssetAtPath<T>(path);
					}
					return m_Obj;
				}
			}
			public AssetPair(string p){path = p;}
			int System.IComparable.CompareTo(object obj){
				var pair = obj as AssetPair;
				if(pair!=null) 
					return string.Compare(path,pair.path);
				return -1;
			}
		}
		public AssetPair[] assets{get;private set;}

		public AssetPair this[int i]{get{return assets[i];}}
		private string[] m_Names;
		public string[] names{
			get{
				if(m_Names==null){
					m_Names = new string[assets.Length];
					var folder = "Assets/Shaders";
					var startIdx = folder.Length+1;
					for (int i = 0; i < m_Names.Length; i++)
					{
						var path = assets[i].path;
						m_Names[i] = path.StartsWith(folder)?path.Substring(startIdx):path;
					}
				}
				return m_Names;
			}
		}
		public Vector2 scrollPos;
		public int selectIndex;
		public static AssetsSet<T> Generate(ICollection<string> assetPaths,string ext){
			var Set = new AssetsSet<T>();
			var assets = new List<AssetPair>();
			foreach (var path in assetPaths)
			{
				if(path.EndsWith(ext)){
					assets.Add(new AssetPair(path));
				}
			}
			assets.Sort();
			Set.assets = assets.ToArray();
			return Set;
		}
	}
	private static string[] m_AllAB;
	private static string[] AllAB{
		get{
			if(m_AllAB==null){
				m_AllAB = AssetDatabase.GetAllAssetBundleNames();
			}
			return m_AllAB;
		}
	}
	private static string[] m_FxAB;
	private static string[] FxAB{
		get{
			if(m_FxAB == null){
				var list = new List<string>();
				foreach (var a in AllAB)	
				{
					if(a.StartsWith("fx/")||a.StartsWith("uifx"))
						list.Add(a);
				}
				m_FxAB = list.ToArray();
			}
			return m_FxAB;
		}
	}
	private const string fxdepends = "fxdepends.tmp";
	private static List<string> m_FxDepends;
	private static List<string> FxDepends{
		get{
			if(m_FxDepends == null){
				m_FxDepends = new List<string>();
				if(File.Exists(fxdepends)){
					var lines = File.ReadAllLines(fxdepends);
					m_FxDepends.AddRange(lines);
				}else{
					foreach (var a in FxAB)
					{
						var paths = AssetDatabase.GetAssetPathsFromAssetBundle(a);
						var depends = AssetDatabase.GetDependencies(paths);
						foreach (var d in depends)
						{
							if(!m_FxDepends.Contains(d)){
								m_FxDepends.Add(d);
							}
						}
					}
					File.WriteAllLines(fxdepends,m_FxDepends.ToArray());
				}
			}
			return m_FxDepends;
		}
	}
	private static AssetsSet<Material> m_FxMats;
	private static AssetsSet<Material> fxMats{
		get{
			if(m_FxMats == null){
				m_FxMats = AssetsSet<Material>.Generate(FxDepends,".mat");
			}
			return m_FxMats;
		}
	}
	private static AssetsSet<Shader> m_FXShaders;
	private static AssetsSet<Shader> fxShaders{
		get{
			if(m_FXShaders==null){
				m_FXShaders = AssetsSet<Shader>.Generate(FxDepends,".shader");
			}
			return m_FXShaders;
		}
	}
	private void ListFxDetail(){
		m_AssetT = (AssetType)GUILayout.Toolbar((int)m_AssetT,AssetToolbarStrings);
		switch (m_AssetT)
		{
			case AssetType.Shader :ListFxShaders();break;
			case AssetType.Asset :ListFxAsset("obj");break;
			case AssetType.Texture:ListFxAsset("tex");break;
			case AssetType.Material:ListFxAsset("mat");break;
			case AssetType.Audio:ListFxAsset("aud");break;
			case AssetType.Model:ListFxAsset("fbx");break;
			case AssetType.Animation:ListFxAsset("ani");break;
			default:break;
		}
	}
	private Vector2 m_FxBundleScrollPos;
	private int m_SelFx;
	private void ListFxAsset(string type){
		GUILayout.BeginHorizontal();
		m_FxBundleScrollPos = EditorGUILayout.BeginScrollView(m_FxBundleScrollPos,GUILayout.Width(160));
		GUILayout.BeginVertical();
		if(GUILayout.Button("初始化所有")){
			foreach (var ab in FxAB)
			{
				BundleInf.Get(ab);
			}
		}
		m_SelFx = GUILayout.SelectionGrid(m_SelFx,FxAB,1,leftToggle);
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		if(m_SelFx>=0){
			BundleInf.Get(FxAB[m_SelFx]).DrawList(type);
		}
		GUILayout.EndHorizontal();
	}
	private void ListFxShaders(){
		GUILayout.BeginHorizontal();
		m_FxBundleScrollPos = EditorGUILayout.BeginScrollView(m_FxBundleScrollPos,GUILayout.Width(300));
		GUILayout.BeginVertical();
		var preSel = fxShaders.selectIndex;
		fxShaders.selectIndex = GUILayout.SelectionGrid(fxShaders.selectIndex,fxShaders.names,1,leftToggle);
		m_SelFx = GUILayout.SelectionGrid(m_SelFx,FxAB,1,leftToggle);
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		if(fxShaders.selectIndex>=0){
			if(preSel!=fxShaders.selectIndex)
				m_SelMat = -1;
			if(fxShaders.selectIndex!=0)
				ListMatsUsingShader(fxShaders[fxShaders.selectIndex].path);
		}
		GUILayout.EndHorizontal();
	}
	private static Dictionary<string,AssetsSet<Material>.AssetPair[]>m_ShaderMats;
	private Vector2 m_ShaderMatScrollPos;
	private int m_SelMat = -1;
	private void ListMatsUsingShader(string shaderPath){
		if(m_ShaderMats == null)
			m_ShaderMats = new Dictionary<string, AssetsSet<Material>.AssetPair[]>();
		AssetsSet<Material>.AssetPair[] Pairs;
		if(!m_ShaderMats.TryGetValue(shaderPath,out Pairs)){
			var list = new List<AssetsSet<Material>.AssetPair>();
			foreach (var asset in fxMats.assets)
			{
				if(asset.Obj){
					var s = AssetDatabase.GetAssetPath(asset.Obj.shader);
					if(s==shaderPath){
						list.Add(asset);
					}
				}
			}
			Pairs = list.ToArray();
			m_ShaderMats.Add(shaderPath,Pairs);
		}
		m_ShaderMatScrollPos = EditorGUILayout.BeginScrollView(m_ShaderMatScrollPos);
		GUILayout.BeginVertical();
		var defColor = GUI.color;
		for (int i = 0; i < Pairs.Length; i++)
		{
			var pair = Pairs[i];
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("",GUILayout.Width(30))){
				ShowFxesUsingMat(pair.path);
			}
			GUI.color = m_SelMat==i?Color.yellow:defColor;
			if(GUILayout.Button(pair.path,CustomEditorStyles.leftText,GUILayout.Width(400))){
				m_SelMat = i;
				SelectObject(pair.Obj,ctrlPressed);
			}
			GUI.color = defColor;
			GUILayout.EndHorizontal();
		}
		if(GUILayout.Button("选择全部材质")){
			SelectObjects(Pairs,ctrlPressed);
		}
		if(GUILayout.Button("选择着色器")){
			SelectObject(shaderPath,ctrlPressed);
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}
	private void ShowFxesUsingMat(string asset){
		var strbld = new System.Text.StringBuilder();
		var listInf = new List<BundleInf>();
		foreach (var kv in BundleInf.AllBundles)
		{
			string[] assets = kv.Value.GetAssets("mat");
			if(assets!=null){
				foreach (var path in assets)
				{
					if(path==asset){
						listInf.Add(kv.Value);
						break;
					}
				}
			}
		}
		var startIdx = "Assets/Prefabs".Length;
		foreach (var inf in listInf)
		{
			foreach (var path in inf.GetAssets("obj"))
			{
				if(path!=asset){
					var depends = AssetDatabase.GetDependencies(path);
					foreach (var d in depends)
					{
						if(d==asset)
							strbld.AppendLine(path.Substring(startIdx+1));
					}
				}
			}
		}
		EditorUtility.DisplayDialog(asset,strbld.Length>0?strbld.ToString():"未被其他引用","确定");
	}
	[MenuItem("Demo/资源优化/特效网格")]
	private static void OptFxMesh(){
		foreach (var bundle in FxAB)
		{
			foreach (var asset in AssetDatabase.GetAssetPathsFromAssetBundle(bundle))
			{
				if(asset.EndsWith(".prefab")){
					var o = AssetDatabase.LoadMainAssetAtPath(asset);
					var go = Instantiate(o) as GameObject;
					var list = go.GetComponentsInChildren(typeof(MeshFilter));
					foreach (var c in list)
					{
						var mf = c as MeshFilter;
						if(mf.sharedMesh == null){
							Debug.LogWarningFormat("{0}存在空白的网格{1}",asset,c.name);
						}
					}
					DestroyImmediate(go);
				}
			}
		}
	}
	[MenuItem("Demo/资源优化/设置特效的Renderer属性")]
	private static void OptFxRenderer(){
		foreach (var bundle in FxAB)
		{
			foreach (var asset in AssetDatabase.GetAssetPathsFromAssetBundle(bundle)){
				if(asset.EndsWith(".prefab")){
					var o = AssetDatabase.LoadMainAssetAtPath(asset);
					var go = Instantiate(o) as GameObject;
					var list = go.GetComponentsInChildren(typeof(Renderer));
					var changed = false;
					foreach (var c in list)
					{
						var rdr = c as Renderer;
						if(rdr.lightProbeUsage==UnityEngine.Rendering.LightProbeUsage.Off
							&&rdr.reflectionProbeUsage == UnityEngine.Rendering.ReflectionProbeUsage.Off
							&&rdr.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.Off
							&&rdr.receiveShadows == false
							&&rdr.motionVectorGenerationMode == MotionVectorGenerationMode.ForceNoMotion)
							continue;
						changed = true;
						rdr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
						rdr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
						rdr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
						rdr.receiveShadows = false;
						rdr.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;

					}
					if(changed){
						PrefabUtility.ReplacePrefab(go,o);
					}
					DestroyImmediate(go);
				}
			}

		}
	}
	[MenuItem("Demo/资源优化/移除特效预设上空的动画")]
	private static void OptFxAnimation(){
		foreach (var bundle in FxAB)
		{
			foreach (var asset in AssetDatabase.GetAssetPathsFromAssetBundle(bundle))
			{
				if(asset.EndsWith(".prefab")){
					var o = AssetDatabase.LoadMainAssetAtPath(asset);
					var go = Instantiate(o) as GameObject;
					var list = go.GetComponentsInChildren(typeof(Animator));
					var changed = false;
					foreach (var c in list)
					{
						var ani = c as Animator;
						var n = 0;
						if(ani.runtimeAnimatorController){
							foreach (var clip in ani.runtimeAnimatorController.animationClips)
							{
								if(clip) n++;
							}
							if(n==0){
								changed = true;
								DestroyImmediate(c);
							}
						}else{
							changed = true;
							DestroyImmediate(c);
						}
					}
					list = go.GetComponentsInChildren(typeof(Animation));
					foreach (var c in list)
					{
						var ani = c as Animation;
						var n = 0;
						foreach (AnimationState state in ani)
						{
							if(state.clip)n++;
						}
						if(n==0){
							changed = true;
							DestroyImmediate(c);
						}
					}
					if(changed) PrefabUtility.ReplacePrefab(go,o);
					DestroyImmediate(go);

				}
			}
		}
	}
	[MenuItem("Demo/资源优化/检查依赖规则")]
	private static void CheckDepends(){
		foreach (var ab in FxAB)
		{
			var bundle = BundleInf.Get(ab);
			foreach (var asset in bundle.AllDepends)
			{
				if(ab.StartsWith("fx")&&asset.Contains("/UIFX/Shared")){
					Debug.LogFormat("{0}的依赖{1}在UIFX公共资源内",ab,asset);
					continue;
				}
				if(ab.StartsWith("uifx")&&asset.Contains("/FX/Shared")){
					Debug.LogFormat("{0}的依赖{1}在公共资源内",ab,asset);
				}
			}
		}
	}
	[MenuItem("Demo/资源优化/检查一些东西")]
	private static void CheckSomething(){
		foreach (var ab in FxAB)
		{
			var bundle= BundleInf.Get(ab);
			foreach (var asset in bundle.GetAssets("tex"))
			{
				if(asset.StartsWith("Assets/Artwork/FX/Textures")){
					Debug.LogFormat("{0}依赖了{1}",ab,asset);
				}
			}
		}
	}

}
