// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/Internal-CustomReflections" {
	Properties{
		_SrcBlend("", Float) = 1
		_DstBlend("", Float) = 1
	}
		SubShader{
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityDeferredLibrary.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityGBuffer.cginc"
		#include "UnityStandardBRDF.cginc"
		#include "UnityPBSLighting.cginc"

		UNITY_DECLARE_TEXCUBE(_customCubeTexture);
		float4 _customCubeHDR;
		float4 _customCubePosition;
		float3 _customCubeRotation;
		float4 _customCubeMin;
		float4 _customCubeMax;

		inline half3 CustomBoxProjectedCubemapDirection(half3 worldRefl, float3 worldPos, float4 cubemapCenter, float4 boxMin, float4 boxMax)
		{
			// Do we have a valid reflection probe?
			UNITY_BRANCH
				if (cubemapCenter.w > 0.0)
				{
					half3 nrdir = normalize(worldRefl);

					half3 rbmax = (boxMax.xyz - worldPos) / nrdir;
					half3 rbmin = (boxMin.xyz - worldPos) / nrdir;

					half3 rbminmax = (nrdir > 0.0f) ? rbmax : rbmin;

					half fa = min(min(rbminmax.x, rbminmax.y), rbminmax.z);

					//nrdir = normalize(_customCubeRotation * nrdir);

					worldPos -= cubemapCenter.xyz;
					worldRefl = worldPos + nrdir * fa;
				}
			return worldRefl;
		}


		inline half3 UnityGI_IndirectSpecularCustom(UnityGIInput data, half occlusion, Unity_GlossyEnvironmentData glossIn)
		{
			half3 specular;

			#ifdef UNITY_SPECCUBE_BOX_PROJECTION
			// we will tweak reflUVW in glossIn directly (as we pass it to Unity_GlossyEnvironment twice for probe0 and probe1), so keep original to pass into BoxProjectedCubemapDirection
			half3 originalReflUVW = glossIn.reflUVW;
			glossIn.reflUVW = CustomBoxProjectedCubemapDirection(originalReflUVW, data.worldPos, data.probePosition[0], data.boxMin[0], data.boxMax[0]);
			#endif

		#ifdef _GLOSSYREFLECTIONS_OFF
			specular = unity_IndirectSpecColor.rgb;
		#else
			half3 env0 = Unity_GlossyEnvironment(UNITY_PASS_TEXCUBE(_customCubeTexture), data.probeHDR[0], glossIn);
			#ifdef UNITY_SPECCUBE_BLENDING
				const float kBlendFactor = 0.99999;
				float blendLerp = data.boxMin[0].w;
				UNITY_BRANCH
				if (blendLerp < kBlendFactor)
				{
					#ifdef UNITY_SPECCUBE_BOX_PROJECTION
						glossIn.reflUVW = BoxProjectedCubemapDirection(originalReflUVW, data.worldPos, data.probePosition[1], data.boxMin[1], data.boxMax[1]);
					#endif

					half3 env1 = Unity_GlossyEnvironment(UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1,unity_SpecCube0), data.probeHDR[1], glossIn);
					specular = lerp(env1, env0, blendLerp);
				}
				else
				{
					specular = env0;
				}
			#else
				specular = env0;
			#endif
		#endif

		return specular * occlusion;
		}
	ENDCG

	// Calculates reflection contribution from a single probe (rendered as cubes) or default reflection (rendered as full screen quad)
		Pass{
			ZWrite Off
			ZTest Always
			Cull Front
			Blend[_SrcBlend][_DstBlend]
	CGPROGRAM
	#pragma target 3.0
	#pragma vertex vert_deferred
	#pragma fragment frag

	sampler2D _CameraGBufferTexture0;
	sampler2D _CameraGBufferTexture1;
	sampler2D _CameraGBufferTexture2;

	half3 distanceFromAABB(half3 p, half3 aabbMin, half3 aabbMax)
	{
		return max(max(p - aabbMax, aabbMin - p), half3(0.0, 0.0, 0.0));
	}


	half4 frag(unity_v2f_deferred i) : SV_Target
	{
		// Stripped from UnityDeferredCalculateLightParams, refactor into function ?
		i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
		float2 uv = i.uv.xy / i.uv.w;

		// read depth and reconstruct world position
		float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
		depth = Linear01Depth(depth);
		float4 viewPos = float4(i.ray * depth,1);
		float3 worldPos = mul(unity_CameraToWorld, viewPos).xyz;

		half4 gbuffer0 = tex2D(_CameraGBufferTexture0, uv);
		half4 gbuffer1 = tex2D(_CameraGBufferTexture1, uv);
		half4 gbuffer2 = tex2D(_CameraGBufferTexture2, uv);
		UnityStandardData data = UnityStandardDataFromGbuffer(gbuffer0, gbuffer1, gbuffer2);

		float3 eyeVec = normalize(worldPos - _WorldSpaceCameraPos);
		eyeVec = _customCubeRotation * eyeVec;
		half oneMinusReflectivity = 1 - SpecularStrength(data.specularColor);

		half3 worldNormalRefl = reflect(eyeVec, data.normalWorld);

		worldNormalRefl = _customCubeRotation * worldNormalRefl;

		// Unused member don't need to be initialized
		UnityGIInput d;
		d.worldPos = worldPos;
		d.worldViewDir = -eyeVec;
		d.probeHDR[0] = _customCubeHDR;
		d.boxMin[0].w = 1; // 1 in .w allow to disable blending in UnityGI_IndirectSpecular call since it doesn't work in Deferred

		float blendDistance = _customCubePosition.w; // will be set to blend distance for this probe
		#ifdef UNITY_SPECCUBE_BOX_PROJECTION
		d.probePosition[0] = _customCubePosition;
		d.boxMin[0].xyz = _customCubeMin - float4(blendDistance,blendDistance,blendDistance,0);
		d.boxMax[0].xyz = _customCubeMax + float4(blendDistance,blendDistance,blendDistance,0);
		#endif

		data.smoothness = 1;

		Unity_GlossyEnvironmentData g = UnityGlossyEnvironmentSetup(data.smoothness, d.worldViewDir, data.normalWorld, data.specularColor);

		half3 env0 = UnityGI_IndirectSpecularCustom(d, data.occlusion, g);

		UnityLight light;
		light.color = half3(0, 0, 0);
		light.dir = half3(0, 1, 0);

		UnityIndirect ind;
		ind.diffuse = 0;
		ind.specular = env0;

		half3 rgb = UNITY_BRDF_PBS(0, data.specularColor, oneMinusReflectivity, data.smoothness, data.normalWorld, -eyeVec, light, ind).rgb;

		// Calculate falloff value, so reflections on the edges of the probe would gradually blend to previous reflection.
		// Also this ensures that pixels not located in the reflection probe AABB won't
		// accidentally pick up reflections from this probe.
		half3 distance = distanceFromAABB(worldPos, _customCubeMin.xyz, _customCubeMax.xyz);
		half falloff = saturate(1.0 - length(distance) / blendDistance);
		float testDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
		//return half4(half3(falloff, falloff, falloff), 1);
		//return half4(rgb*falloff, falloff);
		return half4(env0, falloff);
	}

	ENDCG
	}

		// Adds reflection buffer to the lighting buffer
		Pass
	{
		ZWrite Off
		ZTest Always
		Blend[_SrcBlend][_DstBlend]

		CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile ___ UNITY_HDR_ON

			#include "UnityCG.cginc"

			sampler2D _CameraReflectionsTexture;

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = ComputeScreenPos(o.pos).xy;
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				half4 c = tex2D(_CameraReflectionsTexture, i.uv);
				#ifdef UNITY_HDR_ON
				return float4(c.rgb, 0.0f);
				#else
				return float4(exp2(-c.rgb), 0.0f);
				#endif

			}
		ENDCG
	}

	}
		Fallback Off
}