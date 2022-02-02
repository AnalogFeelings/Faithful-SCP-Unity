// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

#ifndef AUTOLIGHT_INCLUDED
#define AUTOLIGHT_INCLUDED

#include "HLSLSupport.cginc"
#include "UnityShadowLibrary.cginc"

#if (UNITY_VERSION < 2017)
#define unityShadowCoord float
#define unityShadowCoord2 float2
#define unityShadowCoord3 float3
#define unityShadowCoord4 float4
#define unityShadowCoord4x4 float4x4
#endif

// ----------------
//  Shadow helpers
// ----------------

// If none of the keywords are defined, assume directional?
#if !defined(POINT) && !defined(SPOT) && !defined(DIRECTIONAL) && !defined(POINT_COOKIE) && !defined(DIRECTIONAL_COOKIE)
    #define DIRECTIONAL
#endif
#ifndef SHADOWMAPSAMPLER_AND_TEXELSIZE_DEFINED
#define SHADOWMAPSAMPLER_AND_TEXELSIZE_DEFINED
float4 _ShadowMapTexture_TexelSize;		
#endif
// ---- Screen space direction light shadows helpers (any version)
#if defined (SHADOWS_SCREEN)

	#if defined(UNITY_NO_SCREENSPACE_SHADOWS)//CASCADED SHADOWS OFF
			
		UNITY_DECLARE_SHADOWMAP(_ShadowMapTexture);
        #define TRANSFER_SHADOW(a) a._ShadowCoord = mul( unity_WorldToShadow[0], mul( unity_ObjectToWorld, v.vertex ) );
		#define NGSS_SHADOWS_DITHERING_OPTIMIZED
		#define NGSS_ROTATED_DISK_OPTIMIZED
		#define ditherPatternOptimized float4x4(0.0,0.5,0.125,0.625, 0.75,0.22,0.875,0.375, 0.1875,0.6875,0.0625,0.5625, 0.9375,0.4375,0.8125,0.3125)
		
		#ifndef NGSS_PCSS_RANDOM_ROTATED_TEXTURE_DEFINED
		#define NGSS_PCSS_RANDOM_ROTATED_TEXTURE_DEFINED
		uniform sampler2D unity_RandomRotation16;
		#endif
		uniform float NGSS_BANDING_TO_NOISE_RATIO_DIR = 1;
		uniform uint NGSS_OPTIMIZED_ITERATIONS = 5;
		uniform uint NGSS_OPTIMIZED_SAMPLERS = 5;
		uniform float NGSS_GLOBAL_SOFTNESS_OPTIMIZED = 0.05;
		static const float MaxKernelSize = 9.0f;
		//static const float kernel[5] = { 0.198005, 0.200995, 0.202001, 0.200995, 0.198005 };
		
		float InterleavedGradientNoiseOptimized(float2 position_screen)
		{
			#if defined(NGSS_SHADOWS_DITHERING_OPTIMIZED)//defined(SHADOWS_SOFT)
			/*
			const static float ditherPatternOptimized[4][4] = {
				{ 0.0f, 0.5f, 0.125f, 0.625f},
				{ 0.75f, 0.22f, 0.875f, 0.375f},
				{ 0.1875f, 0.6875f, 0.0625f, 0.5625},
				{ 0.9375f, 0.4375f, 0.8125f, 0.3125}};
			*/
			float ditherValue = ditherPatternOptimized[position_screen.x * _ScreenParams.x % 4][position_screen.y * _ScreenParams.y % 4] * UNITY_FOUR_PI;
			return lerp(0, ditherValue, NGSS_BANDING_TO_NOISE_RATIO_DIR);
			#else
			float2 magic = float2(
				23.14069263277926, // e^pi (Gelfond's constant)
				 2.665144142690225 // 2^sqrt(2) (Gelfondâ€“Schneider constant)
			);
			return lerp(0, frac(cos(dot(position_screen, magic)) * 12345.6789), NGSS_BANDING_TO_NOISE_RATIO_DIR);
			#endif
		}
		
		float2 VogelDiskSampleOptimized(int sampleIndex, int samplesCount, float phi)
		{
			//float phi = 3.14159265359f;//UNITY_PI;
			float GoldenAngle = 2.4f;

			float r = sqrt(sampleIndex + 0.5f) / sqrt(samplesCount);
			float theta = sampleIndex * GoldenAngle + phi;

			float sine, cosine;
			sincos(theta, sine, cosine);
			
			return float2(r * cosine, r * sine);
		}
		
		float PCF_FILTER_SPOT_OPTIMIZED(float4 coord, float diskRadius, float randPied, int samplers)
		{
			float result = 0.0;
			float sampleDepth = coord.z;
			
			for (int i = 0; i < samplers; ++i)
			{
				float2 rotatedOffset = VogelDiskSampleOptimized(i, samplers, randPied) * diskRadius;
				
				result += UNITY_SAMPLE_SHADOW(_ShadowMapTexture, float3(coord.xy + rotatedOffset, sampleDepth)).r;//coord.zw
			}
			half shadow = result / samplers;

			return shadow;
		}
		
        inline fixed unitySampleShadowNGSS (unityShadowCoord4 shadowCoord, float2 screenPos)
        {
		
		#if defined(NGSS_ROTATED_DISK_OPTIMIZED)
			/*************************************************************************************/
			//ROTATED DITHERED DISK. Uncomment #define NGSS_ROTATED_DISK_OPTIMIZED to enable this filter
			
			float diskRadius = clamp(NGSS_GLOBAL_SOFTNESS_OPTIMIZED, 0.1, 2.0) * 0.000275f;//(1.0 - _LightShadowData.r);
			
			float4 coord = shadowCoord;
			//coord.xyz /= coord.w;
			
			#if defined(NGSS_SHADOWS_DITHERING_OPTIMIZED)//defined(SHADOWS_SOFT)
			float randPied = InterleavedGradientNoiseOptimized(screenPos) * UNITY_TWO_PI;
			#else			
				#if defined(NGSS_PCSS_RANDOM_ROTATED_TEXTURE_DEFINED)
				float2 spos = tex2D(unity_RandomRotation16, screenPos.xy).xy;//coord.xy / 8?
				#else
				float2 spos = screenPos.xy * _ScreenParams.xy * 16;
				#endif
			float randPied = InterleavedGradientNoiseOptimized(spos) * UNITY_TWO_PI;//6.28318530718f
			#endif
			
			//diskRadius *= 0.0175;
			int samplers = clamp(NGSS_OPTIMIZED_SAMPLERS, 4, 128);//adding a minimal sampling value to avoid black shadowed light
			return PCF_FILTER_SPOT_OPTIMIZED(coord, diskRadius, randPied, samplers) + _LightShadowData.r;
		#else
			/*************************************************************************************/
			//SINGLE PASS 1D-GAUSSIAN-BOX filter. Comment #define NGSS_ROTATED_DISK_OPTIMIZED to enable this filter
			
			//return SampleShadowMapGridPCF(shadowCoord.xyz);
			
			float iterations = 0;
			fixed shadow = 0;
			
			//declare stuff
			float mSize = clamp(NGSS_OPTIMIZED_ITERATIONS, 3, 49);//incoming number is always odd
			float kSize = (mSize-1)/2;
			/*
			float kernel[7];//mSize
			float Z = 0.0;
			
			//create the 1-D kernel			
			float sigma = 7.0;
			for (int j = 0; j <= kSize; ++j)
			{
				kernel[kSize+j] = kernel[kSize-j] = normpdf(float(j), sigma);
			}
			
			//get the normalization factor (as the gaussian has been clamped)
			for (int j = 0; j < mSize; ++j)
			{
				Z += kernel[j];
			}*/
			float softness = clamp(NGSS_GLOBAL_SOFTNESS_OPTIMIZED, 0.1, 2.0) / _ShadowMapTexture_TexelSize.w;
			for (float i=-kSize; i <= kSize; ++i)
			{
				for (float j=-kSize; j <= kSize; ++j)
				{
					shadow += UNITY_SAMPLE_SHADOW(_ShadowMapTexture, float3(shadowCoord.xy + float2(i,j) * softness, shadowCoord.z));
					//shadow += kernel[kSize+j]*kernel[kSize+i] * UNITY_SAMPLE_SHADOW(_ShadowMapTexture, float3(shadowCoord.xy + float2(float(i),float(j)) * softness, shadowCoord.z));
					
					iterations++;
				}
			}
			
			return shadow / iterations + _LightShadowData.r;
			//return shadow / (Z*Z) + _LightShadowData.r;
		#endif
        }
		
		inline fixed unitySampleShadow (unityShadowCoord4 shadowCoord)
        {
			#if defined(NGSS_PCSS_RANDOM_ROTATED_TEXTURE_DEFINED)
			float2 spos = tex2D(unity_RandomRotation16, shadowCoord.xy * _ScreenParams.xy * 16).xy;
			#else
			float2 spos = shadowCoord.xy * _ScreenParams.xy * 16;
			#endif
			return unitySampleShadowNGSS (shadowCoord, spos);
		}
		
    #else // UNITY_SCREENSPACE_SHADOWS
	
		//UNITY_DECLARE_SCREENSPACE_SHADOWMAP(_ShadowMapTexture);
		sampler2D _ShadowMapTexture;
		
        #define TRANSFER_SHADOW(a) a._ShadowCoord = ComputeScreenPos(a.pos);
		
		inline float normpdf(float x, float sigma)
		{
			return 0.39894*exp(-0.5*x*x/(sigma*sigma))/sigma;
		}

		inline float normpdf3(float3 x, float sigma)
		{
			return 0.39894*exp(-0.5*dot(x,x)/(sigma*sigma))/sigma;
		}
		
		uniform float NGSS_DENOISER_SIZE = 512;
		uniform uint NGSS_DENOISER_ITERATIONS = 4;		
		uniform float NGSS_GLOBAL_SOFTNESS_DENOISER = 1;
		#define SIGMA 10.0
		#define BSIGMA 0.1
		#define MSIZE 15
		//MSIZE must match kernel size
		static const float kernel[15] = { 0.031225216, 0.033322271, 0.035206333, 0.036826804, 0.038138565, 0.039104044, 0.039695028, 0.039894000, 0.039695028, 0.039104044, 0.038138565, 0.036826804, 0.035206333, 0.033322271, 0.031225216 };
		//static const float kernel[13] = { 0.068786, 0.072672, 0.076014, 0.078719, 0.08071, 0.081929, 0.082339, 0.081929, 0.08071, 0.078719, 0.076014, 0.072672, 0.068786 };
		//static const float kernel[11] = { 0.084264, 0.088139, 0.091276, 0.093585, 0.094998, 0.095474, 0.094998, 0.093585, 0.091276, 0.088139, 0.084264 };
		//static const float kernel[9] = { 0.106004, 0.109777, 0.112553, 0.114253, 0.114825, 0.114253, 0.112553, 0.109777, 0.106004 };
		//static const float kernel[7] = { 0.139312, 0.142836, 0.144993, 0.145719, 0.144993, 0.142836, 0.139312 };
		//static const float kernel[5] = { 0.198005, 0.200995, 0.202001, 0.200995, 0.198005 };
		//static const float kernel[3] = { 0.332778, 0.334444, 0.332778 };
				
        inline fixed unitySampleShadow (unityShadowCoord4 shadowCoord)
        {
			//float3 c = tex2D( texture, float2(0.0, 0.0) + (shadowCoord.xy / NGSS_DENOISER_SIZE) ).rgb;
			//fixed shadow = UNITY_SAMPLE_SCREEN_SHADOW(_ShadowMapTexture, float4(shadowCoord.xy, shadowCoord.zw));
			shadowCoord.xyz /= shadowCoord.w;
			fixed shadow = tex2D(_ShadowMapTexture, shadowCoord.xy).r;
			//return shadow;//dont let the denoiser interfere with base filter for now
			
			//declare stuff			
			float final_shadow = 0.0;
			float Z = 0.0;
			
			//create the 1-D kernel	on the fly
			float mSize = clamp(NGSS_DENOISER_ITERATIONS, 3, 49);//incoming number is always odd
			float kSize = (mSize-1)/2;
			/*
			float kernel[15];//mSize
			for (float j = 0; j <= kSize; ++j)
			{
				kernel[kSize+j] = kernel[kSize-j] = normpdf(float(j), SIGMA);
			}*/
						
			float shadowClean;
			float factor;
			float bZ = 1.0/normpdf3(0.0, BSIGMA);
			
			//float softness = NGSS_GLOBAL_SOFTNESS_DENOISER / NGSS_DENOISER_SIZE;//best value for NGSS_DENOISER_SIZE is 512
			float softness = clamp(NGSS_GLOBAL_SOFTNESS_DENOISER, 0.1, 2.0) / clamp(NGSS_DENOISER_SIZE, 128, 1024);//NGSS_GLOBAL_SOFTNESS_DENOISER / _ShadowMapTexture_TexelSize.zw
			
			//read out the texels
			for (float i=-kSize; i <= kSize; ++i)
			{
				for (float j=-kSize; j <= kSize; ++j)
				{
					//shadowClean = tex2D(texture, float2(0.0, 0.0) + ( shadowCoord.xy + float2(float(i),float(j)) ) / 512 ).rgb;
					//shadowClean = UNITY_SAMPLE_SCREEN_SHADOW(_ShadowMapTexture, float4(shadowCoord.xy + float2(float(i),float(j)) * softness, shadowCoord.zw));					
					shadowClean = tex2D(_ShadowMapTexture, shadowCoord.xy + float2(i,j) * softness).r;	
					factor = normpdf3(shadowClean-shadow, BSIGMA)*bZ*kernel[kSize+j]*kernel[kSize+i];//normpdf3
					Z += factor;
					final_shadow += factor*shadowClean;
				}
			}
			
			return final_shadow/Z;
        }
	#endif
	
    #define SHADOW_COORDS(idx1) unityShadowCoord4 _ShadowCoord : TEXCOORD##idx1;
    #define SHADOW_ATTENUATION(a) unitySampleShadow(a._ShadowCoord)
#endif

// -----------------------------
//  Shadow helpers (5.6+ version)
// -----------------------------
// This version depends on having worldPos available in the fragment shader and using that to compute light coordinates.
// if also supports ShadowMask (separately baked shadows for lightmapped objects)

half UnityComputeForwardShadows(float2 lightmapUV, float3 worldPos, float4 screenPos)
{
    //fade value
    float zDist = dot(_WorldSpaceCameraPos - worldPos, UNITY_MATRIX_V[2].xyz);
    float fadeDist = UnityComputeShadowFadeDistance(worldPos, zDist);
    half  realtimeToBakedShadowFade = UnityComputeShadowFade(fadeDist);

    //baked occlusion if any
    half shadowMaskAttenuation = UnitySampleBakedOcclusion(lightmapUV, worldPos);

    half realtimeShadowAttenuation = 1.0f;
    //directional realtime shadow
    #if defined (SHADOWS_SCREEN)
        #if defined(UNITY_NO_SCREENSPACE_SHADOWS) && !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
			float4 spos = ComputeScreenPos(UnityWorldToClipPos(worldPos));
            realtimeShadowAttenuation = unitySampleShadowNGSS(mul(unity_WorldToShadow[0], unityShadowCoord4(worldPos, 1)), (spos.xyz /= spos.w).xy);//NGSS OPTIMZIED DIRECTIONAL
        #else
            //Only reached when LIGHTMAP_ON is NOT defined (and thus we use interpolator for screenPos rather than lightmap UVs). See HANDLE_SHADOWS_BLENDING_IN_GI below.
            realtimeShadowAttenuation = unitySampleShadow(screenPos);//NGSS DENOISER
        #endif
    #endif

    #if defined(UNITY_FAST_COHERENT_DYNAMIC_BRANCHING) && defined(SHADOWS_SOFT) && !defined(LIGHTMAP_SHADOW_MIXING)
    //avoid expensive shadows fetches in the distance where coherency will be good
    UNITY_BRANCH
    if (realtimeToBakedShadowFade < (1.0f - 1e-2f))
    {
    #endif

        //spot realtime shadow
        #if (defined (SHADOWS_DEPTH) && defined (SPOT))
            #if !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
                unityShadowCoord4 spotShadowCoord = mul(unity_WorldToShadow[0], unityShadowCoord4(worldPos, 1));
            #else
                unityShadowCoord4 spotShadowCoord = screenPos;
            #endif
			#if defined (NGSS_SUPPORT_LOCAL)
			float4 spos = ComputeScreenPos(UnityWorldToClipPos(worldPos));
            realtimeShadowAttenuation = UnitySampleShadowmapNGSS(spotShadowCoord, (spos.xyz /= spos.w).xy);
			#else
			realtimeShadowAttenuation = UnitySampleShadowmap(spotShadowCoord);
			#endif
        #endif

        //point realtime shadow
        #if defined (SHADOWS_CUBE)
			#if defined (NGSS_SUPPORT_LOCAL)
			float4 spos = ComputeScreenPos(UnityWorldToClipPos(worldPos));			
            realtimeShadowAttenuation = UnitySampleShadowmapNGSS(worldPos - _LightPositionRange.xyz, (spos.xyz /= spos.w).xy);
			#else
			realtimeShadowAttenuation = UnitySampleShadowmap(worldPos - _LightPositionRange.xyz);
			#endif
        #endif

    #if defined(UNITY_FAST_COHERENT_DYNAMIC_BRANCHING) && defined(SHADOWS_SOFT) && !defined(LIGHTMAP_SHADOW_MIXING)
    }
    #endif

    return UnityMixRealtimeAndBakedShadows(realtimeShadowAttenuation, shadowMaskAttenuation, realtimeToBakedShadowFade);
}

#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D12) || defined(SHADER_API_XBOXONE) || defined(SHADER_API_PSSL)
#   define UNITY_SHADOW_W(_w) _w
#else
#   define UNITY_SHADOW_W(_w) (1.0/_w)
#endif

#if !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
#    define UNITY_READ_SHADOW_COORDS(input) 0
#else
#    define UNITY_READ_SHADOW_COORDS(input) READ_SHADOW_COORDS(input)
#endif

#if defined(HANDLE_SHADOWS_BLENDING_IN_GI) // handles shadows in the depths of the GI function for performance reasons
#   define UNITY_SHADOW_COORDS(idx1) SHADOW_COORDS(idx1)
#   define UNITY_TRANSFER_SHADOW(a, coord) TRANSFER_SHADOW(a)
#   define UNITY_SHADOW_ATTENUATION(a, worldPos) SHADOW_ATTENUATION(a)
#elif defined(SHADOWS_SCREEN) && !defined(LIGHTMAP_ON) && !defined(UNITY_NO_SCREENSPACE_SHADOWS) // no lightmap uv thus store screenPos instead
    // can happen if we have two directional lights. main light gets handled in GI code, but 2nd dir light can have shadow screen and mask.
    // - Disabled on ES2 because WebGL 1.0 seems to have junk in .w (even though it shouldn't)
#   if defined(SHADOWS_SHADOWMASK) && !defined(SHADER_API_GLES)
#       define UNITY_SHADOW_COORDS(idx1) unityShadowCoord4 _ShadowCoord : TEXCOORD##idx1;
#       define UNITY_TRANSFER_SHADOW(a, coord) {a._ShadowCoord.xy = coord * unity_LightmapST.xy + unity_LightmapST.zw; a._ShadowCoord.zw = ComputeScreenPos(a.pos).xy;}
#       define UNITY_SHADOW_ATTENUATION(a, worldPos) UnityComputeForwardShadows(a._ShadowCoord.xy, worldPos, float4(a._ShadowCoord.zw, 0.0, UNITY_SHADOW_W(a.pos.w)));
#   else
#       define UNITY_SHADOW_COORDS(idx1) SHADOW_COORDS(idx1)
#       define UNITY_TRANSFER_SHADOW(a, coord) TRANSFER_SHADOW(a)
#       define UNITY_SHADOW_ATTENUATION(a, worldPos) UnityComputeForwardShadows(0, worldPos, a._ShadowCoord)
#   endif
#else
#   define UNITY_SHADOW_COORDS(idx1) unityShadowCoord4 _ShadowCoord : TEXCOORD##idx1;
#   if defined(SHADOWS_SHADOWMASK)
#       define UNITY_TRANSFER_SHADOW(a, coord) a._ShadowCoord.xy = coord.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#       if (defined(SHADOWS_DEPTH) || defined(SHADOWS_SCREEN) || defined(SHADOWS_CUBE) || UNITY_LIGHT_PROBE_PROXY_VOLUME)
#           define UNITY_SHADOW_ATTENUATION(a, worldPos) UnityComputeForwardShadows(a._ShadowCoord.xy, worldPos, UNITY_READ_SHADOW_COORDS(a))
#       else
#           define UNITY_SHADOW_ATTENUATION(a, worldPos) UnityComputeForwardShadows(a._ShadowCoord.xy, 0, 0)
#       endif
#   else
#       if !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
#           define UNITY_TRANSFER_SHADOW(a, coord)
#       else
#           define UNITY_TRANSFER_SHADOW(a, coord) TRANSFER_SHADOW(a)
#       endif
#       if (defined(SHADOWS_DEPTH) || defined(SHADOWS_SCREEN) || defined(SHADOWS_CUBE))
#           define UNITY_SHADOW_ATTENUATION(a, worldPos) UnityComputeForwardShadows(0, worldPos, UNITY_READ_SHADOW_COORDS(a))
#       else
#           if UNITY_LIGHT_PROBE_PROXY_VOLUME
#               define UNITY_SHADOW_ATTENUATION(a, worldPos) UnityComputeForwardShadows(0, worldPos, UNITY_READ_SHADOW_COORDS(a))
#           else
#               define UNITY_SHADOW_ATTENUATION(a, worldPos) UnityComputeForwardShadows(0, 0, 0)
#           endif
#       endif
#   endif
#endif

#ifdef POINT
sampler2D_float _LightTexture0;
unityShadowCoord4x4 unity_WorldToLight;
#   define UNITY_LIGHT_ATTENUATION(destName, input, worldPos) \
        unityShadowCoord3 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(worldPos, 1)).xyz; \
        fixed shadow = UNITY_SHADOW_ATTENUATION(input, worldPos); \
        fixed destName = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).r * shadow;
#endif

#ifdef SPOT
sampler2D_float _LightTexture0;
unityShadowCoord4x4 unity_WorldToLight;
sampler2D_float _LightTextureB0;
inline fixed UnitySpotCookie(unityShadowCoord4 LightCoord)
{
    return tex2D(_LightTexture0, LightCoord.xy / LightCoord.w + 0.5).w;
}
inline fixed UnitySpotAttenuate(unityShadowCoord3 LightCoord)
{
    return tex2D(_LightTextureB0, dot(LightCoord, LightCoord).xx).r;
}
#if !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
#define DECLARE_LIGHT_COORD(input, worldPos) unityShadowCoord4 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(worldPos, 1))
#else
#define DECLARE_LIGHT_COORD(input, worldPos) unityShadowCoord4 lightCoord = input._LightCoord
#endif
#   define UNITY_LIGHT_ATTENUATION(destName, input, worldPos) \
        DECLARE_LIGHT_COORD(input, worldPos); \
        fixed shadow = UNITY_SHADOW_ATTENUATION(input, worldPos); \
        fixed destName = (lightCoord.z > 0) * UnitySpotCookie(lightCoord) * UnitySpotAttenuate(lightCoord.xyz) * shadow;
#endif

#ifdef DIRECTIONAL
#   define UNITY_LIGHT_ATTENUATION(destName, input, worldPos) fixed destName = UNITY_SHADOW_ATTENUATION(input, worldPos);
#endif

#ifdef POINT_COOKIE
samplerCUBE_float _LightTexture0;
unityShadowCoord4x4 unity_WorldToLight;
sampler2D_float _LightTextureB0;
#   if !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
#       define DECLARE_LIGHT_COORD(input, worldPos) unityShadowCoord3 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(worldPos, 1)).xyz
#   else
#       define DECLARE_LIGHT_COORD(input, worldPos) unityShadowCoord3 lightCoord = input._LightCoord
#   endif
#   define UNITY_LIGHT_ATTENUATION(destName, input, worldPos) \
        DECLARE_LIGHT_COORD(input, worldPos); \
        fixed shadow = UNITY_SHADOW_ATTENUATION(input, worldPos); \
        fixed destName = tex2D(_LightTextureB0, dot(lightCoord, lightCoord).rr).r * texCUBE(_LightTexture0, lightCoord).w * shadow;
#endif

#ifdef DIRECTIONAL_COOKIE
sampler2D_float _LightTexture0;
unityShadowCoord4x4 unity_WorldToLight;
#   if !defined(UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS)
#       define DECLARE_LIGHT_COORD(input, worldPos) unityShadowCoord2 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(worldPos, 1)).xy
#   else
#       define DECLARE_LIGHT_COORD(input, worldPos) unityShadowCoord2 lightCoord = input._LightCoord
#   endif
#   define UNITY_LIGHT_ATTENUATION(destName, input, worldPos) \
        DECLARE_LIGHT_COORD(input, worldPos); \
        fixed shadow = UNITY_SHADOW_ATTENUATION(input, worldPos); \
        fixed destName = tex2D(_LightTexture0, lightCoord).w * shadow;
#endif


// -----------------------------
//  Light/Shadow helpers (4.x version)
// -----------------------------
// This version computes light coordinates in the vertex shader and passes them to the fragment shader.

// ---- Spot light shadows
#if defined (SHADOWS_DEPTH) && defined (SPOT)
#define SHADOW_COORDS(idx1) unityShadowCoord4 _ShadowCoord : TEXCOORD##idx1;
#define TRANSFER_SHADOW(a) a._ShadowCoord = mul (unity_WorldToShadow[0], mul(unity_ObjectToWorld,v.vertex));
#define SHADOW_ATTENUATION(a) UnitySampleShadowmap(a._ShadowCoord)
#endif

// ---- Point light shadows
#if defined (SHADOWS_CUBE)
#define SHADOW_COORDS(idx1) unityShadowCoord3 _ShadowCoord : TEXCOORD##idx1;
#define TRANSFER_SHADOW(a) a._ShadowCoord.xyz = mul(unity_ObjectToWorld, v.vertex).xyz - _LightPositionRange.xyz;
#define SHADOW_ATTENUATION(a) UnitySampleShadowmap(a._ShadowCoord)
#define READ_SHADOW_COORDS(a) unityShadowCoord4(a._ShadowCoord.xyz, 1.0)
#endif

// ---- Shadows off
#if !defined (SHADOWS_SCREEN) && !defined (SHADOWS_DEPTH) && !defined (SHADOWS_CUBE)
#define SHADOW_COORDS(idx1)
#define TRANSFER_SHADOW(a)
#define SHADOW_ATTENUATION(a) 1.0
#define READ_SHADOW_COORDS(a) 0
#else
#ifndef READ_SHADOW_COORDS
#define READ_SHADOW_COORDS(a) a._ShadowCoord
#endif
#endif

#ifdef POINT
#   define DECLARE_LIGHT_COORDS(idx) unityShadowCoord3 _LightCoord : TEXCOORD##idx;
#   define COMPUTE_LIGHT_COORDS(a) a._LightCoord = mul(unity_WorldToLight, mul(unity_ObjectToWorld, v.vertex)).xyz;
#   define LIGHT_ATTENUATION(a)    (tex2D(_LightTexture0, dot(a._LightCoord,a._LightCoord).rr).r * SHADOW_ATTENUATION(a))
#endif

#ifdef SPOT
#   define DECLARE_LIGHT_COORDS(idx) unityShadowCoord4 _LightCoord : TEXCOORD##idx;
#   define COMPUTE_LIGHT_COORDS(a) a._LightCoord = mul(unity_WorldToLight, mul(unity_ObjectToWorld, v.vertex));
#   define LIGHT_ATTENUATION(a)    ( (a._LightCoord.z > 0) * UnitySpotCookie(a._LightCoord) * UnitySpotAttenuate(a._LightCoord.xyz) * SHADOW_ATTENUATION(a) )
#endif

#ifdef DIRECTIONAL
#   define DECLARE_LIGHT_COORDS(idx)
#   define COMPUTE_LIGHT_COORDS(a)
#   define LIGHT_ATTENUATION(a) SHADOW_ATTENUATION(a)
#endif

#ifdef POINT_COOKIE
#   define DECLARE_LIGHT_COORDS(idx) unityShadowCoord3 _LightCoord : TEXCOORD##idx;
#   define COMPUTE_LIGHT_COORDS(a) a._LightCoord = mul(unity_WorldToLight, mul(unity_ObjectToWorld, v.vertex)).xyz;
#   define LIGHT_ATTENUATION(a)    (tex2D(_LightTextureB0, dot(a._LightCoord,a._LightCoord).rr).r * texCUBE(_LightTexture0, a._LightCoord).w * SHADOW_ATTENUATION(a))
#endif

#ifdef DIRECTIONAL_COOKIE
#   define DECLARE_LIGHT_COORDS(idx) unityShadowCoord2 _LightCoord : TEXCOORD##idx;
#   define COMPUTE_LIGHT_COORDS(a) a._LightCoord = mul(unity_WorldToLight, mul(unity_ObjectToWorld, v.vertex)).xy;
#   define LIGHT_ATTENUATION(a)    (tex2D(_LightTexture0, a._LightCoord).w * SHADOW_ATTENUATION(a))
#endif

#define UNITY_LIGHTING_COORDS(idx1, idx2) DECLARE_LIGHT_COORDS(idx1) UNITY_SHADOW_COORDS(idx2)
#define LIGHTING_COORDS(idx1, idx2) DECLARE_LIGHT_COORDS(idx1) SHADOW_COORDS(idx2)
#define UNITY_TRANSFER_LIGHTING(a, coord) COMPUTE_LIGHT_COORDS(a) UNITY_TRANSFER_SHADOW(a, coord)
#define TRANSFER_VERTEX_TO_FRAGMENT(a) COMPUTE_LIGHT_COORDS(a) TRANSFER_SHADOW(a)

#endif
