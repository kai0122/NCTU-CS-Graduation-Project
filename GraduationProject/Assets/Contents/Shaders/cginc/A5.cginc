#ifndef BEND_A5_CGINC
#define BEND_A5_CGINC

vOutput vert(vInput v)
{ 
	vOutput o = (vOutput)0;
		
	V_CW_BEND(v.vertex)			

	#ifdef V_CW_TERRAIN
		o.texcoord.xy = v.texcoord.xy;
	#else
		o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		#ifdef V_CW_DECAL
			o.texcoord.zw = v.texcoord.xy * _DecalTex_ST.xy + _DecalTex_ST.zw;
		#elif defined(V_CW_DETAIL)
			o.texcoord.zw = v.texcoord.xy * _Detail_ST.xy + _Detail_ST.zw;
		#elif defined(V_CW_BLEND_BY_VERTEX)
			o.texcoord.zw = v.texcoord.xy * _BlendTex_ST.xy + _BlendTex_ST.zw;
		#elif defined(V_CW_BUMP)
			o.texcoord.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
		#endif

		#ifdef V_CW_UVSCROLL
			o.texcoord.xy += fixed2(_V_CV_MainTex_scrollSpeed_U, _V_CV_MainTex_scrollSpeed_V) * _Time.y;

			#ifdef V_CW_DECAL
				o.texcoord.zw += fixed2(_V_CV_DecalTex_scrollSpeed_U, _V_CV_DecalTex_scrollSpeed_V) * _Time.y;
			#endif
			#ifdef V_CW_DETAIL
				o.texcoord.zw += fixed2(_V_CV_DetailTex_scrollSpeed_U, _V_CV_DetailTex_scrollSpeed_V) * _Time.y;
			#endif
			#ifdef V_CW_BLEND_BY_VERTEX
				o.texcoord.zw += fixed2(_V_CV_SecondTex_scrollSpeed_U, _V_CV_SecondTex_scrollSpeed_V) * _Time.y;
			#endif		
		#endif
	#endif 

	#ifdef NEED_V_CALC_ROTATION
		TANGENT_SPACE_ROTATION;
	#endif

	#ifdef LIGHTMAP_ON
		o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
	#elif defined(V_CW_BUMP)		
		o.lightDir = mul (rotation, ObjSpaceLightDir(v.vertex));
	#endif
	
	#ifdef NEED_V_CALC_VIEWDIR_WS
		float3 viewDir_WS = V_WorldSpaceViewDir(v.vertex);
	#endif
	#ifdef NEED_V_CALC_VIEWDIR_OS
		float3 viewDir_OS = V_ObjSpaceViewDir(v.vertex.xyz);
	#endif
	#ifdef NEED_V_CALC_VIEWDIR_TS
		float3 viewDir_TS = mul (rotation, V_ObjSpaceViewDir(v.vertex.xyz));
	#endif
	
	
	#ifdef NEED_V_CALC_POS_WS
		half3 pos_WS = mul(unity_ObjectToWorld, v.vertex).xyz;
	#endif

	#ifdef NEED_V_CALC_NORMAL_WS
		float3 normal_WS = normalize(mul((half3x3)unity_ObjectToWorld, v.normal * 1.0));
	#endif


	#ifdef NEED_P_NORMAL_WS
		o.normal = normal_WS;
	#endif
	#ifdef NEED_P_VIEWDIR_WS
		o.viewDir.xyz = viewDir_WS;
	#endif
	#ifdef NEED_P_VIEWDIR_TS
		o.viewDir.xyz = viewDir_TS;
	#endif

	#ifdef NEED_P_REFLECTION_WS		
		o.refl.xyz = reflect(-viewDir_WS, normal_WS);

		#ifdef V_CW_FRESNEL_ON
			half fresnel = dot(v.normal, viewDir_OS);
			o.refl.w = max(0, _V_CW_Fresnel_Bias - fresnel * fresnel);
		#endif
	#endif

	#ifdef NEED_P_LIGHTDIR_WS
		o.vLight.xyz = lightDir_WS;
	#endif

	#if defined(V_CW_RIM_ON) || defined(V_CW_GLOBAL_RIM_ON)
		half rim = saturate(dot (v.normal, viewDir_OS) + _V_CW_Rim_Bias);
		o.vfx.x = rim * rim;
	#endif
	
	#if defined(V_CW_VERTEX_COLOR_ON) || defined(V_CW_GLOBAL_VERTEX_COLOR_ON) || defined(V_CW_BLEND_BY_VERTEX) || defined(V_CW_TERRAINBLEND_VERTEXCOLOR)
		o.color = v.color;
	#endif

	#ifdef UNITY_PASS_FORWARDBASE
		#ifndef LIGHTMAP_ON
		 
			#if defined(VERTEXLIGHT_ON) && defined(V_CW_UNITY_VERTEXLIGHT_ON)		
				o.vLight.rgb = Shade4PointLights ( unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
			  				 				        unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
												    unity_4LightAtten0, pos_WS, normal_WS );
			#endif

			#ifndef V_CW_LIGHT_PER_PIXEL

				o.vLight.a = (max(0, dot (normal_WS, _WorldSpaceLightPos0.xyz)));	
				 
				#ifdef V_CW_SPECULAR
					o.viewDir.w = max (0, dot(normal_WS, normalize(_WorldSpaceLightPos0.xyz + normalize(viewDir_WS))));
				#endif

			#endif
		#endif
	#endif
	


	#if defined(UNITY_PASS_FORWARDBASE) || defined(UNITY_PASS_FORWARDADD)
		#ifndef V_NO_SHADOWS
			TRANSFER_VERTEX_TO_FRAGMENT(o);
		#endif
	#endif

	return o;
}
 

vOutput vert_particles(vInput v)
{ 
	vOutput o = (vOutput)0;
	
	V_CW_BEND(v.vertex)


	o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

	#if defined(V_CW_VERTEX_COLOR_ON) || defined(V_CW_GLOBAL_VERTEX_COLOR_ON)
		o.color = v.color;
	#endif


	return o;
}

vOutput vert_text(vInput v)
{ 
	vOutput o = (vOutput)0;
	
	V_CW_BEND(v.vertex)
	

	o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

	#if defined(V_CW_VERTEX_COLOR_ON) || defined(V_CW_GLOBAL_VERTEX_COLOR_ON)
		o.color = v.color * _Color;
	#endif

	return o;
}


	
#endif