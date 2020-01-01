﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Xffect/PerObj/blur_conetap" {
Properties {
	_MainTex ("", 2D) = "" {}
}
Subshader {
	ZTest Always Cull Off ZWrite Off Fog { Mode Off }

CGINCLUDE
#include "UnityCG.cginc"

sampler2D _MainTex;
ENDCG


// -- BlurPass
	 Pass {
	  ZTest Always Cull Off ZWrite Off
	  Fog { Mode off }      

	  CGPROGRAM
	  #pragma fragmentoption ARB_precision_hint_fastest
	  #pragma vertex vert
	  #pragma fragment frag
	  
	  #include "UnityCG.cginc"
		struct v2f {
			float4 pos : POSITION;
			half2 uv : TEXCOORD0;
			half2 taps[4] : TEXCOORD1; 
		};
		half4 _MainTex_TexelSize;
		half4 _BlurOffsets;
		v2f vert( appdata_img v ) {
			v2f o; 
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord - _BlurOffsets.xy * _MainTex_TexelSize.xy; // hack, _BlurOffsets corresponding to the offsets set by scripts.
			o.taps[0] = o.uv + _MainTex_TexelSize * _BlurOffsets.xy;
			o.taps[1] = o.uv - _MainTex_TexelSize * _BlurOffsets.xy;
			o.taps[2] = o.uv + _MainTex_TexelSize * _BlurOffsets.xy * half2(1,-1);
			o.taps[3] = o.uv - _MainTex_TexelSize * _BlurOffsets.xy * half2(1,-1);
			return o;
		}
		half4 frag(v2f i) : COLOR {
			half4 color = tex2D(_MainTex, i.taps[0]);
			color += tex2D(_MainTex, i.taps[1]);
			color += tex2D(_MainTex, i.taps[2]);
			color += tex2D(_MainTex, i.taps[3]); 
			return color * 0.25;
		}
	  ENDCG
  }

}

Fallback off
}
