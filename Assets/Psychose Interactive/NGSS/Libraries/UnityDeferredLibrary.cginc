#ifndef UNITY_DEFERRED_LIBRARY_INCLUDED
#define UNITY_DEFERRED_LIBRARY_INCLUDED

// Deferred lighting / shading helpers


// --------------------------------------------------------
// Vertex shader

struct unity_v2f_deferred {
    float4 pos : SV_POSITION;
    float4 uv : TEXCOORD0;
    float3 ray : TEXCOORD1;
};

float _LightAsQuad;

unity_v2f_deferred vert_deferred (float4 vertex : POSITION, float3 normal : NORMAL)
{
    unity_v2f_deferred o;
    o.pos = UnityObjectToClipPos(vertex);
    o.uv = ComputeScreenPos(o.pos);
    o.ray = UnityObjectToViewPos(vertex) * float3(-1,-1,1);

    // normal contains a ray pointing from the camera to one of near plane's
    // corners in camera space when we are drawing a full screen quad.
    // Otherwise, when rendering 3D shapes, use the ray calculated here.
    o.ray = lerp(o.ray, normal, _LightAsQuad);

    return o;
}


// --------------------------------------------------------
// Shared uniforms


UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

float4 _LightDir;
float4 _LightPos;
float4 _LightColor;
float4 unity_LightmapFade;
float4x4 unity_WorldToLight;
sampler2D_float _LightTextureB0;

#if defined (POINT_COOKIE)
samplerCUBE_float _LightTexture0;
#else
sampler2D_float _LightTexture0;
#endif

#if defined (SHADOWS_SCREEN)
sampler2D _ShadowMapTexture;
#endif

#if defined (SHADOWS_SHADOWMASK)
sampler2D _CameraGBufferTexture4;
#endif

// --------------------------------------------------------
// Shadow/fade helpers

// Receiver plane depth bias create artifacts when depth is retrieved from
// the depth buffer. see UnityGetReceiverPlaneDepthBias in UnityShadowLibrary.cginc
#ifdef UNITY_USE_RECEIVER_PLANE_BIAS
    #undef UNITY_USE_RECEIVER_PLANE_BIAS
#endif

#include "UnityShadowLibrary.cginc"


//Note :
// SHADOWS_SHADOWMASK + LIGHTMAP_SHADOW_MIXING -> ShadowMask mode
// SHADOWS_SHADOWMASK only -> Distance shadowmask mode

// --------------------------------------------------------
half UnityDeferredSampleShadowMask(float2 uv)
{
    half shadowMaskAttenuation = 1.0f;

    #if defined (SHADOWS_SHADOWMASK)
        half4 shadowMask = tex2D(_CameraGBufferTexture4, uv);
        shadowMaskAttenuation = saturate(dot(shadowMask, unity_OcclusionMaskSelector));
    #endif

    return shadowMaskAttenuation;
}
/////////////////////////////////NGSS CONTACT SHADOWS//////////////////////////

sampler2D NGSS_ContactShadowsTexture;
half4 NGSS_ContactShadowsTexture_ST;
float ShadowsOpacity;

/////////////////////////////////NGSS DENOISER/////////////////////////////////

inline float normpdf3( float3 x, float sigma)
{
	return 0.39894*exp(-0.5*dot(x,x)/(sigma*sigma))/sigma;
}

uniform float NGSS_DENOISER_SIZE = 512;
uniform uint NGSS_DENOISER_ITERATIONS = 4;		
uniform float NGSS_GLOBAL_SOFTNESS_DENOISER = 1;
#define SIGMA 10.0
#define BSIGMA 0.1

static const float kernel[15] = { 0.031225216, 0.033322271, 0.035206333, 0.036826804, 0.038138565, 0.039104044, 0.039695028, 0.039894000, 0.039695028, 0.039104044, 0.038138565, 0.036826804, 0.035206333, 0.033322271, 0.031225216 };

// --------------------------------------------------------
half UnityDeferredSampleRealtimeShadow(half fade, float3 vec, float2 uv)
{
    half shadowAttenuation = 1.0f;

    #if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
        #if defined(SHADOWS_SCREEN)			
			#if defined(UNITY_NO_SCREENSPACE_SHADOWS)//CASCADED SHADOWS OFF
				
				shadowAttenuation = tex2D(_ShadowMapTexture, uv).r;

			#else
				half shadow = tex2D(_ShadowMapTexture, uv).r;
				
				//return shadow;//dont let the denoiser interfere with base filter for now
				
				//declare stuff			
				float final_shadow = 0.0;
				float Z = 0.0;
				
				float mSize = clamp(NGSS_DENOISER_ITERATIONS, 3, 49);//incoming number is always odd
				float kSize = (mSize-1)/2;
				
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
						//shadowClean = tex2D(texture, float2(0.0, 0.0) + ( uv.xy + float2(float(i),float(j)) ) / 512 ).rgb;
						shadowClean = tex2D(_ShadowMapTexture, uv.xy + float2(i,j) * softness);					
						factor = normpdf3(shadowClean-shadow, BSIGMA)*bZ*kernel[kSize+j]*kernel[kSize+i];//normpdf3
						Z += factor;
						final_shadow += factor*shadowClean;
					}
				}
				
				shadowAttenuation = final_shadow/Z;
			#endif
        #endif
    #endif

    #if defined(UNITY_FAST_COHERENT_DYNAMIC_BRANCHING) && defined(SHADOWS_SOFT) && !defined(LIGHTMAP_SHADOW_MIXING)
    //avoid expensive shadows fetches in the distance where coherency will be good
    UNITY_BRANCH
    if (fade < (1.0f - 1e-2f))
    {
    #endif

        #if defined(SPOT)
            #if defined(SHADOWS_DEPTH)
                float4 shadowCoord = mul(unity_WorldToShadow[0], float4(vec, 1));
				#if defined (NGSS_SUPPORT_LOCAL)
                shadowAttenuation = UnitySampleShadowmapNGSS(shadowCoord, uv);
				#else
				shadowAttenuation = UnitySampleShadowmap(shadowCoord);
				#endif
            #endif
        #endif

        #if defined (POINT) || defined (POINT_COOKIE)
            #if defined(SHADOWS_CUBE)
				#if defined (NGSS_SUPPORT_LOCAL)
                shadowAttenuation = UnitySampleShadowmapNGSS(vec, uv);
				#else
				shadowAttenuation = UnitySampleShadowmap(vec);
				#endif
            #endif
        #endif

    #if defined(UNITY_FAST_COHERENT_DYNAMIC_BRANCHING) && defined(SHADOWS_SOFT) && !defined(LIGHTMAP_SHADOW_MIXING)
    }
    #endif
	
	//shadowAttenuation *= saturate(tex2D(NGSS_ContactShadowsTexture, UnityStereoScreenSpaceUVAdjust(uv, NGSS_ContactShadowsTexture_ST)) + ShadowsOpacity);
	//shadowAttenuation *= saturate(tex2D(NGSS_ContactShadowsTexture, uv) + ShadowsOpacity);

    return shadowAttenuation;
}

// --------------------------------------------------------
half UnityDeferredComputeShadow(float3 vec, float fadeDist, float2 uv)
{

    half fade                      = UnityComputeShadowFade(fadeDist);
    half shadowMaskAttenuation     = UnityDeferredSampleShadowMask(uv);
    half realtimeShadowAttenuation = UnityDeferredSampleRealtimeShadow(fade, vec, uv);

    return UnityMixRealtimeAndBakedShadows(realtimeShadowAttenuation, shadowMaskAttenuation, fade);
}

// --------------------------------------------------------
// Common lighting data calculation (direction, attenuation, ...)
void UnityDeferredCalculateLightParams (
    unity_v2f_deferred i,
    out float3 outWorldPos,
    out float2 outUV,
    out half3 outLightDir,
    out float outAtten,
    out float outFadeDist)
{
    i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
    float2 uv = i.uv.xy / i.uv.w;

    // read depth and reconstruct world position
    float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
    depth = Linear01Depth (depth);
    float4 vpos = float4(i.ray * depth,1);
    float3 wpos = mul (unity_CameraToWorld, vpos).xyz;

    float fadeDist = UnityComputeShadowFadeDistance(wpos, vpos.z);

    // spot light case
    #if defined (SPOT)
        float3 tolight = _LightPos.xyz - wpos;
        half3 lightDir = normalize (tolight);

        float4 uvCookie = mul (unity_WorldToLight, float4(wpos,1));
        // negative bias because http://aras-p.info/blog/2010/01/07/screenspace-vs-mip-mapping/
        float atten = tex2Dbias (_LightTexture0, float4(uvCookie.xy / uvCookie.w, 0, -8)).w;
        atten *= uvCookie.w < 0;
        float att = dot(tolight, tolight) * _LightPos.w;
        atten *= tex2D (_LightTextureB0, att.rr).r;

        atten *= UnityDeferredComputeShadow (wpos, fadeDist, uv);

    // directional light case
    #elif defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
        half3 lightDir = -_LightDir.xyz;
        float atten = 1.0;

        atten *= UnityDeferredComputeShadow (wpos, fadeDist, uv);

        #if defined (DIRECTIONAL_COOKIE)
        atten *= tex2Dbias (_LightTexture0, float4(mul(unity_WorldToLight, half4(wpos,1)).xy, 0, -8)).w;
        #endif //DIRECTIONAL_COOKIE

    // point light case
    #elif defined (POINT) || defined (POINT_COOKIE)
        float3 tolight = wpos - _LightPos.xyz;
        half3 lightDir = -normalize (tolight);

        float att = dot(tolight, tolight) * _LightPos.w;
        float atten = tex2D (_LightTextureB0, att.rr).r;

        atten *= UnityDeferredComputeShadow (tolight, fadeDist, uv);

        #if defined (POINT_COOKIE)
        atten *= texCUBEbias(_LightTexture0, float4(mul(unity_WorldToLight, half4(wpos,1)).xyz, -8)).w;
        #endif //POINT_COOKIE
    #else
        half3 lightDir = 0;
        float atten = 0;
    #endif

    outWorldPos = wpos;
    outUV = uv;
    outLightDir = lightDir;
    outAtten = atten;
    outFadeDist = fadeDist;
}

#endif // UNITY_DEFERRED_LIBRARY_INCLUDED
