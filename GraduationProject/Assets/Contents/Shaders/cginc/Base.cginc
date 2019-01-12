#ifndef VACUUM_CURVEDWORLD_BASE_CGINC
#define VACUUM_CURVEDWORLD_BASE_CGINC

////////////////////////////////////////////////////////////////////////////
//																		  //
//Variables 															  //
//																		  //
////////////////////////////////////////////////////////////////////////////



#ifdef V_CW_IBL_ON
	half _V_CW_IBL_Intensity;
	half _V_CW_IBL_Contrast;
	samplerCUBE _V_CW_IBL_Cube;	
#endif

#ifdef V_CW_FOG_ON
	fixed4 _V_CW_Fog_Color;
	fixed _V_CW_Fog_Density;
	half _V_CW_Fog_Start;
	half _V_CW_Fog_End;
#endif


////////////////////////////////////////////////////////////////////////////
//																		  //
//Defines    															  //
//																		  //
////////////////////////////////////////////////////////////////////////////


#ifdef V_CW_FOG_ON
	#define V_CW_FOG_COLOR _V_CW_Fog_Color
	#define V_CW_FOG_DENSITY _V_CW_Fog_Density
	#define V_CW_FOG_START _V_CW_Fog_Start
	#define V_CW_FOG_END _V_CW_Fog_End

	#define V_CW_FOG saturate((_V_CW_Fog_End - length(mv.xyz) * _V_CW_Fog_Density) / (_V_CW_Fog_End - _V_CW_Fog_Start));
#endif

#ifdef V_CW_IBL_ON
	#define V_CW_IBL_INTENSITY _V_CW_IBL_Intensity
	#define V_CW_IBL_CONTRAST _V_CW_IBL_Contrast
	#define V_CW_IBL_CUBE _V_CW_IBL_Cube

	#define V_CW_IBL(n) ((texCUBE(_V_CW_IBL_Cube, n).rgb - 0.5) * _V_CW_IBL_Contrast + 0.5) * _V_CW_IBL_Intensity
#endif
#endif

#define V_CW_BEND(v)  float4 mv = mul(UNITY_MATRIX_MV, v); \
				      float zOff = min(0, mv.z); \
				      zOff = zOff * zOff * 0.001; \
				      float xOff = max(0, abs(mv.x)) * sign(mv.x); \
				      float4 pos = mv + float4(1 * zOff, 1 * zOff + (xOff * xOff * 1) * 0.001, 0, 0);	o.pos = mul(UNITY_MATRIX_P, pos);


