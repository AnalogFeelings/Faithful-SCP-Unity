// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "custom/oilShader"
{
	Properties
	{
		_baseColor("baseColor", Color) = (0,0,0,0)
		_smoothness("smoothness", Range( 0 , 1)) = 0
		_metallic("metallic", Range( 0 , 1)) = 0
		[NoScaleOffset][Normal]_bump("bump", 2D) = "bump" {}
		[NoScaleOffset][Normal]_bump2("bump2", 2D) = "bump" {}
		_bumpSpeed1("bumpSpeed1", Float) = 1
		_bumpSpeed2("bumpSpeed2", Float) = -1
		_IOR("IOR", Range( 0 , 1)) = 0
		_bumpStrength1("bumpStrength1", Float) = 0
		_bumpStrength2("bumpStrength2", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform sampler2D _bump;
		uniform float _bumpSpeed1;
		uniform float _bumpStrength1;
		uniform sampler2D _bump2;
		uniform float _bumpSpeed2;
		uniform float _bumpStrength2;
		uniform float4 _baseColor;
		uniform float _IOR;
		uniform float _metallic;
		uniform float _smoothness;


		struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1);
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1);
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (( _Time.y * _bumpSpeed1 )).xx;
			float2 uv_TexCoord26 = i.uv_texcoord + temp_cast_0;
			float2 temp_output_2_0_g5 = uv_TexCoord26;
			float2 break6_g5 = temp_output_2_0_g5;
			float temp_output_25_0_g5 = ( pow( 0.5 , 3.0 ) * 0.1 );
			float2 appendResult8_g5 = (float2(( break6_g5.x + temp_output_25_0_g5 ) , break6_g5.y));
			float4 tex2DNode14_g5 = tex2D( _bump, temp_output_2_0_g5 );
			float temp_output_4_0_g5 = _bumpStrength1;
			float3 appendResult13_g5 = (float3(1.0 , 0.0 , ( ( tex2D( _bump, appendResult8_g5 ).g - tex2DNode14_g5.g ) * temp_output_4_0_g5 )));
			float2 appendResult9_g5 = (float2(break6_g5.x , ( break6_g5.y + temp_output_25_0_g5 )));
			float3 appendResult16_g5 = (float3(0.0 , 1.0 , ( ( tex2D( _bump, appendResult9_g5 ).g - tex2DNode14_g5.g ) * temp_output_4_0_g5 )));
			float3 normalizeResult22_g5 = normalize( cross( appendResult13_g5 , appendResult16_g5 ) );
			float2 temp_cast_1 = (( _Time.y * _bumpSpeed2 )).xx;
			float2 uv_TexCoord28 = i.uv_texcoord + temp_cast_1;
			float2 temp_output_2_0_g4 = uv_TexCoord28;
			float2 break6_g4 = temp_output_2_0_g4;
			float temp_output_25_0_g4 = ( pow( 0.5 , 3.0 ) * 0.1 );
			float2 appendResult8_g4 = (float2(( break6_g4.x + temp_output_25_0_g4 ) , break6_g4.y));
			float4 tex2DNode14_g4 = tex2D( _bump2, temp_output_2_0_g4 );
			float temp_output_4_0_g4 = _bumpStrength2;
			float3 appendResult13_g4 = (float3(1.0 , 0.0 , ( ( tex2D( _bump2, appendResult8_g4 ).g - tex2DNode14_g4.g ) * temp_output_4_0_g4 )));
			float2 appendResult9_g4 = (float2(break6_g4.x , ( break6_g4.y + temp_output_25_0_g4 )));
			float3 appendResult16_g4 = (float3(0.0 , 1.0 , ( ( tex2D( _bump2, appendResult9_g4 ).g - tex2DNode14_g4.g ) * temp_output_4_0_g4 )));
			float3 normalizeResult22_g4 = normalize( cross( appendResult13_g4 , appendResult16_g4 ) );
			float3 temp_output_32_0 = BlendNormals( normalizeResult22_g5 , normalizeResult22_g4 );
			o.Normal = temp_output_32_0;
			Gradient gradient6 = NewGradient( 0, 3, 2, float4( 0.04460192, 0, 1, 0 ), float4( 1, 0, 0.5081825, 0.5029374 ), float4( 0, 1, 0.01039314, 1 ), 0, 0, 0, 0, 0, float2( 1, 0.002944991 ), float2( 1, 0.9735256 ), 0, 0, 0, 0, 0, 0 );
			float3 newWorldNormal11 = normalize( (WorldNormalVector( i , temp_output_32_0 )) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult13 = dot( newWorldNormal11 , ase_worldViewDir );
			float fresnelNdotV7 = dot( newWorldNormal11, ase_worldViewDir );
			float ior7 = _IOR;
			ior7 = pow( max( ( 1 - ior7 ) / ( 1 + ior7 ) , 0.0001 ), 2 );
			float fresnelNode7 = ( ior7 + ( 1.0 - ior7 ) * pow( max( 1.0 - fresnelNdotV7 , 0.0001 ), 5 ) );
			float4 lerpResult17 = lerp( _baseColor , SampleGradient( gradient6, dotResult13 ) , fresnelNode7);
			o.Albedo = lerpResult17.rgb;
			o.Metallic = _metallic;
			o.Smoothness = _smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17900
35;74;1280;951;1988.65;547.2357;1.3;True;True
Node;AmplifyShaderEditor.RangedFloatNode;23;-2168.186,292.5933;Inherit;False;Property;_bumpSpeed1;bumpSpeed1;7;0;Create;True;0;0;False;0;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;22;-2272.438,182.5495;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2340.01,514.6114;Inherit;False;Property;_bumpSpeed2;bumpSpeed2;8;0;Create;True;0;0;False;0;-1;0.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-2046.559,470.2078;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1977.057,182.5497;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1749.451,356.2643;Inherit;False;Property;_bumpStrength2;bumpStrength2;11;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-1793.651,445.1102;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-1754.651,39.06433;Inherit;False;Property;_bumpStrength1;bumpStrength1;10;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;20;-1686.186,-317.4747;Inherit;True;Property;_bump2;bump2;6;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;336510b840517584c9bf41ec50d009ac;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-1702.914,178.6884;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;18;-1997.635,-114.4036;Inherit;True;Property;_bump;bump;5;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;336510b840517584c9bf41ec50d009ac;d3c458add63ea8c4ea69fafb2fbd3039;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FunctionNode;37;-1395.85,7.864209;Inherit;True;NormalCreate;1;;5;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;38;-1372.451,300.3643;Inherit;False;NormalCreate;1;;4;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;32;-1106.77,122.8592;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;12;-1113.209,-145.3123;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;11;-1123.209,-350.3123;Inherit;True;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GradientNode;6;-743.3746,-392.5345;Inherit;False;0;3;2;0.04460192,0,1,0;1,0,0.5081825,0.5029374;0,1,0.01039314,1;1,0.002944991;1,0.9735256;0;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1095.111,34.59738;Inherit;False;Property;_IOR;IOR;9;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;13;-816.6132,-256.1406;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;7;-785.4804,-102.5942;Inherit;True;SchlickIOR;WorldNormal;ViewDir;False;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-752.7592,134.6427;Inherit;False;Property;_baseColor;baseColor;0;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientSampleNode;5;-658.8635,-259.21;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-152.233,148.65;Inherit;False;Property;_metallic;metallic;4;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;17;-355.6006,-84.35847;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-167.8331,75.85002;Inherit;False;Property;_smoothness;smoothness;3;0;Create;True;0;0;False;0;0;0.85;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;219.2,-13;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;custom/oilShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;29;0;22;0
WireConnection;29;1;30;0
WireConnection;27;0;22;0
WireConnection;27;1;23;0
WireConnection;28;1;29;0
WireConnection;26;1;27;0
WireConnection;37;1;18;0
WireConnection;37;2;26;0
WireConnection;37;4;39;0
WireConnection;38;1;20;0
WireConnection;38;2;28;0
WireConnection;38;4;40;0
WireConnection;32;0;37;0
WireConnection;32;1;38;0
WireConnection;11;0;32;0
WireConnection;13;0;11;0
WireConnection;13;1;12;0
WireConnection;7;0;11;0
WireConnection;7;2;33;0
WireConnection;5;0;6;0
WireConnection;5;1;13;0
WireConnection;17;0;1;0
WireConnection;17;1;5;0
WireConnection;17;2;7;0
WireConnection;0;0;17;0
WireConnection;0;1;32;0
WireConnection;0;3;3;0
WireConnection;0;4;2;0
ASEEND*/
//CHKSM=F597ED52F9E2CFAE78C2B555183D179A308D48B6