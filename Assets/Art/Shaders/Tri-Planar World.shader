Shader "Tri-Planar World" {
	Properties{
		_Top("Texture", 2D) = "white" {}
		_TopScale("Scale", Float) = 2
	}

		SubShader{
		Tags{
		"Queue" = "Geometry"
		"IgnoreProjector" = "False"
		"RenderType" = "Opaque"
	}

		Cull Back
		ZWrite On

		CGPROGRAM
#pragma surface surf Lambert
#pragma exclude_renderers flash

		sampler2D _Top;
	float _TopScale;

	struct Input {
		float3 worldPos;
		float3 worldNormal;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		float3 projNormal = saturate(pow(IN.worldNormal * 1.4, 4));

		// SIDE X
		float3 x = tex2D(_Top, frac(IN.worldPos.zy * _TopScale)) * abs(IN.worldNormal.x);

		// TOP / BOTTOM
		float3 y = 0;
		if (IN.worldNormal.y > 0) {
			y = tex2D(_Top, frac(IN.worldPos.zx * _TopScale)) * abs(IN.worldNormal.y);
		}
		else {
			y = tex2D(_Top, frac(IN.worldPos.zx * _TopScale)) * abs(IN.worldNormal.y);
		}

		// SIDE Z	
		float3 z = tex2D(_Top, frac(IN.worldPos.xy * _TopScale)) * abs(IN.worldNormal.z);

		o.Albedo = z;
		o.Albedo = lerp(o.Albedo, x, projNormal.x);
		o.Albedo = lerp(o.Albedo, y, projNormal.y);
	}
	ENDCG
	}
		Fallback "Diffuse"
}