﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// MatCap Shader, (c) 2013,2014 Jean Moreno

Shader "Xffect/clip/matcap_add"
{
	Properties
	{
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MatCap ("MatCap (RGB)", 2D) = "white" {}
	}
	
	Subshader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend One OneMinusSrcColor
		Cull Off
		Lighting Off
		ZWrite Off
		
		Pass
		{
			Tags { "LightMode" = "Always" }
			
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
				
				struct v2f
				{
					float4 pos	: SV_POSITION;
					float2 cap	: TEXCOORD0;
					float3 wpos : TEXCOORD1;
				};
				
				v2f vert (appdata_base v)
				{
					v2f o;

					o.wpos = mul (unity_ObjectToWorld, v.vertex);

					o.pos = UnityObjectToClipPos (v.vertex);
					
					half2 capCoord;
					capCoord.x = dot(UNITY_MATRIX_IT_MV[0].xyz,v.normal);
					capCoord.y = dot(UNITY_MATRIX_IT_MV[1].xyz,v.normal);
					o.cap = capCoord * 0.5 + 0.5;
					
					return o;
				}
				
				uniform float4 _Color;
				uniform sampler2D _MatCap;
				uniform float4 _ClipParam;
				
				float4 frag (v2f i) : COLOR
				{
					float4 mc = tex2D(_MatCap, i.cap);
					
					float dist = distance(i.wpos, float4(_ClipParam.x,_ClipParam.y,_ClipParam.z,1.0));
					if (dist < _ClipParam.w) 
						mc = fixed4(0,0,0,0);

					return _Color * mc * 2.0;
				}
			ENDCG
		}
	}
	
	Fallback "VertexLit"
}