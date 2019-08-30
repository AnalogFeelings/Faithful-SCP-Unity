// Upgrade NOTE: commented out 'float4x4 _CameraToWorld', a built-in variable
// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// http://www.popekim.com/2012/10/siggraph-2012-screen-space-decals-in.html

Shader "Decal/DecalShader Diffuse+Normals"
{
	Properties
	{
		_MainTex ("Albedos", 2D) = "white" {}
		_BumpMap ("Normals", 2D) = "bump" {}

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
		[Enum(Metallic Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}
	}
		CGINCLUDE
#define UNITY_SETUP_BRDF_INPUT MetallicSetup
		ENDCG
	SubShader
	{
		Pass
		{
			Tags { "LightMode" = "Deferred" }
			Fog { Mode Off } // no fog in g-buffers pass
			ZWrite Off
			//Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers nomrt
			#pragma shader_feature _METALLICGLOSSMAP
		#pragma multi_compile_prepassfinal
			#pragma multi_compile_instancing
		#pragma vertex vertDeferred
			#pragma fragment fragDeferred
		#include "UnityStandardCore.cginc"

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				float4 screenUV : TEXCOORD1;
				float3 ray : TEXCOORD2;
				half3 orientation : TEXCOORD3;
				half3 orientationX : TEXCOORD4;
				half3 orientationZ : TEXCOORD5;
			};

			v2f vert (float3 v : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (float4(v,1));
				o.uv = v.xz+0.5;
				o.screenUV = ComputeScreenPos (o.pos);
				o.ray = mul (UNITY_MATRIX_MV, float4(v,1)).xyz * float3(-1,-1,1);
				o.orientation = mul ((float3x3)unity_ObjectToWorld, float3(0,1,0));
				o.orientationX = mul ((float3x3)unity_ObjectToWorld, float3(1,0,0));
				o.orientationZ = mul ((float3x3)unity_ObjectToWorld, float3(0,0,1));
				return o;
			}

			CBUFFER_START(UnityPerCamera2)
			// float4x4 _CameraToWorld;
			CBUFFER_END

			sampler2D __MainTex;
			sampler2D __BumpMap;
			sampler2D_float _CameraDepthTexture;
			sampler2D _NormalsCopy;

			void frag(v2f i, out half4 outDiffuse : COLOR0, out half4 outNormal : COLOR1)
			{
				i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
				float2 uv = i.screenUV.xy / i.screenUV.w;
				// read depth and reconstruct world position
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
				depth = Linear01Depth (depth);
				float4 vpos = float4(i.ray * depth,1);
				float3 wpos = mul (unity_CameraToWorld, vpos).xyz;
				float3 opos = mul (unity_WorldToObject, float4(wpos,1)).xyz;

				clip (float3(0.5,0.5,0.5) - abs(opos.xyz));


				i.uv = opos.xz+0.5;

				half3 normal = tex2D(_NormalsCopy, uv).rgb;
				fixed3 wnormal = normal.rgb * 2.0 - 1.0;
				clip (dot(wnormal, i.orientation) - 0.3);

				fixed4 col = tex2D (_MainTex, i.uv);
				clip (col.a - 0.2);
				outDiffuse = col;

				fixed3 nor = UnpackNormal(tex2D(_BumpMap, i.uv));
				half3x3 norMat = half3x3(i.orientationX, i.orientationZ, i.orientation);
				nor = mul (nor, norMat);
				outNormal = fixed4(nor*0.5+0.5,1);

			}
			ENDCG
		}		

	}

	Fallback Off
}
