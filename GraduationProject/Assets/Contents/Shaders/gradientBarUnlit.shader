// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// gradient bar shader (fragment shader, unlit) by mgear / unitycoder.com

Shader "Unitycoder/gradientBarUnlit" {
	Properties {
		_TopLine("Top Line Y", Float) = 0
		_BottomLine("Bottom Line Y", Float) = 0
		_OverTopColor ("OverTheTopColor", Color) = (1,1,1,1)
		_GradientTopColor ("GradientTopColor", Color) = (1,0,0,1)
		_GradientBottomColor ("GradientBottomColor", Color) = (0,1,0,0)
		_BelowBottomColor ("BelowTheBottomColor", Color) = (0,0,0,0)
		}

SubShader 
{
	Tags { "RenderType"="Opaque" }
	LOD 100
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
			};

			fixed _TopLine;
			fixed _BottomLine;
			fixed4 _OverTopColor;
			fixed4 _BelowBottomColor;
			fixed4 _GradientTopColor;
			fixed4 _GradientBottomColor;

			// remap value to 0-1 range
			float remap(float value, float minSource, float maxSource)
			{
				return (value-minSource)/(maxSource-minSource);
			}

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			
			fixed4 frag (v2f IN) : COLOR
			{
				fixed4 c = fixed4(0,0,0,0);
				// calculate gradient
				float v = remap(clamp(IN.worldPos.y,_BottomLine,_TopLine),_BottomLine,_TopLine);
				c = lerp(_GradientBottomColor,_GradientTopColor, v);
				if (IN.worldPos.y<_BottomLine) c=_BelowBottomColor; // set below the bottomline color
				if (IN.worldPos.y>_TopLine) c=_OverTopColor; // set over the topline color
				return c;
			}
		ENDCG
	}
}
}