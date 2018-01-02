Shader "Unlit/WarFog"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _MaskTex;
			fixed4 frag(v2f_img i):COLOR{
				fixed4 texColor = tex2D(_MainTex,i.uv);
				fixed4 maskColor = tex2D(_MaskTex,i.uv);
				fixed4 result;
				if(maskColor.r<0.3){
					result = maskColor.rgba;
				}else{
					result = texColor.rgba;
				}
				return result;
			}
	
			ENDCG
		}
		
	}
}
