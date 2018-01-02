using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarFogTest : MonoBehaviour
{
    [SerializeField]
    private RenderTexture mask;

    [SerializeField] private Material mat;
    public void OnRenderImage(RenderTexture source, RenderTexture des)
    {
        mat.SetTexture("_MaskTex",mask);
        Graphics.Blit(source,des,mat);

    }
}
