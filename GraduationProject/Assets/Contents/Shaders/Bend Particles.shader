Shader "Bend Shader/Addative Soft"
{
	Properties 
	{
	

		_Color("Tint Color", color) = (1, 1, 1, 1)
		_MainTex ("Particle Texture", 2D) = "white" {}


		[HideInInspector]
		_V_CW_Fog_Color("", color) = (1, 1, 1, 1)
		[HideInInspector]
		_V_CW_Fog_Density("", Range(0.0, 1.0)) = 1
		[HideInInspector]
		_V_CW_Fog_Start("", float) = 0
		[HideInInspector]
		_V_CW_Fog_End("", float) = 100

		[HideInInspector]
		_V_CW_Emission_Color("", color) = (1, 1, 1, 1)
		[HideInInspector]
		_V_CW_Emission_Strength("", float) = 1
	}

	SubShader 
	{
		Tags { "Queue"="Transparent" 
		       "IgnoreProjector"="True" 
			   "RenderType"="Transparent" 
			   "CurvedWorldTag"="Global/Particles/Addative (Soft)" 
			   "CurvedWorldBakedKeywords"="" 
			 }
		LOD 100

		Blend One OneMinusSrcColor
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog{Mode Off}

		Pass
	    {
			CGPROGRAM
			#pragma vertex vert_particles
	    	#pragma fragment frag_particle
	    	#pragma multi_compile_particles

			#pragma multi_compile V_CW_GLOBAL_FOG_OFF V_CW_GLOBAL_FOG_ON 

			#define V_CW_GLOBAL_VERTEX_COLOR_ON
			#define V_CW_GLOBAL_OFF
			 
			#pragma exclude_renderers d3d11_9x
			#include "cginc/Bend.cginc" 

			fixed4 frag_particle(vOutput i) : SV_Target 
			{
				fixed4 retColor = i.color * tex2D(_MainTex, i.texcoord.xy);
				retColor.rgb *= retColor.a;
				
				#ifdef V_CW_GLOBAL_FOG_ON
					retColor.rgb *= i.vfx.y;
				#endif

				return retColor;
			}

			ENDCG

		}	
	}	
	 
}
