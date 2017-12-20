using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public class SubAssetPostprocessor:AssetPostprocessor{
    //这是一个迷幻的委托方式，泛型里的第一个是返回值，后面的是参数，最大不能超过12个（出自C#高级编程）
    private static System.Func<string,string> GetAssetName = System.IO.Path.GetFileNameWithoutExtension;

    #region Model
    public void OnPreprocessModel(){
        ModelImporter mi = (ModelImporter) assetImporter;
        string path = mi.assetPath;
        string an = GetAssetName(path);
        ProcModelDefault(an,mi);
        if(path.Contains("ArtWork/Heroes")){
            ProcModelHeroes(an,mi);
        }else if(path.Contains("Artwork/Scenes")){
            ProcModelStage(an,mi);
        }else if(path.Contains("FX/")){
            ProcModelFX(an,mi);
        }
    }
    void ProcModelDefault(string assetName,ModelImporter mi){
        mi.optimizeMesh = true;
        mi.meshCompression = ModelImporterMeshCompression.Medium;
        mi.isReadable = assetName.Contains("_RW");
        if(assetName.Contains("@")||assetName.Contains("-ani")){
            mi.importAnimation = true;
            if(assetName.Contains("-g")){
                mi.animationType = ModelImporterAnimationType.Generic;
            }else{
                mi.animationType = ModelImporterAnimationType.Legacy;
            }
        }else{
            mi.importAnimation = false;
            mi.animationType = ModelImporterAnimationType.None;
        }
    }
    void OnPostprocessModel(GameObject go){

    }
    void ProcModelStage(string assetName,ModelImporter mi){
        mi.importTangents = ModelImporterTangents.CalculateMikk;
        string path = mi.assetPath;
    }
    void ProcModelHeroes(string assetName,ModelImporter mi){
        mi.importAnimation = true;
        mi.animationType = ModelImporterAnimationType.Legacy;
        mi.importNormals = ModelImporterNormals.Import;
        mi.importTangents = ModelImporterTangents.CalculateMikk;
        if(mi.globalScale<=0.02f) mi.globalScale = 1f;
        if(assetName.Contains("@")){
            mi.importMaterials = false;
        }
    }
    void ProModelStage(string assetName,ModelImporter mi){
        mi.importTangents = ModelImporterTangents.CalculateMikk;
        string path = mi.assetPath;
    }
    void ProcModelFX(string assetName,ModelImporter mi){
        mi.importTangents = ModelImporterTangents.None;
        mi.importMaterials = false;
    }
    #endregion
    #region Texture
        void OnPreprocessTexture(Texture2D texture){
            string pathUniform = assetImporter.assetPath.Replace("\\","/");
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(pathUniform,typeof(Texture));
            var labels = AssetDatabase.GetLabels(obj);//I Don't Know
            var mansual = false;
            foreach (var lb in labels)
            {
                if(lb == "Mansual"){
                    mansual = true;
                }
            }
            if(mansual) return;
            foreach (var kv in dictTexProce)    
            {
                if(pathUniform.StartsWith(kv.Key)){
                    TextureImporter ti = (TextureImporter) assetImporter;
                    kv.Value.Invoke(ti,pathUniform);
                    ProcTextureFormat(ti,pathUniform,texture);
                    break;
                }
            }
        }
        void ProcTextureFormat(TextureImporter ti,string path,Texture2D texture){
            bool formatOverWriten = false;
            if(path.Contains("_a")){
                formatOverWriten = true;

            }else if(path.Contains("_B16")){
                ti.textureCompression = TextureImporterCompression.Compressed;
                formatOverWriten = true;
            }else if(path.Contains("_B32")){
                ti.textureCompression = TextureImporterCompression.Uncompressed;
                formatOverWriten = true;
            }
            else if(path.Contains("_S128")){
                ti.maxTextureSize =128;

            }else if(path.Contains("_S256")){
                ti.maxTextureSize = 256;
            }else if(path.Contains("_S512")){
                ti.maxTextureSize = 512;
            }else if(path.Contains("_S1024")){
                ti.maxTextureSize = 1024;
            }else if (path.Contains("_dither565"))
            {
                var texw = texture.width;
                var texh = texture.height;
                var pixels = texture.GetPixels();
                var offs = 0;
                var k1Per31 = 1.0f / 31.0f;
                var k1Per32 = 1.0f / 32.0f;
                var k5Per32 = 5.0f / 32.0f;
                var k11Per32 = 11.0f / 32.0f;
                var k15Per32 = 15.0f / 32.0f;
                var k1Per63 = 1.0f / 63.0f;
                var k3Per64 = 3.0f / 64.0f;
                var k11Per64 = 11.0f / 64.0f;
                var k21Per64 = 21.0f / 64.0f;
                var k29Per64 = 29.0f / 64.0f;
                var k_r = 32;//R&B压缩到5位，所以取2的5次方
                var k_g = 64;//G压缩到6位，所以取2的6次方
                for (int y = 0; y < texh; y++)
                {
                    for (int x = 0; x < texw; x++)
                    {
                        float r = pixels[offs].r;
                        float g = pixels[offs].g;
                        float b = pixels[offs].b;
                        var r2 = Mathf.Clamp01(Mathf.Floor(r * k_r) * k1Per31);
                        var g2 = Mathf.Clamp01(Mathf.Floor(g * k_g) * k1Per63);
                        var b2 = Mathf.Clamp01(Mathf.Floor(g * k_g) * k1Per31);
                        var re = r - r2;
                        var ge = g - g2;
                        var be = b - b2;
                        var n1 = offs + 1;
                        var n2 = offs + texw - 1;
                        var n3 = offs + texw;
                        var n4 = offs + texw + 1;
                        if (x < texw - 1)
                        {
                            pixels[n1].r += re * k15Per32;
                            pixels[n1].g += ge * k29Per64;
                            pixels[n1].b += be * k15Per32;
                        }
                        if (y < texh - 1)
                        {
                            pixels[n3].r += re * k11Per32;
                            pixels[n3].g += re * k21Per64;
                            pixels[n3].b += be * k11Per32;
                            if (x > 0)
                            {
                                pixels[n2].r += re * k5Per32;
                                pixels[n2].g += ge * k11Per64;
                                pixels[n3].b += be * k5Per32;

                            }
                            if (x < texw - 1)
                            {
                                pixels[n4].r += re * k1Per32;
                                pixels[n4].g += ge * k3Per64;
                                pixels[n4].b += be * k1Per32;
                            }
                        }
                        pixels[offs].r = r2;
                        pixels[offs].r = g2;
                        pixels[offs].b = b2;
                        offs++;
                    }
                }
                texture.SetPixels(pixels);
            EditorUtility.CompressTexture(texture,TextureFormat.RGB565,TextureCompressionQuality.Best);
            }
            if(path.Contains("_NM")){
                ti.mipmapEnabled = true;
            }
            if(formatOverWriten){
                ti.ClearPlatformTextureSettings("iPhone");
            }

        }
        //这个委托则是由两个参数不带返回值的委托君。话说这个的用法很想lua里面的，table中可以直接根据键名加上括号就可以直接调用，但是
        //C#需要加上Invoke而已，本质上是一样的。很炫酷的使用方法，学习了啊
        readonly Dictionary<string,System.Action<TextureImporter,string>> dictTexProce = new Dictionary<string,System.Action<TextureImporter,string>>(){
            {"Assets/Artwork/UI/",ProcTextureUI},
            {"Assets/Artwork/Heroes/",ProcTextureHeros},
            {"Assets/Artwork/Scenes/Stage",ProcTextureScene},
            {"Assets/Artwork/FX/Textures/",ProcTextureFX},
            {"Assets/Artwork/UIFX/Textures/",ProcTextureFX},
            //{"Assets/Scenes/Stage-",ProcTextureLM},
            {"Assets/Scenes/Stage-",ProcTextureLM},
        };
        //UI贴图压缩格式
        static void ProcTextureUI(TextureImporter ti,string path){
            var objName = System.IO.Path.GetFileNameWithoutExtension(path);
            if(objName.StartsWith("_"))
                return;
            ti.textureType = TextureImporterType.Default;
            ti.npotScale = TextureImporterNPOTScale.ToNearest;
            ti.isReadable = false;
            ti.mipmapEnabled = false;
            ti.wrapMode = TextureWrapMode.Clamp;
            ti.filterMode = FilterMode.Bilinear;
            ti.anisoLevel = 0;
            ti.ClampTexSize(2048);
        }
        //英雄贴图压缩格式
        static void ProcTextureHeros(TextureImporter ti,string path){
            var obj = AssetDatabase.LoadAssetAtPath(path,typeof(Texture));
            AssetDatabase.SetLabels(obj,new string[]{"Hero"});
            ti.textureType = TextureImporterType.Default;
            ti.npotScale = TextureImporterNPOTScale.ToNearest;
            ti.mipmapEnabled = false;
            ti.wrapMode = TextureWrapMode.Clamp;
            ti.alphaSource = TextureImporterAlphaSource.None;
            ti.alphaIsTransparency = false;
            if(path.Contains("_a")){
                ti.ClampTexSize(128);
            }else{
                ti.ClampTexSize(512);
            }
        }
        private static void ProcCompressedTexture(TextureImporter ti){
            ti.textureCompression = TextureImporterCompression.Compressed;
            var setting = ti.GetPlatformTextureSettings("iPhone");
            setting.overridden = true;
            if(setting.maxTextureSize>1024){
                setting.maxTextureSize = 1024;
            }
            setting.format = TextureImporterFormat.PVRTC_RGB4;
            ti.SetPlatformTextureSettings(setting);
            setting = ti.GetPlatformTextureSettings("Android");
            setting.overridden = true;
            setting.format = TextureImporterFormat.ETC_RGB4;
            ti.SetPlatformTextureSettings(setting);
        }
        //副本地形贴图
        static void ProcTextureScene(TextureImporter ti,string path){
            ti.mipmapEnabled = true;
            if(path.Contains("_HD"))return;
            ProcCompressedTexture(ti);    
        }
        //Lightmap格式设置
        static void ProcTextureLM(TextureImporter ti,string path){
            ti.mipmapEnabled = true;
            ti.textureType = TextureImporterType.Lightmap;
            ti.wrapMode = TextureWrapMode.Clamp;
            ti.filterMode = FilterMode.Bilinear;
            if(path.Contains("_HD")) return;
            ProcCompressedTexture(ti);
        }
        //特效 图片设置
        static void ProcTextureFX(TextureImporter ti,string path){
            ti.mipmapEnabled = true;
            ti.ClampTexSize(512);
        }
    #endregion Texture
    #region Sound
        void OnPreprocessAudio(){
            string pathUniform = assetImporter.assetPath.Replace("\\","/");
            foreach (var kv in dictAudProce)
            {
                if(pathUniform.StartsWith(kv.Key)){//StartWIth明确

                    AudioImporter ai = (AudioImporter) assetImporter;
                    kv.Value.Invoke(ai);
                    break;
                }
            }
        }
        readonly Dictionary<string,System.Action<AudioImporter>> dictAudProce = new Dictionary<string,System.Action<AudioImporter>>(){
            {"Assets/Prefabs/BGM/",ProcAudioBGM},
            {"Assets/Prefabs/Sound/",ProcAudioSound},
            {"Assets/Prefabs/Voice/",ProcAudioVoice},
            {"Assets/Prefabs/FX/",ProcAudioSFX},
        };
        static void ProcAudioBGM(AudioImporter ai){
            ai.preloadAudioData = false;
            var setting = ai.defaultSampleSettings;
            setting.loadType = AudioClipLoadType.CompressedInMemory;
            ai.defaultSampleSettings = setting;
        }
        static void ProcAudioSound(AudioImporter ai){
            //此类音效的资源内存镜像会被卸载，需要在载入的时候自动加载AudioDAta
            ai.preloadAudioData = true;
        }
        static void ProcAudioVoice(AudioImporter ai){
            ai.preloadAudioData = false;
            var setting = ai.defaultSampleSettings;
            setting.loadType = AudioClipLoadType.CompressedInMemory;
            ai.defaultSampleSettings = setting;
        }
        private static void ProcAudioSFX(AudioImporter ai){
            ai.preloadAudioData = false;
            //战斗特效：提升加载速度，压缩在内存中
            var setting = ai.defaultSampleSettings;
            setting.loadType = AudioClipLoadType.CompressedInMemory;
            ai.defaultSampleSettings = setting;
        }
        #endregion SOUND
}
public static class ImporterExt{
    public static void ClampTexSize(this TextureImporter self,int maxSize){
        if(self.maxTextureSize>maxSize){
            self.maxTextureSize = maxSize;
        }
    }
}