#ifndef BEND_A6_CGINC
#define BEND_A6_CGINC


fixed4 frag(vOutput i) : SV_Target 
{		
	#ifdef V_CW_TERRAIN
		#ifdef V_CW_TERRAINBLEND_VERTEXCOLOR
			half4 splat_control = i.color;
		#else
			half4 splat_control = tex2D (_V_CW_Control, i.texcoord.xy);
		#endif

		half4 mainTex  = splat_control.r * tex2D (_V_CW_Splat1, i.texcoord.xy * _V_CW_Splat1_uvScale);
		mainTex += splat_control.g * tex2D (_V_CW_Splat2, i.texcoord.xy * _V_CW_Splat2_uvScale);

		#ifdef V_CW_TERRAIN_3TEX 
			mainTex += splat_control.b * tex2D (_V_CW_Splat3, i.texcoord.xy * _V_CW_Splat3_uvScale);
		#endif

		#ifdef V_CW_TERRAIN_4TEX 
			mainTex += splat_control.a * tex2D (_V_CW_Splat4, i.texcoord.xy * _V_CW_Splat4_uvScale);
		#endif

		fixed4 retColor = mainTex;


		#ifdef V_CW_BUMP
			half4 terrainNormal = splat_control.r * tex2D (_V_CW_Splat1_Bump, i.texcoord.xy * _V_CW_Splat1_uvScale); 
			terrainNormal      += splat_control.g * tex2D (_V_CW_Splat2_Bump, i.texcoord.xy * _V_CW_Splat2_uvScale); 
		#endif

	#else
		half4 mainTex = tex2D(_MainTex, i.texcoord.xy);

		#ifdef V_CW_DETAIL
			fixed4 retColor = mainTex * _Color;
			retColor.rgb *= tex2D(_Detail, i.texcoord.zw).rgb * 2;
		#elif defined(V_CW_DECAL)
			fixed4 decal = tex2D(_DecalTex, i.texcoord.zw);
			fixed4 retColor = fixed4(lerp(mainTex.rgb, decal.rgb, decal.a), mainTex.a) * _Color;
		#elif defined(V_CW_BLEND_BY_VERTEX)
			fixed vBlend = clamp(_VertexBlend + i.color.a, 0, 1);
			fixed4 retColor = lerp(mainTex, tex2D(_BlendTex, i.texcoord.zw), vBlend) * _Color;
		#else
			fixed4 retColor = mainTex * _Color;
		#endif

		#ifdef V_CW_VERTEX_COLOR_ON
			retColor *= i.color;
		#endif 
	#endif


	#ifdef V_CW_CUTOUT
		#ifdef V_CW_COUTOUT_SOFTEDGE
			clip(-(retColor.a - _Cutoff));
		#else
			clip(retColor.a - _Cutoff);
		#endif
	#endif

	#ifdef V_CW_SELF_ILLUMINATED_ON
		fixed3 albedo = retColor.rgb;
	#endif


	#ifdef LIGHTMAP_ON
		fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, V_CW_LIGHTMAP_UV);
		fixed3 diff = V_DecodeLightmap (lmtex);
	#endif


	#ifndef LIGHTMAP_ON
		#ifdef UNITY_PASS_FORWARDBASE		
			
			#ifdef V_NO_SHADOWS
				fixed atten = 1;
			#else
				fixed atten = LIGHT_ATTENUATION(i);
			#endif

			#ifdef V_CW_LIGHT_PER_PIXEL
				
				#ifdef V_CW_BUMP
					#ifdef V_CW_TERRAIN
						i.normal = UnpackNormal(terrainNormal);
					#else
						i.normal = UnpackNormal(tex2D(_BumpMap, i.texcoord.zw));
					#endif
				#else
					i.normal = normalize(i.normal);
				#endif

				fixed3 diff = (_LightColor0.rgb * max(0, dot(i.normal, V_CW_LIGHTDIR)) * atten + UNITY_LIGHTMODEL_AMBIENT.xyz) * 2;
					
				#ifdef V_CW_UNITY_VERTEXLIGHT_ON
					diff += i.vLight.rgb;
				#endif
					
				#ifdef V_CW_SPECULAR  
					half nh = max (0, dot (i.normal, normalize (V_CW_LIGHTDIR + normalize(i.viewDir.xyz))));
					fixed3 specular = tex2D(_V_CW_Specular_Lookup, half2(nh, 0.5)).rgb * mainTex.a * _LightColor0.rgb * atten * _V_CW_Specular_Intensity;
				#endif

			#else
				fixed3 diff = (_LightColor0.rgb * i.vLight.a * atten + UNITY_LIGHTMODEL_AMBIENT.xyz) * 2;
				
				#ifdef V_CW_UNITY_VERTEXLIGHT_ON
					diff += i.vLight.rgb;
				#endif

				#ifdef V_CW_SPECULAR
					fixed3 specular = tex2D(_V_CW_Specular_Lookup, half2(i.viewDir.w, 0.5)).rgb * mainTex.a * _LightColor0.rgb * atten * _V_CW_Specular_Intensity;
				#endif	
							
			#endif
		#endif	
	#endif


	//IBL
	#ifdef UNITY_PASS_FORWARDBASE
		#if defined(V_CW_IBL_ON) || defined(V_CW_GLOBAL_IBL_ON)
			//diff += V_CW_IBL(i.normal);									
		#endif	
				
		retColor.rgb = diff * retColor.rgb;		

		#if !defined(LIGHTMAP_ON) && defined(V_CW_SPECULAR)
			retColor.rgb += specular;
		#endif
	#endif
	#ifdef UNITY_PASS_UNLIT
		#ifdef LIGHTMAP_ON
			#if defined(V_CW_IBL_ON) || defined(V_CW_GLOBAL_IBL_ON)
				//retColor.rgb = (diff + V_CW_IBL(i.normal)) * retColor.rgb;									
			#else
				retColor.rgb *= diff;
			#endif	
		#else
			#if defined(V_CW_IBL_ON) || defined(V_CW_GLOBAL_IBL_ON)
				//retColor.rgb = (UNITY_LIGHTMODEL_AMBIENT.xyz * 2 + V_CW_IBL(i.normal)) * retColor.rgb;									
			#endif	
		#endif
	#endif


	#ifdef V_CW_SELF_ILLUMINATED_ON
		retColor.rgb += albedo * mainTex.a * _V_CW_Emission_Strength;
	#endif


	#ifdef V_CW_REFLECTION
		#ifdef V_CW_BUMP
			#ifndef LIGHTMAP_ON
				i.refl.xyz += i.normal.xyz;
			#endif
			fixed4 reflTex = texCUBE( _Cube, i.refl.xyz ) * _ReflectColor;
		#else
			fixed4 reflTex = texCUBE( _Cube, i.refl.xyz ) * _ReflectColor;
		#endif

		#ifdef V_CW_FRESNEL_ON
			retColor.rgb += reflTex.rgb * mainTex.a * i.refl.w;
		#else
			retColor.rgb += reflTex.rgb * mainTex.a;
		#endif
	#endif

	
	#if defined(V_CW_RIM_ON) || defined(V_CW_GLOBAL_RIM_ON)
		retColor.rgb = lerp(_V_CW_Rim_Color.rgb, retColor.rgb, i.vfx.x);
	#endif



	return retColor; 
}
	
	
#endif