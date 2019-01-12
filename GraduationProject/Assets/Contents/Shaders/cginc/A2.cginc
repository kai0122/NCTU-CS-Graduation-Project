#ifndef BEND_A2_CGINC
#define BEND_A2_CGINC


#if defined(UNITY_PASS_FORWARDBASE) && !defined(LIGHTMAP_ON)
	#define NEED_V_CALC_NORMAL_WS
	
	#if defined(V_CW_LIGHT_PER_PIXEL) || defined(V_CW_BUMP)
		#define NEED_P_NORMAL_WS
	#endif
	#if defined(V_CW_BUMP)
		#define NEED_V_CALC_ROTATION
	#endif

	#if defined(VERTEXLIGHT_ON) && defined(V_CW_UNITY_VERTEXLIGHT_ON)
		#define NEED_V_CALC_POS_WS
	#endif

	#if !defined(V_CW_LIGHT_PER_PIXEL) || defined(V_CW_UNITY_VERTEXLIGHT_ON)
		#define NEED_P_VERTEX_LIGHT
	#endif

	#ifdef V_CW_SPECULAR
		#ifdef V_CW_BUMP
			#define NEED_V_CALC_VIEWDIR_TS
			#define NEED_P_VIEWDIR_TS
		#else
			#define NEED_V_CALC_VIEWDIR_WS
			#define NEED_P_VIEWDIR_WS
		#endif
	#endif	
#endif

#if defined(UNITY_PASS_UNLIT) && !defined(LIGHTMAP_OFF) && !defined(V_CW_UNLIT_LIGHTMAP_ON)
	#ifdef LIGHTMAP_ON
	#undef LIGHTMAP_ON
	#endif

	#ifndef LIGHTMAP_OFF
	#define LIGHTMAP_OFF
	#endif
#endif

#ifdef V_CW_FRESNEL_ON
	#define NEED_V_CALC_VIEWDIR_OS
#endif
 
#ifdef V_CW_REFLECTION
	#ifndef NEED_V_CALC_NORMAL_WS
		#define NEED_V_CALC_NORMAL_WS
	#endif

	#ifndef NEED_V_CALC_VIEWDIR_WS
		#define NEED_V_CALC_VIEWDIR_WS
	#endif

	#define NEED_P_REFLECTION_WS
#endif

#if defined(V_CW_IBL_ON) || defined(V_CW_GLOBAL_IBL_ON)
	#ifndef NEED_V_CALC_NORMAL_WS
		#define NEED_V_CALC_NORMAL_WS
	#endif

	#ifndef NEED_P_NORMAL_WS
		#define NEED_P_NORMAL_WS
	#endif
#endif


#if defined(V_CW_RIM_ON) || defined(V_CW_GLOBAL_RIM_ON)
	#ifndef NEED_V_CALC_VIEWDIR_OS
		#define NEED_V_CALC_VIEWDIR_OS
	#endif 
#endif


#ifdef V_CW_BUMP
	#define V_CW_LIGHTDIR i.lightDir
#else
	#define V_CW_LIGHTDIR _WorldSpaceLightPos0.xyz
#endif

#ifdef V_CW_TERRAIN
	#define V_CW_LIGHTMAP_UV i.texcoord.zw
#else
	#define V_CW_LIGHTMAP_UV i.lmap.xy
#endif


#ifdef V_CW_GLOBAL_ON
	#define V_CW_X_BEND_SIZE        _V_CW_X_Bend_Size_GLOBAL
	#define V_CW_Y_BEND_SIZE        _V_CW_Y_Bend_Size_GLOBAL
	#define V_CW_Z_BEND_SIZE        _V_CW_Z_Bend_Size_GLOBAL
	#define V_CW_Z_BEND_BIAS        _V_CW_Z_Bend_Bias_GLOBAL
	#define V_CW_CAMERA_BEND_OFFSET _V_CW_Camera_Bend_Offset_GLOBAL
	
	#ifdef V_CW_GLOBAL_FOG_ON
		#define V_CW_FOG_COLOR _V_CW_Fog_Color_GLOBAL
		#define V_CW_FOG_DENSITY _V_CW_Fog_Density_GLOBAL
		#define V_CW_FOG_START _V_CW_Fog_Start_GLOBAL
		#define V_CW_FOG_END   _V_CW_Fog_End_GLOBAL
	#endif

	#ifdef V_CW_GLOBAL_IBL_ON
		#define V_CW_IBL_INTENSITY _V_CW_IBL_Intensity_GLOBAL
		#define V_CW_IBL_CONTRAST _V_CW_IBL_Contrast_GLOBAL
		#define V_CW_IBL_CUBE _V_CW_IBL_Cube_GLOBAL

		#define V_CW_IBL(n) ((texCUBE(_V_CW_IBL_Cube_GLOBAL, n).rgb - 0.5) * _V_CW_IBL_Contrast_GLOBAL + 0.5) * _V_CW_IBL_Intensity_GLOBAL

	#endif
#else
	#define V_CW_X_BEND_SIZE        _V_CW_X_Bend_Size
	#define V_CW_Y_BEND_SIZE        _V_CW_Y_Bend_Size
	#define V_CW_Z_BEND_SIZE        _V_CW_Z_Bend_Size
	#define V_CW_Z_BEND_BIAS        _V_CW_Z_Bend_Bias
	#define V_CW_CAMERA_BEND_OFFSET _V_CW_Camera_Bend_Offset
	
	#ifdef V_CW_FOG_ON
		#define V_CW_FOG_COLOR _V_CW_Fog_Color
		#define V_CW_FOG_DENSITY _V_CW_Fog_Density
		#define V_CW_FOG_START _V_CW_Fog_Start
		#define V_CW_FOG_END   _V_CW_Fog_End
	#endif

	#ifdef V_CW_IBL_ON
		#define V_CW_IBL_INTENSITY _V_CW_IBL_Intensity
		#define V_CW_IBL_CONTRAST _V_CW_IBL_Contrast
		#define V_CW_IBL_CUBE _V_CW_IBL_Cube
		
		#define V_CW_IBL(n) ((texCUBE(_V_CW_IBL_Cube, n).rgb - 0.5) * _V_CW_IBL_Contrast + 0.5) * _V_CW_IBL_Intensity

	#endif
#endif



#define V_CW_BEND(v)  float4 mv = mul(UNITY_MATRIX_MV, v); \
				      float zOff = min(0, mv.z + V_CW_CAMERA_BEND_OFFSET); \
				      zOff = zOff * zOff * 0.001; \
				      float xOff = max(0, abs(mv.x) - V_CW_Z_BEND_BIAS) * sign(mv.x); float4 pos = mv; \
				      pos.xy += float2(V_CW_Y_BEND_SIZE * zOff, V_CW_X_BEND_SIZE * zOff + (xOff * xOff * V_CW_Z_BEND_SIZE) * 0.001);	o.pos = mul(UNITY_MATRIX_P, pos);
					 

#endif