#ifndef BEND_S1_CGINC
#define BEND_S1_CGINC

#ifdef V_CW_CUTOUT
	fixed4 _Color;
	sampler2D _MainTex;
	half4 _MainTex_ST;

	half _Cutoff;
#endif

struct vOutput
{
	#ifdef UNITY_PASS_SHADOWCOLLECTOR
		V2F_SHADOW_COLLECTOR;
	#endif

	#ifdef UNITY_PASS_SHADOWCASTER
		V2F_SHADOW_CASTER;
	#endif

	#ifdef V_CW_CUTOUT
		half2 texcoord : TEXCOORD5;
	#endif
};

#ifdef UNITY_PASS_SHADOWCASTER
vOutput vert_ShadowCaster(appdata_full v)
{ 
	vOutput o;	

	V_CW_BEND(v.vertex)


	#ifdef V_CW_CUTOUT
		o.texcoord = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
	#endif

	TRANSFER_SHADOW_CASTER(o)
	 
	return o;
} 
#endif
 

#ifdef UNITY_PASS_SHADOWCOLLECTOR
vOutput vert_ShadowCollector(appdata_full v)
{ 
	vOutput o;
	
	V_CW_BEND(v.vertex)

	 
	#ifdef V_CW_CUTOUT
		o.texcoord = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
	#endif
	
	float4 wpos = mul(unity_ObjectToWorld, v.vertex); 
	o._WorldPosViewZ.xyz = wpos; 
	o._WorldPosViewZ.w = -mul( UNITY_MATRIX_MV, v.vertex ).z; 
	o._ShadowCoord0 = mul(unity_WorldToShadow[0], wpos).xyz; 
	o._ShadowCoord1 = mul(unity_WorldToShadow[1], wpos).xyz; 
	o._ShadowCoord2 = mul(unity_WorldToShadow[2], wpos).xyz; 
	o._ShadowCoord3 = mul(unity_WorldToShadow[3], wpos).xyz;

	return o;
}
#endif


#ifdef UNITY_PASS_SHADOWCASTER
half4 frag_ShadowCaster(vOutput i) : SV_Target 
{	
	#ifdef V_CW_CUTOUT
		clip(tex2D(_MainTex, i.texcoord.xy).a * _Color.a - _Cutoff);
	#endif

	SHADOW_CASTER_FRAGMENT(i)
}
#endif

#ifdef UNITY_PASS_SHADOWCOLLECTOR
half4 frag_ShadowCollector(vOutput i) : SV_Target 
{
	#ifdef V_CW_CUTOUT
		clip(tex2D(_MainTex, i.texcoord.xy).a * _Color.a - _Cutoff);
	#endif

	SHADOW_COLLECTOR_FRAGMENT(i)
}
#endif
	
#endif