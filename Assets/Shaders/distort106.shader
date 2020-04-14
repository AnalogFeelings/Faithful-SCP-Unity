// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "custom/distort106"
{
	Properties
	{
		_MainTex("Albedo", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		_DistortionScale("Distortion Scale", Range( 0.2 , 15)) = 15
		_DistortionSpeed("Distortion Speed", Range( 0 , 1)) = 0.4146917
		_DistortionEffect("Distortion Effect", Range( 0 , 1)) = 1
		_DistortionMask("DistortionMask", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows nolightmap  nodynlightmap nodirlightmap nofog 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _NormalMap;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _DistortionSpeed;
		uniform float _DistortionScale;
		uniform float _DistortionEffect;
		uniform sampler2D _DistortionMask;
		uniform float4 _DistortionMask_ST;
		uniform float _Metallic;
		uniform float _Smoothness;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv0_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float mulTime9 = _Time.y * _DistortionSpeed;
			float2 temp_cast_0 = (mulTime9).xx;
			float2 uv_TexCoord3 = i.uv_texcoord + temp_cast_0;
			float simplePerlin2D7 = snoise( uv_TexCoord3*_DistortionScale );
			simplePerlin2D7 = simplePerlin2D7*0.5 + 0.5;
			float2 temp_cast_1 = (( simplePerlin2D7 * ( _DistortionEffect * 0.03 ) )).xx;
			float2 blendOpSrc13 = uv0_MainTex;
			float2 blendOpDest13 = temp_cast_1;
			float2 uv_DistortionMask = i.uv_texcoord * _DistortionMask_ST.xy + _DistortionMask_ST.zw;
			float4 clampResult32 = clamp( tex2D( _DistortionMask, uv_DistortionMask ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 blendOpSrc25 = float4( abs( blendOpSrc13 - blendOpDest13 ), 0.0 , 0.0 );
			float4 blendOpDest25 = ( float4( uv0_MainTex, 0.0 , 0.0 ) * clampResult32 );
			float4 temp_output_25_0 = 	max( blendOpSrc25, blendOpDest25 );
			o.Normal = tex2D( _NormalMap, temp_output_25_0.rg ).rgb;
			o.Albedo = tex2D( _MainTex, temp_output_25_0.rg ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17900
0;0;1280;707;574.6931;43.56351;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;11;-1187.868,-233.7164;Inherit;False;Property;_DistortionSpeed;Distortion Speed;3;0;Create;True;0;0;False;0;0.4146917;0.075;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-1055.269,-141.4162;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1055.228,313.8898;Inherit;False;Property;_DistortionEffect;Distortion Effect;4;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-884.0647,399.2668;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;0.03;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1090.7,-59.50002;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-1202.167,63.98381;Inherit;False;Property;_DistortionScale;Distortion Scale;2;0;Create;True;0;0;False;0;15;15;0.2;15;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;16;-1282.688,170.0206;Inherit;True;Property;_DistortionMask;DistortionMask;5;0;Create;True;0;0;False;0;9a207cd3a4cb76649bbac10b357b0ba6;9a207cd3a4cb76649bbac10b357b0ba6;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;7;-831.6679,102.9837;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-964.9369,470.923;Inherit;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-708.0647,319.2668;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-802,-199;Inherit;True;Property;_MainTex;Albedo;0;0;Create;False;0;0;False;0;c91626291a81a8f4e885ebea0e65eca1;c91626291a81a8f4e885ebea0e65eca1;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ClampOpNode;32;-541.5178,511.2679;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-522.2275,207.8898;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-540.4665,-93.31637;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-295.5601,442.5586;Inherit;True;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;13;-257.2654,183.2838;Inherit;True;Difference;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;5;44.72252,11.05842;Inherit;True;Property;_NormalMap;Normal Map;1;0;Create;True;0;0;False;0;7566cbda3776a9c468a4bb57b4e394b8;7566cbda3776a9c468a4bb57b4e394b8;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.BlendOpsNode;25;42.9353,322.2668;Inherit;True;Lighten;False;3;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;30;629.1279,125.4232;Inherit;False;Property;_Metallic;Metallic;6;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;210.9328,-199.9162;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;641.1447,218.5521;Inherit;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;309.6853,10.26332;Inherit;True;Property;_TextureSample2;Texture Sample 2;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1003.384,-191.3987;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;custom/distort106;False;False;False;False;False;False;True;True;True;True;False;False;False;False;False;False;False;False;True;True;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;11;0
WireConnection;3;1;9;0
WireConnection;7;0;3;0
WireConnection;7;1;8;0
WireConnection;19;0;16;0
WireConnection;28;0;15;0
WireConnection;28;1;27;0
WireConnection;32;0;19;0
WireConnection;14;0;7;0
WireConnection;14;1;28;0
WireConnection;12;2;4;0
WireConnection;24;0;12;0
WireConnection;24;1;32;0
WireConnection;13;0;12;0
WireConnection;13;1;14;0
WireConnection;25;0;13;0
WireConnection;25;1;24;0
WireConnection;6;0;4;0
WireConnection;6;1;25;0
WireConnection;29;0;5;0
WireConnection;29;1;25;0
WireConnection;0;0;6;0
WireConnection;0;1;29;0
WireConnection;0;3;30;0
WireConnection;0;4;31;0
ASEEND*/
//CHKSM=CC94D3B5FBE5248810A961FB685B931E8FBF008F