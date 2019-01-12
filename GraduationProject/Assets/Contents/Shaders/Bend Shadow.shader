Shader "Bend Shader/ShadowPass_Global"
{
	SubShader 
	{
		Pass  
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			Fog {Mode Off}
			ZWrite On ZTest LEqual Cull Off
			Offset 1, 1
	 
			CGPROGRAM
			#pragma vertex vert_ShadowCaster
			#pragma fragment frag_ShadowCaster   
			 
			#pragma multi_compile_shadowcaster 
			#define UNITY_PASS_SHADOWCASTER
			#include "UnityCG.cginc"  	

			#define V_CW_GLOBAL_ON
		    
			#include "cginc/Base.cginc"
			#include "cginc/S1.cginc"
			        
			ENDCG  
		}			  

		
		Pass 
		{  
			Name "ShadowCollector"
			Tags { "LightMode" = "ShadowCollector" }
			Fog {Mode Off}
			ZWrite On ZTest LEqual

			    
			CGPROGRAM   
			#pragma vertex vert_ShadowCollector
			#pragma fragment frag_ShadowCollector
			 
			#pragma multi_compile_shadowcollector
			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			#define UNITY_PASS_SHADOWCOLLECTOR
			#define SHADOW_COLLECTOR_PASS
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			#define V_CW_GLOBAL_ON

			#include "cginc/Base.cginc"
			#include "cginc/S1.cginc"

 			ENDCG
		} 
	} 
}
