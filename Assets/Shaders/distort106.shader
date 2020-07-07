// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "custom/distort106"
{
	Properties
	{
		_MainTex("Albedo", 2D) = "white" {}
		[Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		_DistortionSpeed("Distortion Speed", Range( 0 , 1)) = 0.4146917
		_DistortionEffect("Distortion Effect", Range( 0 , 1)) = 1
		_DistortionMask("DistortionMask", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_distortNoise("distortNoise", 2D) = "white" {}
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
		uniform sampler2D _DistortionMask;
		uniform float4 _DistortionMask_ST;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _distortNoise;
		uniform float _DistortionSpeed;
		uniform float _DistortionEffect;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_DistortionMask = i.uv_texcoord * _DistortionMask_ST.xy + _DistortionMask_ST.zw;
			float2 uv0_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float mulTime9 = _Time.y * _DistortionSpeed;
			float2 temp_cast_1 = (mulTime9).xx;
			float2 uv_TexCoord3 = i.uv_texcoord + temp_cast_1;
			float layeredBlendVar41 = tex2D( _DistortionMask, uv_DistortionMask ).r;
			float4 layeredBlend41 = ( lerp( ( float4( uv0_MainTex, 0.0 , 0.0 ) + ( tex2D( _distortNoise, uv_TexCoord3 ) * ( _DistortionEffect * 0.03 ) ) ),float4( uv0_MainTex, 0.0 , 0.0 ) , layeredBlendVar41 ) );
			o.Normal = UnpackNormal( tex2D( _NormalMap, layeredBlend41.rg ) );
			o.Albedo = tex2D( _MainTex, layeredBlend41.rg ).rgb;
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
1360;-256;1280;963;1711.807;593.4058;1.3;True;True
Node;AmplifyShaderEditor.RangedFloatNode;11;-1187.868,-233.7164;Inherit;False;Property;_DistortionSpeed;Distortion Speed;2;0;Create;True;0;0;False;0;0.4146917;0.075;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-1297.575,-122.3585;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1055.228,313.8898;Inherit;False;Property;_DistortionEffect;Distortion Effect;3;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-884.0647,399.2668;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;0.03;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1164.208,-7.771746;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;42;-1093.06,-476.5823;Inherit;True;Property;_distortNoise;distortNoise;7;0;Create;True;0;0;False;0;a5ac8abb713d8d042873bf12ef702ca1;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-708.0647,319.2668;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-802,-199;Inherit;True;Property;_MainTex;Albedo;0;0;Create;False;0;0;False;0;c91626291a81a8f4e885ebea0e65eca1;c91626291a81a8f4e885ebea0e65eca1;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;43;-922.9028,51.59073;Inherit;True;Property;_TextureSample3;Texture Sample 3;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;16;-1282.688,170.0206;Inherit;True;Property;_DistortionMask;DistortionMask;4;0;Create;True;0;0;False;0;9a207cd3a4cb76649bbac10b357b0ba6;9a207cd3a4cb76649bbac10b357b0ba6;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-393.4494,-116.4579;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-473.2217,220.1412;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;19;-964.9369,470.923;Inherit;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-127.6624,244.8339;Inherit;True;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LayeredBlendNode;41;146.407,287.0815;Inherit;True;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;5;44.72252,11.05842;Inherit;True;Property;_NormalMap;Normal Map;1;1;[Normal];Create;True;0;0;False;0;7566cbda3776a9c468a4bb57b4e394b8;7566cbda3776a9c468a4bb57b4e394b8;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;6;210.9328,-199.9162;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;629.1279,125.4232;Inherit;False;Property;_Metallic;Metallic;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;641.1447,218.5521;Inherit;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;309.6853,10.26332;Inherit;True;Property;_TextureSample2;Texture Sample 2;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1003.384,-191.3987;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;custom/distort106;False;False;False;False;False;False;True;True;True;True;False;False;False;False;False;False;False;False;True;True;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;11;0
WireConnection;3;1;9;0
WireConnection;28;0;15;0
WireConnection;28;1;27;0
WireConnection;43;0;42;0
WireConnection;43;1;3;0
WireConnection;12;2;4;0
WireConnection;14;0;43;0
WireConnection;14;1;28;0
WireConnection;19;0;16;0
WireConnection;45;0;12;0
WireConnection;45;1;14;0
WireConnection;41;0;19;1
WireConnection;41;1;45;0
WireConnection;41;2;12;0
WireConnection;6;0;4;0
WireConnection;6;1;41;0
WireConnection;29;0;5;0
WireConnection;29;1;41;0
WireConnection;0;0;6;0
WireConnection;0;1;29;0
WireConnection;0;3;30;0
WireConnection;0;4;31;0
ASEEND*/
//CHKSM=77D5AEC9DB5240574F15192694E70B3837D838D1