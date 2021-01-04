Shader "Hidden/HorrorBlur"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _AccumOrig;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		return float4(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).rgb, _AccumOrig);
	}

	ENDHLSL

	SubShader
	{
	Cull Off ZWrite Off ZTest Always

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB

			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}