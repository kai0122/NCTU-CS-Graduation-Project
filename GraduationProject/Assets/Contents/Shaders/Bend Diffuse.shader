Shader "Bend Shader/Diffuse"
{
	Properties 
	{ 
	

		_Color("Main Color", color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}


		[HideInInspector]
		_V_CW_Rim_Color("", color) = (1, 1, 1, 1)
		[HideInInspector]
		_V_CW_Rim_Bias("", Range(-1, 1)) = 0.2

		[HideInInspector]
		_V_CW_Fog_Color("", color) = (1, 1, 1, 1)
		[HideInInspector]
		_V_CW_Fog_Density("", Range(0.0, 1.0)) = 1
		[HideInInspector]
		_V_CW_Fog_Start("", float) = 0
		[HideInInspector]
		_V_CW_Fog_End("", float) = 100
		
		[HideInInspector]
		_V_CW_IBL_Intensity("", float) = 1
		[HideInInspector] 
		_V_CW_IBL_Contrast("", float) = 1 
		[HideInInspector]   
		_V_CW_IBL_Cube("", cube ) = ""{}

		[HideInInspector]
		_V_CW_Emission_Color("", color) = (1, 1, 1, 1)
		[HideInInspector]
		_V_CW_Emission_Strength("", float) = 1

		[HideInIspector]
		V_CW_MV_MAT("", float) = 1
	}

	SubShader 
	{
		Tags { "RenderType"="CurvedWorld_Global_Opaque" 
		       "CurvedWorldTag"="Global/One Directional Light/Diffuse" 
			   "CurvedWorldBakedKeywords"="" 
			 }
		LOD 200
		

		Fog{Mode Off}

		Pass
	    {
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" } 

			CGPROGRAM
			#pragma vertex vert
	    	#pragma fragment frag
	    	#pragma fragmentoption ARB_precision_hint_fastest

			#define UNITY_PASS_FORWARDBASE  

			#pragma multi_compile V_CW_UNITY_VERTEXLIGHT_OFF V_CW_UNITY_VERTEXLIGHT_ON
			#pragma multi_compile V_CW_LIGHT_PER_VERTEX V_CW_LIGHT_PER_PIXEL
			#pragma multi_compile V_CW_SELF_ILLUMINATED_OFF V_CW_SELF_ILLUMINATED_ON
			
			#pragma multi_compile V_CW_RIM_OFF V_CW_RIM_ON
			#pragma multi_compile V_CW_GLOBAL_FOG_OFF V_CW_GLOBAL_FOG_ON
			#pragma multi_compile V_CW_VERTEX_COLOR_OFF V_CW_VERTEX_COLOR_ON
			#pragma multi_compile V_CW_GLOBAL_IBL_OFF V_CW_GLOBAL_IBL_ON

			#define V_CW_GLOBAL_OFF

			
			#ifdef V_CW_UNITY_VERTEXLIGHT_ON    
				#pragma multi_compile_fwdbase nodirlightmap  
			#else 
				#pragma multi_compile_fwdbase nodirlightmap novertexlight
			#endif

			#pragma exclude_renderers d3d11_9x
			#include "UnityCG.cginc"  
            #include "AutoLight.cginc"
			 
			#include "cginc/Bend.cginc"

			ENDCG

		}	//Pass

		//ShadowCaster
		UsePass "Bend Shader/ShadowPass_Global/SHADOWCASTER"

		//ShadowCollector
		UsePass "Bend Shader/ShadowPass_Global/SHADOWCOLLECTOR"

	}	//SubShader

		 
	//Fallback - Unlit
	SubShader 
	{
		Tags { "RenderType"="CurvedWorld_Global_Opaque" 
		       "CurvedWorldTag"="Global/Unlit/Texture" 
			   "CurvedWorldBakedKeywords"="" 
			 }
		LOD 100

		Fog{Mode Off}
		
		Pass 
	    {
			CGPROGRAM
			#pragma vertex vert
	    	#pragma fragment frag
			#define UNITY_PASS_UNLIT

			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF

			#pragma multi_compile V_CW_SELF_ILLUMINATED_OFF V_CW_SELF_ILLUMINATED_ON
			#pragma multi_compile V_CW_RIM_OFF V_CW_RIM_ON
			#pragma multi_compile V_CW_GLOBAL_FOG_OFF V_CW_GLOBAL_FOG_ON
			#pragma multi_compile V_CW_VERTEX_COLOR_OFF V_CW_VERTEX_COLOR_ON
			#pragma multi_compile V_CW_GLOBAL_IBL_OFF V_CW_GLOBAL_IBL_ON

			#define V_CW_GLOBAL_OFF
			#define V_CW_UNLIT_LIGHTMAP_ON
			
			#pragma exclude_renderers d3d11_9x
			#include "cginc/Bend.cginc" 

			ENDCG

		}	

	}	


}
