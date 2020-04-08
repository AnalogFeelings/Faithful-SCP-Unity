Shader "Hidden/NGSS_ContactShadows"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		CGINCLUDE

		#pragma vertex vert
		#pragma fragment frag
		#pragma exclude_renderers gles d3d9
		#pragma target 3.0

		#include "UnityCG.cginc"
		half4 _MainTex_ST;
		/*
#if !defined(UNITY_SINGLE_PASS_STEREO)
#define UnityStereoTransformScreenSpaceTex(uv) (uv)
#endif*/
		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			float2 uv : TEXCOORD0;
			//float2 uv2 : TEXCOORD0;
		};

		v2f vert (appdata v)
		{
			v2f o = (v2f)0;

			o.vertex = UnityObjectToClipPos(v.vertex);
			//o.uv = v.uv;//NGSS 2.0
			o.uv = ComputeNonStereoScreenPos(o.vertex).xy;//NGSS 2.0
			
			//o.uv = UnityStereoTransformScreenSpaceTex(v.uv);
			
			#if UNITY_UV_STARTS_AT_TOP
			//o.uv2 = UnityStereoTransformScreenSpaceTex(v.uv);
			if (_MainTex_ST.y < 0.0)
				o.uv.y = 1.0 - o.uv.y;
			#endif
			return o;
		}

		ENDCG
		
		Pass // clip edges
		{
			CGPROGRAM
			
			sampler2D _CameraDepthTexture;
			half4 _CameraDepthTexture_ST;

			fixed4 frag (v2f input) : SV_Target
			{
				float depth = tex2D(_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(input.uv)).r;
				//float depth = tex2D(_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(input.uv, _CameraDepthTexture_ST)).r;

				if (input.vertex.x <= 1.0 || input.vertex.x >= _ScreenParams.x - 1.0 ||  input.vertex.y <= 1.0 || input.vertex.y >= _ScreenParams.y - 1.0)
				{
					#if defined(UNITY_REVERSED_Z)
						depth = 0.0;
					#else
						depth = 1.0;
					#endif
				}

				return depth.xxxx;
			}
			ENDCG
		}

		Pass // render screen space rt shadows
		{
			CGPROGRAM
			//#pragma multi_compile _ NGSS_CONTACT_SHADOWS_USE_NOISE
			#pragma shader_feature NGSS_CONTACT_SHADOWS_USE_NOISE
			#pragma shader_feature NGSS_USE_LOCAL_CONTACT_SHADOWS
			
			sampler2D _MainTex;
			//sampler2D ScreenSpaceMask;
			//half4 _MainTex_ST;
			
			//float4x4 unity_CameraInvProjection;
			//float4x4 InverseView;//camera to world
			//float4x4 InverseViewProj;//clip to world
			//float4x4 InverseProj;//clip to camera			
			float4x4 WorldToView;//world to camera
			
			float3 LightDir;
			float3 LightPos;
			float ShadowsDistance;
			float RaySamples;
			float RayWidth;
			float ShadowsFade;
			float ShadowsBias;
			float RaySamplesScale;

			#define ditherPattern float4x4(0.0,0.5,0.125,0.625, 0.75,0.22,0.875,0.375, 0.1875,0.6875,0.0625,0.5625, 0.9375,0.4375,0.8125,0.3125)
			/*
			const static float ditherPattern[4][4] = {
				{ 0.0f, 0.5f, 0.125f, 0.625f},
				{ 0.75f, 0.22f, 0.875f, 0.375f},
				{ 0.1875f, 0.6875f, 0.0625f, 0.5625},
				{ 0.9375f, 0.4375f, 0.8125f, 0.3125} };
			*/
			
			fixed4 frag (v2f input) : SV_Target
			{
				//Early out?
				//float mask = tex2D(_MainTex, input.uv).r;
				//if (mask < 0.001) return mask;
				
				float2 coord = input.uv;
				float shadow = 1.0;
				float depth = tex2Dlod(_MainTex, float4(UnityStereoTransformScreenSpaceTex(coord.xy), 0, 0)).r;
				//float depth = tex2Dlod(_MainTex, float4(UnityStereoScreenSpaceUVAdjust(coord.xy, _MainTex_ST), 0, 0)).r;
								
				#if defined(UNITY_REVERSED_Z)
					depth = 1.0 - depth;
				#endif

				coord.xy = coord.xy * 2.0 - 1.0;
				float4 clipPos = float4(coord.xy, depth * 2.0 - 1.0, 1.0);
				
				float4 viewPos = mul(unity_CameraInvProjection, clipPos);//go from clip to view space | InverseProj
				viewPos.xyz /= viewPos.w;
				//viewPos.z *= -1;
				
				float samplers = lerp(RaySamples / -viewPos.z, RaySamples, RaySamplesScale);//reduce samplers over distance
				#if defined(NGSS_USE_LOCAL_CONTACT_SHADOWS)
				float3 lightDir = normalize(mul(WorldToView, float4(LightPos.xyz, 1.0)) - viewPos).xyz;//W == 1.0 treat as position | W == 0.0 treat as direction
				float3 rayDir = lightDir * (ShadowsDistance / samplers);	
				#else				
				//float3 lightDir = normalize(mul(WorldToView, float4(LightPos.xyz, 1.0)) - viewPos).xyz;//W == 1.0 treat as position | W == 0.0 treat as direction
				//float3 rayDir = lightDir * (ShadowsDistance / samplers);				
				float3 rayDir = LightDir * float3(1.0, 1.0, -1.0) * (ShadowsDistance / samplers);
				#endif
				#if defined(NGSS_CONTACT_SHADOWS_USE_NOISE)
				//float3 rayPos = viewPos + rayDir * saturate(frac(sin(dot(coord, float2(12.9898, 78.223))) * 43758.5453));//NGSS 2.0
				float3 rayPos = viewPos + rayDir * saturate(ditherPattern[input.uv.x * _ScreenParams.x % 4][input.uv.y * _ScreenParams.y % 4]);//NGSS 2.0
				#else
				float3 rayPos = viewPos + rayDir;
				#endif
				
				rayPos -= (viewPos.z * ShadowsBias);

				for (float i = 0; i < samplers; i++)
				{
					rayPos += rayDir;
					
					float4 rayPosProj = mul(unity_CameraProjection, float4(rayPos.xyz, 0.0));
					rayPosProj.xyz = rayPosProj.xyz / rayPosProj.w * 0.5 + 0.5;
					
					float lDepth = LinearEyeDepth(tex2Dlod(_MainTex, float4(UnityStereoTransformScreenSpaceTex(rayPosProj.xy), 0, 0)).r);
					//float lDepth = LinearEyeDepth(tex2Dlod(_MainTex, float4(UnityStereoScreenSpaceUVAdjust(rayPosProj.xy, _MainTex_ST), 0, 0)).r);

					float depthDiff = -rayPos.z - lDepth;// + (viewPos.z * ShadowsBias);//0.02
					shadow *= (depthDiff > 0.0 && depthDiff < RayWidth)? i / samplers * ShadowsFade : 1.0;
				}
				
				return shadow.rrrr;
			}
			ENDCG
		}
		/*
		Pass // poison blur
		{
			CGPROGRAM			

			sampler2D _MainTex;
			//half4 _MainTex_ST;

			//sampler2D _CameraDepthTexture;

			float ShadowsSoftness;
			float ShadowsOpacity;

			static float2 poissonDisk[9] =
			{
				float2 ( 0.4636456f, 0.3294131f),
				float2 ( 0.3153244f, 0.8371656f),
				float2 ( 0.7389247f, -0.3152522f),
				float2 ( -0.1819379f, -0.3826133f),
				float2 ( -0.38396f, 0.2479579f),
				float2 ( 0.1985026f, -0.8434925f),
				float2 ( -0.25466f, 0.9213307f),
				float2 ( -0.8729509f, -0.3795996f),
				float2 ( -0.8918442f, 0.3004266f)
			};

			float rand01(float2 seed)
			{
			   float dt = dot(seed, float2(12.9898,78.233));// project seed on random constant vector   
			   return frac(sin(dt) * 43758.5453);// return only fractional part
			}

			// returns random angle
			float randAngle(float2 seed)
			{
				return rand01(seed)*6.283285;
			}

			fixed4 frag(v2f input) : COLOR0
			{
				float result = 0.0;//tex2Dlod(_MainTex, float4(input.uv.xy, 0, 0)).r;
				ShadowsSoftness *= (_ScreenParams.zw - 1.0);
				//float angle = randAngle(input.uv.xy);
				//float s = sin(angle);
				//float c = cos(angle);

				//float lDepth = LinearEyeDepth(tex2Dlod(_CameraDepthTexture, float4(input.uv, 0, 0))) * 0.5;

				for(int i = 0; i < 9; ++i)
				{
					//float2 offs = float2(poissonDisk[i].x * c + poissonDisk[i].y * s, poissonDisk[i].x * -s + poissonDisk[i].y * c) * ShadowsSoftness;//rotated samples
					float2 offs = poissonDisk[i] * ShadowsSoftness;// / lDepth;//no rotation
					//result += tex2Dlod(_MainTex, float4(input.uv + offs.xy, 0, 0)).r;
					result += tex2D(_MainTex, UnityStereoTransformScreenSpaceTex(input.uv + offs.xy)).r;
				}

				result /= 9.0;
				result += ShadowsOpacity;//faster opacity
				//result = lerp(ShadowsOpacity, 1.0, result);
				return result.xxxx;
			}

			ENDCG
		}*/

		Pass // bilateral blur (taking into account screen depth)
		{
			CGPROGRAM			

			sampler2D _CameraDepthTexture;

			float ShadowsSoftness;
			float ShadowsOpacity;

			sampler2D _MainTex;
			float2 ShadowsKernel;
			float ShadowsEdgeTolerance;

			fixed4 frag(v2f input) : COLOR0
			{
				float weights = 0.0;
				float result = 0.0;
				float2 offsets = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y) * ShadowsKernel.xy * ShadowsSoftness;

				float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(input.uv)));
				offsets /= depth;//adjust kernel size over distance

				for (float i = -1; i <= 1; i++)
				{
					float2 offs = i * offsets;
					float curDepth = LinearEyeDepth(tex2Dlod(_CameraDepthTexture, float4(input.uv + offs.xy, 0, 0)));

					float curWeight = saturate(1.0 - abs(depth - curDepth) / ShadowsEdgeTolerance);

					float blurSample = tex2D(_MainTex, UnityStereoTransformScreenSpaceTex(input.uv + offs.xy)).r;
					result += blurSample * curWeight;
					weights += curWeight;
				}

				result /= weights;//weights + 0.001

				return result.xxxx;
			}

			ENDCG
		}

		Pass // final mix
		{
			BlendOp Min
			Blend DstColor Zero

			CGPROGRAM
			
			sampler2D NGSS_ContactShadowsTexture;
			half4 NGSS_ContactShadowsTexture_ST;
			float ShadowsOpacity;

			fixed4 frag (v2f input) : SV_Target
			{
				//return tex2D(NGSS_ContactShadowsTexture, input.uv);
				return saturate(tex2D(NGSS_ContactShadowsTexture, UnityStereoScreenSpaceUVAdjust(input.uv, NGSS_ContactShadowsTexture_ST)) + ShadowsOpacity);
			}
			ENDCG
		}
	}
	Fallback Off
}
