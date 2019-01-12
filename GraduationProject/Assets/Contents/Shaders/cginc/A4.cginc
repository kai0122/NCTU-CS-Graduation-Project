#ifndef BEND_A4_CGINC
#define BEND_A4_CGINC


#ifdef LIGHTMAP_ON
inline fixed3 V_DecodeLightmap( fixed4 color )
{
	#if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) && defined(SHADER_API_MOBILE)
		return 2.0 * color.rgb;
	#else
		return (8.0 * color.a) * color.rgb;
	#endif
}
#endif

inline float3 V_WorldSpaceViewDir( in float4 v )
{
	return _WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v).xyz;
}

inline half3 V_ObjSpaceViewDir ( half3 vertexPos )
{				
	half3 objSpaceCameraPos = mul(unity_WorldToObject, half4(_WorldSpaceCameraPos.xyz, 1)).xyz * 1.0;
				
	return normalize(objSpaceCameraPos - vertexPos);
}


	
#endif