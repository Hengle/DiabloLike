    Shader "aRPG/OutlinedRimBumpSpec" {
        Properties {
            _Color ("Main Color", Color) = (1.0,1.0,1.0,1.0)
            _SpecColor ("Specular Color", Color) = (1.0,1.0,1.0,1.0)
            _Shininess ("Shininess", Range (0.01, 1)) = 0.1
            _MainTex ("Base (RGB)", 2D) = "white" {}
            _BumpMap ("Normalmap", 2D) = "bump" {}
            _OutlineColor ("Outline Color", Color) = (0,0,0,1)
            _Outline ("Outline width", Range (0.0000, 0.009)) = 0.005
			_RimColor ("Rim Color", Color) = (1.0,1.0,1.0,1.0)
			_RimPower ("Rim Power", Range(0.1,5.0)) = 3.0
        }
     
        SubShader {
			Tags { "RenderType"="Opaque" }
            
		
        
    	UsePass "Toon/Basic Outline/OUTLINE"

			Tags { "RenderType" = "Opaque" }
			CGPROGRAM
			#pragma surface surf BlinnPhong
			
			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float3 viewDir;
			};

			sampler2D _MainTex;
			sampler2D _BumpMap;
			half _Shininess;
			fixed4 _Color; 
			float4 _RimColor;
			float _RimPower;

			void surf (Input IN, inout SurfaceOutput o) {
				o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color;
				o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
				o.Gloss = tex2D(_MainTex, IN.uv_MainTex).a; 
				o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a * _Color; 
				o.Specular = _Shininess;
				half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
				o.Emission = o.Albedo * _RimColor.rgb * pow (rim, _RimPower);
			}





		ENDCG
		
        }
            
        Fallback "Diffuse"
    }
