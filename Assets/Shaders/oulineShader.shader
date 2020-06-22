Shader "Custom/RimLighting"
{
	Properties
	{
		_RimColor("Rim Color", Color) = (0,0.5,0.5,0)
		_RimPower("Rim Power", Range(0.5,8)) = 2.0
	}
		SubShader
		{
			CGPROGRAM
			#pragma surface surf Lambert

			struct Input
			{
				float3 viewDir;
				float3 worldPos;
			};

			float4 _RimColor;
			float _RimPower;

			void surf(Input IN, inout SurfaceOutput o)
			{
				half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Emission = _RimColor.rgb * pow(rim, _RimPower); //Gradient rim colour
				//o.Emission = _RimColor.rgb * rim>0.5?float3(1,0,0):0; //Set inner line to red.
				//o.Emission = rim > 0.5 ? float3(1, 0, 0) : rim > 0.3 ? float3(0, 1, 0) : 0; // green and red rim.

			}
			ENDCG
		}
			FallBack "Diffuse"
}
