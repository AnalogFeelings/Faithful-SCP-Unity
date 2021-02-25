// Upgrade NOTE: commented out 'float4x4 _CameraToWorld', a built-in variable
// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// http://www.popekim.com/2012/10/siggraph-2012-screen-space-decals-in.html

Shader "Decal/DecalShader Full"
{
	Properties
	{
		_MainTex ("Diffuse", 2D) = "white" {}
		_PBR ("Metalness", 2D) = "black" {}
		_BumpMap ("Normals", 2D) = "bump" {}
	}
	SubShader
	{
		Pass
		{
			Fog { Mode Off } // no fog in g-buffers pass
			ZWrite Off
			Blend 0 SrcAlpha OneMinusSrcAlpha
			Blend 1 SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			Blend 2 SrcAlpha OneMinusSrcAlpha
			Blend 3 SrcAlpha OneMinusSrcAlpha
			
			ColorMask RGB 0
			ColorMask RGBA 1
			ColorMask RGB 2
			ColorMask RGB 3
			

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers nomrt
			#pragma multi_compile _ UNITY_HDR_ON
			
			#include "UnityStandardBRDF.cginc"
			#include "UnityStandardUtils.cginc"

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
				o.ray = UnityObjectToViewPos(float4(v,1)).xyz * float3(-1,-1,1);
				o.orientation = normalize(mul ((float3x3)unity_ObjectToWorld, float3(0,1,0)));
				o.orientationX = normalize(mul ((float3x3)unity_ObjectToWorld, float3(1,0,0)));
				o.orientationZ = normalize(mul ((float3x3)unity_ObjectToWorld, float3(0,0,1)));
				return o;
			}

			CBUFFER_START(UnityPerCamera2)
			// float4x4 _CameraToWorld;
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _PBR;
			sampler2D _BumpMap;
			sampler2D_float _CameraDepthTexture;
			sampler2D _NormalsCopy;

			void frag(v2f i, out half4 outDiffuse : COLOR0, out half4 outSpec : COLOR1, out half4 outNormal : COLOR2, out half4 outLightCum : COLOR3)
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
				clip (col.a - 0.05);
				fixed4 pbr = tex2D (_PBR, i.uv);
				
				fixed3 albedo = col.rgb;
				fixed3 specularTint;
				fixed oneMinusReflectivity;
				
				albedo = DiffuseAndSpecularFromMetallic(albedo, pbr.r, specularTint, oneMinusReflectivity);
				
				
				//clip (col.a - 0.2);
				outDiffuse = fixed4(albedo, col.a);
				outSpec = fixed4(specularTint, col.a);

				fixed3 nor = UnpackNormal(tex2D(_BumpMap, i.uv));
				half3x3 norMat = half3x3(i.orientationX, i.orientationZ, i.orientation);
				nor = mul (nor, norMat);
				outNormal = fixed4(nor*0.5+0.5,col.a);
				
				float3 shColor = ShadeSH9(float4(nor, 1)) * albedo;
				
				#if !defined(UNITY_HDR_ON)
					shColor = exp2(-shColor);
				#endif
				
				outLightCum = fixed4(shColor, col.a);

			}
			ENDCG
		}	
		Pass
		{
			Fog { Mode Off } // no fog in g-buffers pass
			ZWrite Off
			Blend 1 Zero One, DstAlpha SrcAlpha
			ColorMask A 1

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers nomrt
			#pragma multi_compile _ UNITY_HDR_ON
			
			#include "UnityStandardBRDF.cginc"
			#include "UnityStandardUtils.cginc"

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
				o.ray = UnityObjectToViewPos(float4(v,1)).xyz * float3(-1,-1,1);
				o.orientation = normalize(mul ((float3x3)unity_ObjectToWorld, float3(0,1,0)));
				o.orientationX = normalize(mul ((float3x3)unity_ObjectToWorld, float3(1,0,0)));
				o.orientationZ = normalize(mul ((float3x3)unity_ObjectToWorld, float3(0,0,1)));
				return o;
			}

			CBUFFER_START(UnityPerCamera2)
			// float4x4 _CameraToWorld;
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _PBR;
			sampler2D_float _CameraDepthTexture;
			sampler2D _NormalsCopy;

			void frag(v2f i, out half4 outSpec : COLOR1)
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
				fixed4 pbr = tex2D (_PBR, i.uv);
				pbr.a = lerp(0.5, pbr.a, col.a);
				outSpec = fixed4(1, 1, 1, pbr.a);
				//outSpec = fixed4(col.a, col.a, col.a, pbr.a);
			}
			ENDCG
		}				

	}

	Fallback Off
}
