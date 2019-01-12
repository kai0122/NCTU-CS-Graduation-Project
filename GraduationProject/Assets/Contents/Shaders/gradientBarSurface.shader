// gradient bar shader (surface shader, supports lights) by mgear / unitycoder.com

Shader "Unitycoder/gradientBarSurfaceShader" 
{
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
		Tags { "RenderType" = "Opaque" }
		Cull Off
		CGPROGRAM
		#pragma surface surf Lambert
		
		struct Input {
		  float3 worldPos;
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

		  void surf (Input IN, inout SurfaceOutput o) 
		  {
				fixed4 c = fixed4(0,0,0,0);
				
				// calculate gradient
				float v = remap(clamp(IN.worldPos.y,_BottomLine,_TopLine),_BottomLine,_TopLine);
				c = lerp(_GradientBottomColor,_GradientTopColor, v);
				
				if (IN.worldPos.y<_BottomLine) c=_BelowBottomColor; // set below the bottomline color
				if (IN.worldPos.y>_TopLine) c=_OverTopColor; // set over the topline color
				
				o.Albedo = c;
		  }
		ENDCG
    } 
    Fallback "Diffuse"
}
