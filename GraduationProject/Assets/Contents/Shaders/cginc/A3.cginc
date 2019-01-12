#ifndef BEND_A3_CGINC
#define BEND_A3_CGINC


struct vInput
{
	float4 vertex : POSITION;
    
	float4 texcoord : TEXCOORD0;

	#ifdef LIGHTMAP_ON
		float4 texcoord1 : TEXCOORD1;
	#endif

	#if defined(NEED_V_CALC_NORMAL_WS) || defined(V_CW_RIM_ON) || defined(V_CW_GLOBAL_RIM_ON) || defined(V_CW_BUMP)
		float3 normal : NORMAL;
	#endif
        
	#ifdef V_CW_BUMP
		float4 tangent : TANGENT;
	#endif

    fixed4 color : COLOR;
};

struct vOutput
{
	float4 pos : SV_POSITION;

	half4 texcoord : TEXCOORD0;

	#ifdef LIGHTMAP_ON
		half2 lmap : TEXCOORD1;
	#elif defined(V_CW_BUMP)
		half3 lightDir : TEXCOORD1;
	#endif
				
	fixed4 vfx : TEXCOORD2;

	//Normal
	#ifdef NEED_P_NORMAL_WS
		half3 normal : TEXCOORD3; 
	#endif

	//Vertex light
	#if defined(NEED_P_VERTEX_LIGHT) 
		fixed4 vLight : TEXCOORD4;	
	#endif

	//View Dir
	#if defined(NEED_P_VIEWDIR_WS) || defined(NEED_P_VIEWDIR_TS)
		half4 viewDir : TEXCOORD5;	//xyz - viewdir, w - specular(nh)
	#endif
		
	//Reflection
	#ifdef NEED_P_REFLECTION_WS
		half4 refl : TEXCOORD6;	//xyz - refl, w - fresnel
	#endif
		 		
	//Shadows
	#if defined(UNITY_PASS_FORWARDBASE) || defined(UNITY_PASS_FORWARDADD)	
		#ifndef V_NO_SHADOWS
			LIGHTING_COORDS(7, 8)
		#endif
	#endif

	//Color
	#if defined(V_CW_VERTEX_COLOR_ON) || defined(V_CW_GLOBAL_VERTEX_COLOR_ON) || defined(V_CW_BLEND_BY_VERTEX) || defined(V_CW_TERRAINBLEND_VERTEXCOLOR)
		fixed4 color : COLOR;
	#endif
};
	
#endif